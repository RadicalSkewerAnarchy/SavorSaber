using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitBreakSweetrain : PoweredObject
{

    GameObject[] healTargets;
    private WaitForSeconds secondTic;
    public int maxTics = 10;
    private int numTics = 0;
    // Start is called before the first frame update
    void Start()
    {
        secondTic = new WaitForSeconds(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TurnOn()
    {
        numTics = 0;
        base.TurnOn();
        StartCoroutine(StartHealTics());
    }

    private IEnumerator StartHealTics()
    {
        if(numTics <= maxTics)
        {
            yield return secondTic;
            HealTargets();       
        }
        yield return null;
    }

    private void HealTargets()
    {
        CharacterData data;
        healTargets = GameObject.FindGameObjectsWithTag("Prey");
        foreach(GameObject target in healTargets)
        {
            data = target.GetComponent<CharacterData>();
            if(data != null)
            {
                data.DoHeal(1);
            }
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerData pd = player.GetComponent<PlayerData>();
        if (pd.health < pd.maxHealth)
        {
            pd.DoHeal(1);
        }
        StartCoroutine(StartHealTics());
        numTics++;
    }
}
