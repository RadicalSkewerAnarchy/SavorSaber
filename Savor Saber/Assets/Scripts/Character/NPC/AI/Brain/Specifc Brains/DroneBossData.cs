using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBossData : AIData
{
    public override Protocols DecideProtocol()
    {
        Protocols proto;

        if (health > 2*maxHealth/3)
        {
            // do nothing for a while
        }
        else if (health > maxHealth/2)
        {
            Speed = 3f;
        }
        else
        {
            this.Behavior.attackCooldown = 0.25f;
            this.Behavior.meleeAttackDelay = 0;
        }

        var weakling = Checks.WeakestCreature();
        var closest = Checks.ClosestCreature(new string[] { "Predator" });
        float weakDist = ( weakling!=null ? Vector2.Distance(this.transform.position, weakling.transform.position) : 10);
        float closeDist = ( closest!=null ? Vector2.Distance(this.transform.position, closest.transform.position) : 10);

        if ((closeDist <= 5 && this.health > maxHealth/2) || closeDist <= 2)
            proto = Protocols.Melee;
        else
            proto = Protocols.Ranged;

        return proto;
    }
}
