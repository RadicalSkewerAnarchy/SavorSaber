using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Gameflow;
using SerializableCollections;

[RequireComponent(typeof(DialogPlayer))]
public class EventGraph : MonoBehaviour
{ 
    [SerializeField] private ActorDict _actors = new ActorDict();
    public ActorDict Actors { get => _actors; }
    [SerializeField] private GameObjectDict _dependencies = new GameObjectDict();
    public GameObjectDict Dependencies { get => _dependencies; }
    public UnityEvent specialEvent;
    [SerializeField] private EventDict _events = new EventDict();
    public EventDict Events { get => _events; }
    public DialogCanvas Graph = null;//{ get; set; } = null;
    private DialogPlayer dialog;
    private GameObject player;
    private BaseNode currNode = null;
    private BaseNode lastNode = null;
    private bool stopped = false;

    private void Update()
    {

    }
    /// <summary> Initialized the root node (for if next dialogue is called in DialogManager's awake function </summary>
    public void ResetScene()
    {
        currNode = Graph.getStartNode();
        dialog.Initialize();
    }
    /// <summary> Resume playing the event graph if it had been previously stopped</summary>
    public void Restart()
    {
        stopped = false;
        Debug.Log("Restarting cutscene " + this.gameObject.name);
    }

    private void Awake()
    {
        dialog = GetComponent<DialogPlayer>();
        currNode = Graph.getStartNode();
    }

    private void Initialize()
    {
        player = PlayerController.instance.gameObject;
        if (_actors.ContainsKey("Soma"))
        {
            _actors["Soma"] = player.GetComponent<DialogData>();
        }
        else
            _actors.Add("Soma", player.GetComponent<DialogData>());
    }

    #region Branching and graph traversal
    private BaseNode Next()
    {
        if (currNode is BaseNodeOUT)
            return (currNode as BaseNodeOUT).Next;
        else if (currNode is GameflowBranchNode)
            return Branch(currNode as GameflowBranchNode);
        else if (currNode is DialogEndNode)
        {
            var trig = GetComponent<EventTrigger>();
            if (trig != null)
                trig.playCompletionEvents = (currNode as DialogEndNode).playCompletionEvents;
            return null;
        }
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
    #endregion

    /// <summary> Go through the graph, iterating through sequential events but returning events that should be waited for</summary>
    public IEnumerator Play()
    {
        Initialize();
        lastNode = currNode;
        currNode = Next();
        while (currNode != null)
        {
            if (lastNode is DialogNode && !(currNode is DialogNode))
                dialog.Visible = false;
            if (currNode is DialogNode)
            {
                if (currNode is DialogNodeSpeechBubble)
                {
                    var dNode = currNode as DialogNodeSpeechBubble;
                    yield return StartCoroutine(dialog.NextDialogBubble(new DialogItem(dNode.text, dNode.emotion), Actors[dNode.actor]));
                }
                else if (currNode is DialogNodeFixed)
                {
                    var dNode = currNode as DialogNodeFixed;
                    yield return StartCoroutine(dialog.NextDialogBubble(new DialogItem(dNode.text, dNode.emotion), Actors[dNode.actor]));
                }
            }
            else if (currNode is CutsceneEventNode)
            {
                Debug.Log("Found Cutscene Event Node");
                Coroutine routine = null;
                if (currNode is WaitNode)
                    routine = StartCoroutine(CutsceneNodeEvents.Wait(currNode as WaitNode));
                else if (currNode is MoveCharacterNode)
                    routine = StartCoroutine(CutsceneNodeEvents.MoveCharacter(currNode as MoveCharacterNode, Actors, Dependencies));
                else if (currNode is SetCharacterDirectionNode)
                    routine = StartCoroutine(CutsceneNodeEvents.SetCharacterDirection(currNode as SetCharacterDirectionNode, Actors, Dependencies));
                else if (currNode is PanCameraNode)
                    routine = StartCoroutine(CutsceneNodeEvents.PanCamera(currNode as PanCameraNode, player, Actors, Dependencies));
                else if (currNode is PlayUnityEventNode)
                    Events[(currNode as PlayUnityEventNode).eventName].Invoke();
                else if (currNode is GoToSilenceNode)
                    BGMManager.instance?.FadeBGMToSilence((currNode as GoToSilenceNode).fadeTime);
                else if (currNode is SetBgmNode)
                {
                    var node = currNode as SetBgmNode;
                    BGMManager.instance?.CrossFadeBgm(node.bgm, node.fadeTime);
                }
                else if (currNode is GoToAreaMusicNode)
                    BGMManager.instance?.FadeToAreaSounds((currNode as GoToAreaMusicNode).fadeTime);
                if ((currNode as CutsceneEventNode).waitUntilFinished)
                    yield return routine;
            }
            else if (currNode is SetFlagNode)
            {
                var node = currNode as SetFlagNode;
                FlagManager.SetFlag(node.flagName, node.value);
            }
            else if (currNode is SetQuestTextNode)
            {
                var node = currNode as SetQuestTextNode;
                QuestManager.instance?.SetText(node.text);
            }
            else if (currNode is EnableChildObjectNode)
            {
                var node = currNode as EnableChildObjectNode;
                GameObject target = null;
                if (Actors.ContainsKey(node.parent))
                    target = Actors[node.parent].gameObject;
                else if (Dependencies.ContainsKey(node.parent))
                    target = Dependencies[node.parent];
                else
                {
                    Debug.LogError("Improper EnableChildObjectNode config: " + node.parent + " is not an Actor or Depenency");
                }
                var child = target?.transform.Find(node.childName);
                child?.gameObject.SetActive(!node.disable);
            }
            else if (currNode is StopNode)
            {
                stopped = true;
                yield return new WaitWhile(() => stopped);
            }
            lastNode = currNode;
            currNode = Next();
        }
        dialog.Cleanup();
    }
    [System.Serializable] public class ActorDict : SDictionary<string, DialogData> { }
    [System.Serializable] public class EventDict : SDictionary<string, UnityEvent> { }
}
