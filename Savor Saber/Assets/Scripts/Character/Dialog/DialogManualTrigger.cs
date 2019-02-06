using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManualTrigger : MonoBehaviour
{

    public GameObject dialogTarget;
    private BaseDialog dialog;
    // Start is called before the first frame update
    void Start()
    {
        dialog = dialogTarget.GetComponent<BaseDialog>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && Input.GetButtonDown("Cook"))
        {
            if(dialog != null)
            {

                dialog.Activate();
            }
            else
            {
                Debug.LogWarning("Warning: No dialog associated with trigger");
            }
        }
    }
}
