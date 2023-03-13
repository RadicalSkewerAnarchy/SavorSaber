using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LycheeData : AIData
{
    private WaitForSeconds secondTic;
    public int maxTics = 3;
    private int numTics;
    private bool stopHealing = false;

    // Start is called before the first frame update
    void Start()
    {
        secondTic = new WaitForSeconds(1);
    }

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
                stopHealing = false;
                this.GetComponent<SpriteRenderer>().color = Color.magenta;
                StopCoroutine(StartHealTics());
                StartCoroutine(StartHealTics());
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
                Debug.Log("Lychee no longer overcharged");
                stopHealing = true;
                this.GetComponent<SpriteRenderer>().color = Color.white;
                StopAllCoroutines();
                
                break;
            default:
                break;
        }
    }

    private IEnumerator StartHealTics()
    {
        if (stopHealing) yield return null;
        if (numTics <= maxTics)
        {
            yield return secondTic;
            HealTargets();
        }
        yield return null;
    }

    private void HealTargets()
    {
        if (stopHealing) return;
        PlayerData pd = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        if (pd.health < pd.maxHealth)
        {
            pd.DoHeal(1);
        }
        StartCoroutine(StartHealTics());
        numTics++;
    }
}
