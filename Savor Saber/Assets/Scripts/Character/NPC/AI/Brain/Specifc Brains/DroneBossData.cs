using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBossData : AIData
{
    public override Protocols DecideProtocol()
    {
        Protocols proto;

        if (health > 7* (float)maxHealth /8)
        {
            meleeHunter = true;
        }
        else if (health > 3* (float)maxHealth /4)
        {
            meleeHunter = false;
        }
        else if (health > (float)maxHealth /2)
        {
            meleeHunter = true;
        }
        else if (health > (float)maxHealth /3)
        {
            meleeHunter = false;
        }
        else
        {
            meleeHunter = true;
            this.Behavior.attackCooldown = 0.25f;
            this.Behavior.meleeAttackDelay = 0;
        }
        /*
        var weakling = Checks.WeakestCreature();
        var closest = Checks.ClosestCreature(new string[] { "Predator" });
        float weakDist = ( weakling!=null ? Vector2.Distance(this.transform.position, weakling.transform.position) : 10);
        float closeDist = ( closest!=null ? Vector2.Distance(this.transform.position, closest.transform.position) : 10);

        if ((closeDist <= 5 && this.health > maxHealth/2) || closeDist <= 2)
            proto = Protocols.Melee;
        else
            proto = Protocols.Ranged;*/

        if (Checks.NumberOfEnemies() > 0)
            proto = Protocols.Attack;
        else
            proto = Protocols.Wander;
        return proto;
    }
}
