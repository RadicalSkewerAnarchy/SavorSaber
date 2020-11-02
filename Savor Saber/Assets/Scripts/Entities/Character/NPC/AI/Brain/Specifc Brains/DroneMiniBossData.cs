using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMiniBossData : AIData
{
    public override Protocols DecideProtocol()
    {
        Protocols proto;

        meleeHunter = true;
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
