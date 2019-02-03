using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    #region fields

    public Sprite[] portraitSprites;

    public string[] currentSpeaker;

    public string[] text;

    public GameObject dialogBoxPrefab;
    public Canvas UICanvas;


    private GameObject dialogBox;

    #endregion

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(dialogBox.transform.localPosition);
    }

    public void Activate()
    {
        dialogBox = Instantiate(dialogBoxPrefab, Vector3.zero, Quaternion.identity);
        dialogBox.transform.SetParent(UICanvas.transform);
        RectTransform rectTransform = dialogBox.GetComponent<RectTransform>();
        dialogBox.transform.localScale = new Vector3(1, 1, 1);
        rectTransform.anchoredPosition = new Vector3(0, 50, 0);
    }
}
