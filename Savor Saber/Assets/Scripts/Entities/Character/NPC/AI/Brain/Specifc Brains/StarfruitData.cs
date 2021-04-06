using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfruitData : AIData
{
    //public GameObject normalProjectile;
    //public GameObject overchargedProjectile;
    public GameObject TeslaField;
    private FlavorInputManager fim;

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
        Protocols p = currentProtocol;

        if (Checks.NumberOfEnemies() > 0)
        {
            p = Protocols.Runaway;
        }
        else
        {
            int friends = Checks.NumberOfFriends();
            if (friends >= 0 && friends < 9)
                p = Protocols.Wander;
            else if (friends < 12)
                p = Protocols.Party;
            else
                p = Protocols.Runaway;
        }

        return p;
    }

    public override void OnStateExit(LifeState s)
    {
        switch (s)
        {
            case LifeState.overcharged:
                TeslaField.SetActive(false);
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
                TeslaField.SetActive(true);
                break;
            default:
                // nothing at all
                break;
        }
    }
}
