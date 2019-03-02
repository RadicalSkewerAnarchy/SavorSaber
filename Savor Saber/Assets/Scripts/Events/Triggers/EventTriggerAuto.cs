using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerAuto : EventTrigger
{
    public bool onStart = true;
    private void Start()
    {
        if (onStart)
            Play();
    }
    private void Awake()
    {
        if (!onStart)
            Play();
    }
    private void Play()
    {
        InitializeBase();
        Trigger();
    }
    protected override IEnumerator PlayEvent()
    {
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        yield return base.PlayEvent();
    }
}
