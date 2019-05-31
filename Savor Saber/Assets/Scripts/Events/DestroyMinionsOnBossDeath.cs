using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMinionsOnBossDeath : MonoBehaviour
{
    public List<GameObject> minions;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyMinions()
    {
        CharacterData minionData;
        foreach(GameObject minion in minions)
        {
            if(minion != null)
            {
                minionData = minion.GetComponent<CharacterData>();
                minionData.DoDamage(9999);
            }

        }
    }
}
