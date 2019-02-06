using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach this script to an empty GameObject to create a dialog sequence.
/// Call Activate() to start it up.
/// </summary>
public class DialogSpeechBubbles : BaseDialog
{
    #region fields

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
    private GameObject dialogBox;

    private Text dialogText;
    private Image dialogImage;
    private DialogData dialogData;

    private int stage = 0;
    private bool active = false;

    #endregion

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cook") && active)
        {
            stage++;
            Debug.Log("Advancing to dialog stage " + stage);
            if (stage >= text.Length)
            {
                Destroy(dialogBox);
                active = false;

                if (!repeatable)
                    Destroy(this.gameObject);
            }
            else
            {
                NextDialog();
            }

        }
    }

    /// <summary>
    /// Call this function to begin dialog
    /// </summary>
    public override void Activate()
    {
        stage = 0;
        
        if (dialogBoxPrefab != null)
        {
            //set position on of dialog box on screen
            dialogBox = Instantiate(dialogBoxPrefab, Vector3.zero, Quaternion.identity);
            dialogBox.transform.SetParent(UICanvas.transform);
            RectTransform rectTransform = dialogBox.GetComponent<RectTransform>();
            dialogBox.transform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchoredPosition = new Vector3(0, 50, 0);

            //set dialog box text
            Debug.Log("stage: " + stage);
            dialogText = dialogBox.GetComponentInChildren<Text>();
            dialogText.text = text[stage];

            //set dialog box portrait
            Transform portrait = dialogBox.transform.GetChild(1);
            dialogImage = portrait.GetComponent<Image>();
            dialogData = actors[stage].GetComponent<DialogData>();
            dialogImage.sprite = dialogData.portraitDictionary[emotions[stage]];

            StartCoroutine(Wait(0.5f));
        }
        else
        {
            Debug.LogWarning("Warning: No dialog box prefab found when trying to activate Dialog");
            Destroy(this.gameObject);
        }

    }

    /// <summary>
    /// Advance to the next dialog stage
    /// </summary>
    public override void NextDialog()
    {
        dialogText.text = text[stage];
        dialogData = actors[stage].GetComponent<DialogData>();
        dialogImage.sprite = dialogData.portraitDictionary[emotions[stage]];
        //dialogImage.sprite = portraitSprites[stage];
    }

    /// <summary>
    /// A short delay before considering the dialog "active" or else the stage
    /// gets advanced by the same key press as activation.
    /// </summary>
    protected IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        active = true;
        yield return null;
    }

    protected Vector3 GetActorUISpace(GameObject actor)
    {
        RectTransform rectTransform = dialogBox.GetComponent<RectTransform>();

        Vector2 actorPosition = actor.transform.position;

        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(actorPosition);
        //Vector2 proportionalPosition = new Vector2(viewportPosition.x * Canvas.sizeDelta.x, viewportPosition.y * Canvas.sizeDelta.y);

        return new Vector3(0, 50, 0);
    }
}
