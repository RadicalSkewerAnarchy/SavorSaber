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
            if (Checks.NumberOfFriends() > 2)
                p = Protocols.Wander;
            else if (Checks.NumberOfFriends() < 4)
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
                fim.StartCoroutine(fim.SugarRush(8));
                break;
            default:
                break;
        }
    }
}
