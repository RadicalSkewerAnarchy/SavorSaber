using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneIdleData : AIData
{
    public override Protocols DecideProtocol()
    {
        return Protocols.Lazy;
    }
}
