using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddlePearData : AIData
{
    public override Protocols DecideProtocol()
    {
        Protocols p = currentProtocol;
        if (GetOvercharged())
        {
            p = Protocols.Pollinate;
        }
        else
        {
            if(Checks.NumberOfEnemies() > 0)
            {
                p = Protocols.Runaway;
            }
            else
            {
                p = Protocols.Chase;
            }
        }

        return p;
    }

    public override void WhileOvercharged()
    {
        Protocol.Pollinate();
    }

    public override void OnStateEnter(LifeState s)
    {
        switch(s)
        {
            case LifeState.overcharged:
                this.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            default:
                break;
        }
    }
}
