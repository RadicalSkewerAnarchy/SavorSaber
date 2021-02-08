using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GenericContactTrigger : PoweredObject
{
    public string tagToCheck;
    public bool repeatable = false;
    private bool triggered = false;
    public UnityEvent callOnActivation = new UnityEvent();


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && collision.gameObject.tag == tagToCheck)
        {
            if(repeatable || !triggered)
            {
                triggered = true;
                callOnActivation.Invoke();
            }
            
        }
    }
}
