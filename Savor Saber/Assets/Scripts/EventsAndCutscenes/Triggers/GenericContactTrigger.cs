using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GenericContactTrigger : PoweredObject
{
    public UnityEvent callOnActivation = new UnityEvent();
    public bool repeatable = false;
    private bool triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && collision.gameObject.tag == "Player")
        {
            if(repeatable || !triggered)
            {
                triggered = true;
                callOnActivation.Invoke();
            }
            
        }
    }
}
