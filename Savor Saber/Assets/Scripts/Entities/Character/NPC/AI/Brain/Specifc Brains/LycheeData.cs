using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LycheeData : AIData
{
    public override Protocols DecideProtocol()
    {
        Protocols p = currentProtocol;
       
        if(Checks.NumberOfEnemies() > 0)
        {
            p = Protocols.Attack;
        }
        else
        {
            if (Checks.NumberOfFriends() > 5)
                p = Protocols.Wander;
            else if (Checks.NumberOfFriends() > 2)
                p = Protocols.Party;
            else
                p = Protocols.Chase;
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
