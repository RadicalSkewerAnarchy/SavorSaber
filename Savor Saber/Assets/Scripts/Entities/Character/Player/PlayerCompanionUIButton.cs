using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompanionUIButton : MonoBehaviour
{
    /// <summary>
    /// The ingredient data containing the fruitant morph to be spawned by this button
    /// </summary>
    public IngredientData fruitantData;
    public PlayerCompanionSummon summoner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SummonCompanion()
    {
        summoner.SummonCompanion(fruitantData.displayName);
        summoner.CloseUI();
    }
}
