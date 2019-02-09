using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Exists so that dialog triggers can activate different styles of dialog
/// </summary>
[RequireComponent(typeof(DialogScene))]
public class BaseDialog : MonoBehaviour
{

    #region fields
    [System.NonSerialized]
    public bool active = false;

    /// <summary>
    /// Signals dialog triggers that this dialog has finished, in case something
    /// needs to happen after it plays.
    /// </summary>
    [System.NonSerialized]
    public bool dialogFinished = false;

    /// <summary>
    /// Can the dialog be played more than once?
    /// </summary>
    public bool repeatable = false;

    /// <summary>
    /// Dialog writers set these fields
    /// </summary>
    public ActorDict actors = new ActorDict();

    /// <summary>
    /// references to objects used for display
    /// </summary>
    public GameObject dialogBoxPrefab;
    public Canvas UICanvas;
    protected GameObject dialogBox;
    protected RectTransform dialogRectTransform;

    protected Text dialogText;
    protected Image dialogImage;
    protected DialogData dialogData;
    protected DialogScene scene;
    #endregion

    public virtual void Activate(bool doFirst)
    {

    }

    public virtual void NextDialog()
    {

    }

    /// <summary>
    /// Stop actors from moving during conversations
    /// </summary>
    protected void HoldActors()
    {
        DialogData actorDialogData;
        foreach(var key in actors.Keys)
        {
            actorDialogData = actors[key].GetComponent<DialogData>();
            actorDialogData.inConversation = true;
        }
    }

    protected void ReleaseActors()
    {
        DialogData actorDialogData;
        foreach (var key in actors.Keys)
        {
            actorDialogData = actors[key].GetComponent<DialogData>();
            actorDialogData.inConversation = false;
        }
    }

    [System.Serializable] public class ActorDict : SerializableCollections.SDictionary<string, GameObject> { };
}
