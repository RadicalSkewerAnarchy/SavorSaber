using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach this script to an empty GameObject to create a dialog sequence.
/// Call Activate() to start it up.
/// </summary>
public class Dialog : BaseDialog
{

    public Sprite[] portraitSprites;
    public string[] currentSpeaker;
  

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cook") && active)
        {
            stage++;
            Debug.Log("Advancing to dialog stage " + stage);
            if(stage >= text.Length)
            {
                Destroy(dialogBox);
                active = false;
                dialogFinished = true;

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

        //set position on of dialog box on screen
        if(dialogBoxPrefab != null)
        {
            dialogBox = Instantiate(dialogBoxPrefab, Vector3.zero, Quaternion.identity);
            dialogBox.transform.SetParent(UICanvas.transform);
            RectTransform rectTransform = dialogBox.GetComponent<RectTransform>();
            dialogBox.transform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchoredPosition = new Vector3(0, 50, 0);

            Debug.Log("stage: " + stage);
            dialogText = dialogBox.GetComponentInChildren<Text>();
            dialogText.text = text[stage];

            Transform portrait = dialogBox.transform.GetChild(1);
            dialogImage = portrait.GetComponent<Image>();
            dialogImage.sprite = portraitSprites[stage];

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
        dialogImage.sprite = portraitSprites[stage];
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
}
