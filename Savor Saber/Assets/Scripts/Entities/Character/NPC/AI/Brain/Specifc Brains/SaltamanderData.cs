using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltamanderData : AIData
{
    public GameObject shieldToEnable; 

    public override Protocols DecideProtocol()
    {
        Protocols p = currentProtocol;
        
        if(Checks.NumberOfEnemies() > 0)
        {
            p = Protocols.Attack;
            //Debug.Log("Paddle Pear found enemies, attack");
        }
        else
        {
            //Debug.Log("Paddle pear checking number of friends since there are no enemies nearby");
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
                
                shieldToEnable.SetActive(true);
                break;
            default:
                break;
        }
    }

    public override void OnStateExit(LifeState s)
    {
        base.OnStateExit(s);

        switch (s)
        {
            case LifeState.overcharged:
                shieldToEnable.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void SetSpeed(float s)
    {
        Speed = s;
    }
}
