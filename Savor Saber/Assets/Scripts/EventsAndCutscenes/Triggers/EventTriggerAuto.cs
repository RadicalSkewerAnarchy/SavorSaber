using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerAuto : EventTrigger
{
    public string flagToDisable;
    public string valueToDisable;
    private void Start()
    {
        if (FlagManager.GetFlag(flagToDisable) == valueToDisable)
            return;
        else
            Trigger();
    }
    protected override IEnumerator PlayEvent()
    {
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        yield return StartCoroutine(base.PlayEvent());
    }
}
