using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DialogManualTrigger : MonoBehaviour
{

    public GameObject dialogTarget;
    private SpriteRenderer spriteRenderer;
    private BaseDialog dialog;
    private bool playingDialog = false;
    private bool playerInRange = false;
    private Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        dialog = dialogTarget.GetComponent<BaseDialog>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        //reset playing state if the dialog has finished.
        if (dialog != null && dialog.dialogFinished)
            playingDialog = false;

        if (playerInRange && InputManager.GetButtonDown(Control.Interact) && !playingDialog)
        {
            if (dialog != null)
            {
                playingDialog = true;
                dialog.Activate(false);
            }
            else
            {
                Debug.LogWarning("Warning: No dialog associated with trigger");
            }
        }
    }
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("Player in trigger");
        if(collision.gameObject.tag == "Player" && Input.GetButtonDown("Cook") && !playingDialog)
        {
            if (dialog != null)
            {
                playingDialog = true;
                dialog.Activate();
            }
            else
            {
                Debug.LogWarning("Warning: No dialog associated with trigger");
            }
        }
    }
    */
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            spriteRenderer.color = defaultColor;
            playerInRange = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            spriteRenderer.color = Color.yellow;
            playerInRange = true;
        }
        
    }
}
