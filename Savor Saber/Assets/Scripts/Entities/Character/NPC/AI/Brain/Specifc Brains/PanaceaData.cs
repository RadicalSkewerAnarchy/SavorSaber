using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanaceaData : AIData
{
    public override Protocols DecideProtocol()
    {
        return Protocols.Lazy;
    }
}
