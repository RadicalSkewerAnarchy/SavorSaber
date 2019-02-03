using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    #region fields

    /// <summary>
    /// Can the dialog be repeated?
    /// </summary>
    public bool repeatable = false;

    /// <summary>
    /// Arrays for the dialog content itself
    /// </summary>
    public Sprite[] portraitSprites;

    public string[] currentSpeaker;

    public string[] text;

    public GameObject dialogBoxPrefab;
    public Canvas UICanvas;


    private GameObject dialogBox;
    private Text dialogText;
    private Image dialogImage;
    
    private int stage = 0;
    private bool active = false;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        

    }

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
                if (!repeatable)
                    Destroy(this.gameObject);
            }
            else
            {
                NextDialog();
            }

        }
    }

    public void Activate()
    {
        Debug.Log("Activating dialog");
        stage = 0;
        //set position on of dialog box on screen
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

    public void NextDialog()
    {
        dialogText.text = text[stage];
        dialogImage.sprite = portraitSprites[stage];
    }

    protected IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        active = true;
        yield return null;
    }
}
