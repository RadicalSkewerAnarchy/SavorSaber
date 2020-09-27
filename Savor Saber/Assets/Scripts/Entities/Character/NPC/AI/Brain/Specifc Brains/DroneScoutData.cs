using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneScoutData : AIData
{
    public override Protocols DecideProtocol()
    {
        return Protocols.Attack;
    }
}
