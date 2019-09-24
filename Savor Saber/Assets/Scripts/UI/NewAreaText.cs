using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NewAreaText : MonoBehaviour
{

    public GameObject animTextTemplate;
    public GameObject typingTextTemplate;
    public GameObject targetCanvas;
    public string animatedTextTop;
    public string animatedTextBottom;
    public bool animated = true; //whether to use the animated or typing variant
    public bool repeatable = false;
    public Vector3 color;
    [System.NonSerialized]
    public bool textPlaying = false;
    private bool active = true;
    private GameObject text;

    // Start is called before the first frame update
    void Start()
    {
        //hide the SpriteRenderer used for in-editor visualization
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("text playing? " + textPlaying);
        if (targetCanvas == null)
            targetCanvas = GameObject.Find("DialogCanvas");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(active && !textPlaying && collision.gameObject.tag == "Player")
        {
            if (animated)
            {
                text = Instantiate(animTextTemplate, targetCanvas.transform);
                NewAreaTextAnimShutoff shutoff = text.GetComponentInChildren<NewAreaTextAnimShutoff>();
                shutoff.textTrigger = this;

                //assign proper text and color
                Text textTop = text.transform.GetChild(0).gameObject.GetComponent<Text>();
                Text textBottom = text.transform.GetChild(1).gameObject.GetComponent<Text>();
                textTop.text = animatedTextTop;
                textBottom.text = animatedTextBottom;
                textTop.color = new Color(color.x, color.y, color.z);
                textBottom.color = new Color(color.x, color.y, color.z);
                //play audio as well? 
            }
            else
            {
                //Instantiate the typing variant once that exists
            }

            textPlaying = true;
            if (!repeatable)
                active = false;

        }
    }
}
