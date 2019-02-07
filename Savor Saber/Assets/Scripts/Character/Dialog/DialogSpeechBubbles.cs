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
                dialogFinished = true;
                ReleaseActors();

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
        dialogFinished = false;
        HoldActors();
        
        if (dialogBoxPrefab != null)
        {
            //set position on of dialog box on screen
            dialogBox = Instantiate(dialogBoxPrefab, Vector3.zero, Quaternion.identity);
            dialogBox.transform.SetParent(UICanvas.transform);
            dialogRectTransform = dialogBox.GetComponent<RectTransform>();
            dialogBox.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            //rectTransform.anchoredPosition = new Vector3(0, 50, 0);
            dialogRectTransform.anchoredPosition = GetActorUISpace(actors[stage]);

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
        dialogRectTransform.anchoredPosition = GetActorUISpace(actors[stage]);
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

    protected Vector2 GetActorUISpace(GameObject actor)
    {
        
        RectTransform canvasRect = UICanvas.GetComponent<RectTransform>();
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(actor.transform.position);

        Vector2 UIOffset = new Vector2((float)canvasRect.sizeDelta.x / 2f, -70f);


        Vector2 proportionalPosition = new Vector3(viewportPosition.x * canvasRect.sizeDelta.x, viewportPosition.y * canvasRect.sizeDelta.y);

        return proportionalPosition - UIOffset;
    }
}
