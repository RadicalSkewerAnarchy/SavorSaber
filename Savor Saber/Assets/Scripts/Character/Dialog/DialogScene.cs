using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameflow;

public class DialogScene : MonoBehaviour
{
    [SerializeField] private DialogCanvas graph = null;
    private BaseNode currNode = null;
    /// <summary> Initialized the root node (for if next dialogue is called in DialogManager's awake function </summary>
    public void ResetScene()
    {
        currNode = graph.getStartNode();
    }
    private void Start()
    {
        currNode = graph.getStartNode();
    }
    private BaseNode Next()
    {
        if (currNode is BaseNodeOUT)
            return (currNode as BaseNodeOUT).Next;
        else if (currNode is GameflowBranchNode)
            return Branch(currNode as GameflowBranchNode);
        else
            return null;
    }
    private BaseNode Branch(GameflowBranchNode b)
    {
        string value = string.Empty;
        if (b.exprType == GameflowBranchNode.controlExpressionType.Last_Input)
            return b.toDefaultBranch.connection(0).body as BaseNode;
        else
            value = FlagManager.GetFlag(b.variableName);
        foreach (var brCase in b.cases)
        {
            if (brCase.type == GameflowBranchNode.BranchCase.CaseType.Regex)
            {
                if (CheckRegexCase(brCase.pattern, value))
                    return brCase.connection.connections[0].body as BaseNode;
            }
            else if (CheckTextCase(brCase.pattern, value))//brCase.type == BranchCaseData.CaseType.Text
                return brCase.connection.connections[0].body as BaseNode;
        }
        return b.toDefaultBranch.connection(0).body as BaseNode;
    }
    private bool CheckTextCase(string pattern, string value)
    {
        //Probably should compress this to regex
        return value.Trim().ToLower() == pattern.Trim().ToLower().Replace(".", string.Empty).Replace("?", string.Empty).Replace("!", string.Empty);
    }
    private bool CheckRegexCase(string pattern, string value)
    {
        throw new System.NotImplementedException();
    }
    /// <summary> Go through the graph, porcessing nodes until a dialog node is reached
    /// When reached, translate into a dialog item and return </summary>
    public DialogItem NextDialog()
    {
        currNode = Next();
        if (currNode == null)
            return null;
        if (currNode is DialogNode)
        {
            if (currNode is DialogNodeVN)
            {
                var dNode = currNode as DialogNodeVN;
                return new DialogItem(dNode.text, dNode.actor, dNode.emotion);
            }
        }
        else if(currNode is SetFlagNode)
        {
            var node = currNode as SetFlagNode;
            FlagManager.SetFlag(node.flagName, node.value);
        }
        //Process other node types
        //Recursively move to next
        return NextDialog();
    }

}
