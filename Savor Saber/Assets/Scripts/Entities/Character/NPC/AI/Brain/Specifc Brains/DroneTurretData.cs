using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneTurretData : AIData
{
    public override Protocols DecideProtocol()
    {
        return Protocols.Attack;
    }
}
