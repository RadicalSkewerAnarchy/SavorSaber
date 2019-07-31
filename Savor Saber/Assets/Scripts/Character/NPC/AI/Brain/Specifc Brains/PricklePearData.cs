using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PricklePearData : AIData
{
    public GameObject electricAoE;
    /// <summary>
    /// if enemies around
    ///     if hp is above 1/4
    ///         RangedAttack
    ///     else Runaway
    /// else if friends around
    ///     if also named ghostpepper
    ///         Chase
    ///     else Runaway
    /// else Wander
    /// </summary>
    public override Protocols DecideProtocol()
    {
        Protocols proto = currentProtocol;

        if (this.Checks.NumberOfEnemies() > 0)
        {
            if (Checks.ClosestDrone() != null)
            {
                if (health > (maxHealth / 2))
                    proto = Protocols.Melee;
                else
                    proto = Protocols.Runaway;
            }
            else
                proto = Protocols.Runaway;
        }
        else
        {
            proto = Protocols.Wander;
        }

        return proto;
    }

    public override void WhileOvercharged()
    {
        Protocol.Melee(null);
    }

    public override void OnStateExit(LifeState s)
    {
        switch (s)
        {
            case LifeState.overcharged:
                electricAoE.GetComponent<SpriteRenderer>().color = new Color(255,255,255,255);
                electricAoE.GetComponent<PoweredObjectCharger>().enabled = false;   
                electricAoE.GetComponent<ElectricAOE>().overCharged = false;    
                break;
            default:
                // nothing at all
                break;
        }
    }

    public override void OnStateEnter(LifeState s)
    {
        switch (s)
        {
            case LifeState.overcharged:
                electricAoE.GetComponent<SpriteRenderer>().color = new Color(0,244,255,255);
                electricAoE.GetComponent<PoweredObjectCharger>().enabled = true;
                electricAoE.GetComponent<ElectricAOE>().overCharged = true;
                break;
            default:
                // nothing at all
                break;
        }
    }
}
