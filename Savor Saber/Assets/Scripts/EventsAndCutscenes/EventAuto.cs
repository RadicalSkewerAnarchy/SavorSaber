using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EventAuto : MonoBehaviour
{
    public UnityEvent eventToCall;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Firing automatic event");
        eventToCall.Invoke();
        //Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
