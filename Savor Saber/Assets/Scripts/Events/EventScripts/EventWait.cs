using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventWait : EventScript
{
    public float time;
    public override IEnumerator PlayEvent(GameObject player)
    {
        yield return new WaitForSeconds(time);
    }
}
