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
        return b.toDefaultBranch.connections[0].body as BaseNode;
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
        //Process other node types
        //Recursively move to next
        return NextDialog();
    }

}
