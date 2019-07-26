using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBossData : AIData
{
    public override Protocols DecideProtocol()
    {
        Protocols proto = currentProtocol;

        if (health > maxHealth/2)
        {
            if (Checks.NumberOfFriends() >= Checks.NumberOfEnemies())
                proto = Protocols.Melee;
            else
                proto = Protocols.Ranged;

            //temp
            proto = Protocols.Ranged;
        }
        else if (health > maxHealth/4)
        {
            if (Checks.NumberOfFriends() > Checks.NumberOfEnemies())
                proto = Protocols.Ranged;
            else
                proto = Protocols.Melee;

            // temp
            proto = Protocols.Ranged;
        }
        else
        {
            GameObject c = Checks.ClosestCreature();
            if (Vector2.Distance(c.transform.position, this.transform.position) < 5)
                proto = Protocols.Melee;
            else
                proto = Protocols.Ranged;
        }

        return proto;
    }
}
