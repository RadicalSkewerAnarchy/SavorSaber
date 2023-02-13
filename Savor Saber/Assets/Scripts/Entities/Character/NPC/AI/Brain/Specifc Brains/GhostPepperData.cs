using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPepperData : AIData
{
    public GameObject normalProjectile;
    public GameObject overchargedProjectile;
    private OverchargeCurryBalls fim;

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
                if (health > (maxHealth / 4))
                    proto = Protocols.Ranged;
                else
                    proto = Protocols.Runaway;
            }
            else
                proto = Protocols.Runaway;
        }
        else if (Checks.NumberOfFriends() > 0)
        {
            var ghost = Checks.FriendQuery(GetComponent<Monster>().name);
            if (ghost != null)
            {
                Checks.closestFriend = ghost;
                proto = Protocols.Chase;
            }
            else
            {
                Checks.closestFriend = null;
                proto = Protocols.Wander;
            }
        }
        else
        {
            proto = Protocols.Wander;
        }

        return proto;
    }

    public override void OnStateExit(LifeState s)
    {
        switch (s)
        {
            case LifeState.overcharged:
                this.Behavior.projectile = normalProjectile;
                this.RangeAttackThreshold = 3.5f;
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
                this.Behavior.projectile = overchargedProjectile;
                if (fim == null)
                    fim = GetComponent<OverchargeCurryBalls>();
                fim.CurryBalls(true);
                this.RangeAttackThreshold = 1;
                break;
            default:
                // nothing at all
                break;
        }
    }
}
