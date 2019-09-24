using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAreaTextAnimShutoff : MonoBehaviour
{
    public NewAreaText textTrigger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableTrigger()
    {
        Debug.Log("Called EnableTrigger successfully");
        if(textTrigger != null)
            textTrigger.textPlaying = false;

        Destroy(transform.parent.gameObject);
    }
}
