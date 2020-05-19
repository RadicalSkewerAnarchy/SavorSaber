using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnFlag : MonoBehaviour
{
    public string flag;
    public string value;

    public UnityEvent eventToCall;
    // Start is called before the first frame update
    void Start()
    {
        if (flag == value)
            eventToCall.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
