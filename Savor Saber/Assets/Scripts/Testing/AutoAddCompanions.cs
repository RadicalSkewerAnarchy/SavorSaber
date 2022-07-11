using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAddCompanions : MonoBehaviour
{
    public PlayerCompanionSummon summoner;

    public IngredientData[] dataToAdd;
    public bool autoEnableSummoning = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach(IngredientData data in dataToAdd)
        {
            summoner.UnlockFruitant(data);
        }
        if (autoEnableSummoning)
        {
            FlagManager.SetFlag("CanSummonCompanion", "True");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
