using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerUpdate : EventTrigger
{
    public string value;
    public string key;

    private void Start()
    {
        FlagManager.SetFlag(key, "null");
    }

    // Update is called once per frame
    void Update()
    {
        if (FlagManager.GetFlag(key) == value)
            Trigger();
    }
}
