using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PricklePear : AIData
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
            if (health > (maxHealth / 10))
                proto = Protocols.Melee;
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
            case Protocols.Melee:
                Protocol.Melee(Checks.closestEnemy);
                break;
            // lazy
            case Protocols.Lazy:
                Protocol.Lazy();
                break;
            // Runaway
            case Protocols.Runaway:
                Protocol.NavRunaway(Checks.closestFriend);
                break;
            // Chase
            case Protocols.Chase:
                Protocol.NavChase(Checks.closestFriend, Speed);
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

    public override void WhileOvercharged(){
        currentProtocol = Protocols.Melee;
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
            case LifeState.dead:
                sRenderer.color = Color.white;
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
            case LifeState.dead:
                sRenderer.color = Color.grey;
                break;
            default:
                // nothing at all
                break;
        }
    }
}
