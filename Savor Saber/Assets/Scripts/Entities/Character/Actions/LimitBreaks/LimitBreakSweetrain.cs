using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitBreakSweetrain : PoweredObject
{

    GameObject[] healTargets;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetTargets()
    {
        CharacterData data;
        healTargets = GameObject.FindGameObjectsWithTag("Prey");
        foreach(GameObject target in healTargets)
        {
            data = target.GetComponent<CharacterData>();
            if(data != null)
            {
                data.DoHeal(99);
            }
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerData>().DoHeal(99);
    }
}
