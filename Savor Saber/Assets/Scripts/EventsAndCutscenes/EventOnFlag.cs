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
        if (FlagManager.GetFlag(flag) == value)
        {
            Debug.Log("EventOnFlag: Flag is correct value");
            eventToCall.Invoke();
        }
        else
            Debug.Log("EventOnFlag: Flag is not correct value");
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
