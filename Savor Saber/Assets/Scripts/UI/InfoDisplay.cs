using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
public class InfoDisplay : MonoBehaviour
{
    public bool isMap = true;
    private Canvas UICanvas;

    /// <summary>
    /// A UI Image showing whatever info you want to appear on-screen
    /// </summary>
    public GameObject[] infoToDisplay;
    public Vector2 youAreHereCoordinates;

    private bool playerInRange = false;
    private bool showingInfo = false;

    private GameObject infoBox;
    private AudioSource audio;
    private int infoIndex = 0;

    private PlayerController controller;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        //UICanvas = GetComponentInChildren<Canvas>();
        UICanvas = GameObject.Find("DialogCanvas").GetComponent<Canvas>();
    }


    void Update()
    {

        CheckOpen();


        if (showingInfo && infoToDisplay.Length > 1 && InputManager.GetButtonDown(Control.Right))
        {
            infoIndex++;
            if (infoIndex >= infoToDisplay.Length)
                infoIndex = 0;

            Destroy(infoBox);
            infoBox = Instantiate(infoToDisplay[infoIndex], new Vector3(0, 0, 0), Quaternion.identity);
            infoBox.transform.SetParent(UICanvas.transform);
            infoBox.transform.localPosition = new Vector3(0, 0, 0);
            infoBox.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (showingInfo && infoToDisplay.Length > 1 && InputManager.GetButtonDown(Control.Left))
        {
            infoIndex--;
            if (infoIndex < 0)
                infoIndex = infoToDisplay.Length - 1;

            Destroy(infoBox);
            infoBox = Instantiate(infoToDisplay[infoIndex], new Vector3(0, 0, 0), Quaternion.identity);
            infoBox.transform.SetParent(UICanvas.transform);
            infoBox.transform.localPosition = new Vector3(0, 0, 0);
            infoBox.transform.localScale = new Vector3(1f, 1f, 1f);
        }

    }

    private void CheckOpen()
    {
        if (InputManager.GetButtonDown(Control.Interact) && playerInRange && !showingInfo)
        {
            audio.pitch = 2;
            audio.Play();

            infoBox = Instantiate(infoToDisplay[infoIndex], new Vector3(0, 0, 0), Quaternion.identity);
            infoBox.transform.SetParent(UICanvas.transform);
            infoBox.transform.localPosition = new Vector3(0, 0, 0);
            infoBox.transform.localScale = new Vector3(1f, 1f, 1f);

            //only use navigation arrows if this is displaying a map
            if (isMap)
            {
                Transform arrow = infoBox.transform.GetChild(0);
                if (arrow != null)
                    arrow.localPosition = youAreHereCoordinates;
            }
            controller.enabled = false;        
            showingInfo = true;
        }
        //close the whole box
        else if (showingInfo && (InputManager.GetButtonDown(Control.Interact) || InputManager.GetButtonDown(Control.Cancel)))
        {
            audio.pitch = 0.75f;
            audio.Play();

            Destroy(infoBox.gameObject);
            controller.enabled = true;
            showingInfo = false;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
            controller = collision.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
            if(showingInfo)
            {
                /*
                audio.pitch = 0.75f;
                audio.Play();

                Destroy(infoBox.gameObject);
                showingInfo = false;
                */
            }
        }
    }


}
