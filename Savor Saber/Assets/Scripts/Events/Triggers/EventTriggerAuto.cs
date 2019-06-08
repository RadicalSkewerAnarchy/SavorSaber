using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerAuto : EventTrigger
{
    private void Start()
    {
        Trigger();
    }
    protected override IEnumerator PlayEvent()
    {
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        yield return StartCoroutine(base.PlayEvent());
    }
}
