using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManualTrigger : MonoBehaviour
{

    public GameObject dialogTarget;
    private BaseDialog dialog;
    private bool playingDialog = false;

    // Start is called before the first frame update
    void Start()
    {
        dialog = dialogTarget.GetComponent<BaseDialog>();
    }

    // Update is called once per frame
    void Update()
    {
        //reset playing state if the dialog has finished.
        if (dialog != null && dialog.dialogFinished)
            playingDialog = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && Input.GetButtonDown("Cook") && !playingDialog)
        {
            if(dialog != null)
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
}
