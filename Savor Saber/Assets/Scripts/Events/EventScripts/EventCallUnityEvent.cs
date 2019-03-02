using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCallUnityEvent : EventScript 
{
    public UnityEvent e;
    public override IEnumerator PlayEvent(GameObject player)
    {
        e.Invoke();
        yield break;
    }
}
