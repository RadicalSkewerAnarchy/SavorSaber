using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reverses power input to target objects.
/// Useful when you want a signal to activate some objects but deactivate others.
/// </summary>
public class LogicNOT : PoweredObject
{
    public PoweredObject[] targetObjects;

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
        active = true;
        foreach(PoweredObject target in targetObjects)
        {
            target.ShutOff();
        }
    }

    public override void ShutOff()
    {
        active = false;
        foreach(PoweredObject target in targetObjects)
        {
            target.TurnOn();
        }
    }
}
