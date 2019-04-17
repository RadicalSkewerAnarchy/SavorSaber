using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        
    }


    void Update()
    {

        if (InputManager.GetButtonDown(Control.Interact)&& playerInRange && !showingInfo)
        {
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
        }
    }


}
