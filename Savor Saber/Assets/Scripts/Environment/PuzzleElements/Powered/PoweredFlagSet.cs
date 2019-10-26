using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredFlagSet : PoweredObject
{

    public string[] flagsToSet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TurnOn()
    {
        foreach(string flag in flagsToSet)
        {
            FlagManager.SetFlag(flag, "true");
        }
    }

    public override void ShutOff()
    {
        foreach(string flag in flagsToSet)
        {
            FlagManager.SetFlag(flag, "false");
        }
    }
}
