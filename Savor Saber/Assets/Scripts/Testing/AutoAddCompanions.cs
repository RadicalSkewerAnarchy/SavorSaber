using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAddCompanions : MonoBehaviour
{
    public PlayerCompanionSummon summoner;

    public IngredientData[] dataToAdd;
    // Start is called before the first frame update
    void Start()
    {
        foreach(IngredientData data in dataToAdd)
        {
            summoner.UnlockFruitant(data);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
