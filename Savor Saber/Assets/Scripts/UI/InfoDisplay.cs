using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
public class InfoDisplay : MonoBehaviour
{
    public bool isMap = true;
    public Canvas UICanvas;

    /// <summary>
    /// A UI Image showing whatever info you want to appear on-screen
    /// </summary>
    public GameObject infoToDisplay;
    public Vector2 youAreHereCoordinates;

    private bool playerInRange = false;
    private bool showingInfo = false;

    private GameObject infoBox;
    private AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>(); 
    }


    void Update()
    {

        if (InputManager.GetButtonDown(Control.Interact)&& playerInRange && !showingInfo)
        {
            audio.pitch = 2;
            audio.Play();

            infoBox = Instantiate(infoToDisplay, new Vector3(0, 0, 0), Quaternion.identity);
            infoBox.transform.SetParent(UICanvas.transform);
            infoBox.transform.localPosition = new Vector3(0, 0, 0);
            infoBox.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);

            //only use navigation arrows if this is displaying a map
            if (isMap)
            {
                Transform arrow = infoBox.transform.GetChild(0);
                if(arrow != null)
                    arrow.localPosition = youAreHereCoordinates;
            }


            showingInfo = true;
        }
        else if (showingInfo && (InputManager.GetButtonDown(Control.Interact) || InputManager.GetButtonDown(Control.Cancel)))
        {
            audio.pitch = 0.75f;
            audio.Play();

            Destroy(infoBox.gameObject);
            showingInfo = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
            if(showingInfo)
            {
                audio.pitch = 0.75f;
                audio.Play();

                Destroy(infoBox.gameObject);
                showingInfo = false;
            }
        }
    }


}
