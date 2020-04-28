﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredObject : MonoBehaviour
{

    public bool active = false;
    public bool canBeSourCharged = false;
    public string activeStateFlag;
    // Start is called before the first frame update
    void Start()
    {
        if (active)
        {
            TurnOn();
            FlagManager.SetFlag(activeStateFlag, "True");
        }
        else
        {
            ShutOff();
            FlagManager.SetFlag(activeStateFlag, "False");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TurnOn()
    {
        active = true;
        FlagManager.SetFlag(activeStateFlag, "True");
    }

    public virtual void ShutOff()
    {
        active = false;
        FlagManager.SetFlag(activeStateFlag, "False");
    }
}
