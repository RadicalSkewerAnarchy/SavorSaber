using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddlePearData : AIData
{
    public override Protocols DecideProtocol()
    {
        Protocols p = currentProtocol;
       
        if(Checks.NumberOfEnemies() > 0)
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

    public override void OnStateEnter(LifeState s)
    {
        switch(s)
        {
            case LifeState.overcharged:
                this.GetComponent<SpriteRenderer>().color = Color.magenta;
                FlavorInputManager fim = GetComponent<FlavorInputManager>();
                fim.SugarStack(Mathf.Max(this.health - this.maxHealth, 0));
                break;
            default:
                break;
        }
    }
}
