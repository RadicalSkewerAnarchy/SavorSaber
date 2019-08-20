using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPepperData : AIData
{
    public GameObject normalProjectile;
    public GameObject overchargedProjectile;
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
        Protocols proto = currentProtocol;
        if (this.Checks.NumberOfEnemies() > 0)
        {
            if (Checks.ClosestDrone() != null)
            {
                if (health > (maxHealth / 2))
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
                proto = Protocols.Runaway;
            }
        }
        else
        {
            proto = Protocols.Wander;
        }

        return proto;
    }

    public override void WhileAlive()
    {
        switch (currentProtocol)
        {
            // ranged
            case Protocols.Ranged:
                Protocol.Ranged();
                break;
            // lazy
            case Protocols.Lazy:
                Protocol.Lazy();
                break;
            // feast
            case Protocols.Feast:
                Protocol.Feast(meleeHunter);
                break;
            // Runaway
            case Protocols.Runaway:
                Protocol.NavRunaway();
                break;
            // Conga
            case Protocols.Conga:
                Protocol.Conga();
                break;
            // Chase
            case Protocols.Chase:
                Protocol.NavChase();
                break;
            // Wander
            case Protocols.Wander:
                Protocol.Wander(5f, 5f);
                break;
            default:
                Debug.Log("YOU SHOULD NEVER BE HERE!");
                break;
        }
    }

    public override void OnStateExit(LifeState s)
    {
        switch (s)
        {
            case LifeState.overcharged:
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
                if (fim == null)
                    fim = GetComponent<FlavorInputManager>();
                fim.CurryBalls(true);
                break;
            default:
                // nothing at all
                break;
        }
    }
}
