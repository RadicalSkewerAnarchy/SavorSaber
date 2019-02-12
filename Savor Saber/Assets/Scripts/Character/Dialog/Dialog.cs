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
    private DialogItem item;
    private void Start()
    {
        scene = GetComponent<DialogScene>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cook") && active)
        {
            item = scene.NextDialog();
            if (item == null)
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
    public override void Activate(bool doFirst)
    {
        scene.ResetScene();
        dialogFinished = false;
        //set position on of dialog box on screen
        if (dialogBoxPrefab != null)
        {            
            dialogBox = Instantiate(dialogBoxPrefab, Vector3.zero, Quaternion.identity);
            dialogBox.transform.SetParent(UICanvas.transform);
            RectTransform rectTransform = dialogBox.GetComponent<RectTransform>();
            dialogBox.transform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchoredPosition = new Vector3(0, 50, 0);
            dialogText = dialogBox.GetComponentInChildren<Text>();
            Transform portrait = dialogBox.transform.GetChild(1);
            dialogImage = portrait.GetComponent<Image>();
        }
        else
        {
            Debug.LogWarning("Warning: No dialog box prefab found when trying to activate Dialog");
            Destroy(this.gameObject);
        }
        if (doFirst)
        {
            item = scene.NextDialog();
            NextDialog();
        }
        active = true;
    }

    /// <summary>
    /// Advance to the next dialog stage
    /// </summary>
    public override void NextDialog()
    {
        dialogText.text = item.text;
        dialogData = actors[item.actor].GetComponent<DialogData>();
        dialogImage.sprite = dialogData.portraitDictionary[item.emotion];
    }
}
