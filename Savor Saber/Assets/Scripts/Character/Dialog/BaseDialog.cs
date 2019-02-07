using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Exists so that dialog triggers can activate different styles of dialog
/// </summary>
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
    public DialogData.Emotions[] emotions;
    public string[] text;
    public GameObject[] actors;

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

    protected int stage = 0;


    #endregion

    public virtual void Activate()
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
        for(int i = 0; i < actors.Length; i++)
        {
            actorDialogData = actors[i].GetComponent<DialogData>();
            actorDialogData.inConversation = true;
        }
    }

    protected void ReleaseActors()
    {
        DialogData actorDialogData;
        for (int i = 0; i < actors.Length; i++)
        {
            actorDialogData = actors[i].GetComponent<DialogData>();
            actorDialogData.inConversation = false;
        }
    }
}
