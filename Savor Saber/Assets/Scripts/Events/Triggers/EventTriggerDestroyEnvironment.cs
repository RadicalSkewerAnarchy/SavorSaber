using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerDestroyEnvironment : EventTrigger
{
    public DestructableEnvironment[] destroyChecks;
    public bool needAll = true;

    // Start is called before the first frame update
    void Start()
    {
        InitializeBase();
    }

    // Update is called once per frame
    void Update()
    {
        bool trig = false;
        bool all = true;
        foreach (var obj in destroyChecks)
        {
            if (obj.destroyed)
                trig = true;
            else
                all = false;
        }

        if (trig)
        {
            if (needAll && all)
                Trigger();
            else if (!needAll && trig)
                Trigger();
        }
    }
}
