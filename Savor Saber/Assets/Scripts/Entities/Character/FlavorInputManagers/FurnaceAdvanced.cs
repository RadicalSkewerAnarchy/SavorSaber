using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceAdvanced : Furnace
{
    public PoweredObject[] SourTargets;
    public PoweredObject[] SweetTargets;
    public PoweredObject[] SpicyTargets;
    public PoweredObject[] SaltyTargets;
    public PoweredObject[] BitterTargets;
    // Start is called before the first frame update
    void Start()
    {
        //burnSFXPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Feed(IngredientData ingredient, bool fedByPlayer, CharacterData feeder)
    {
        if (!active) return;

        switch (ingredient.flavors)
        {
            case RecipeData.Flavors.Sweet:
                Debug.Log("Furnace: Sweet");
                PowerTargetObjects(SweetTargets);
                break;
            case RecipeData.Flavors.Sour:
                Debug.Log("Furnace: Sour");
                PowerTargetObjects(SourTargets);
                break;
            case RecipeData.Flavors.Spicy:
                Debug.Log("Furnace: Spicy");
                PowerTargetObjects(SpicyTargets);
                break;
            case RecipeData.Flavors.Salty:
                Debug.Log("Furnace: Salty");
                PowerTargetObjects(SaltyTargets);
                break;
            case RecipeData.Flavors.Bitter:
                Debug.Log("Furnace: Bitter");
                PowerTargetObjects(BitterTargets);
                break;
        }
    }

    private void PowerTargetObjects(PoweredObject[] flavorArray)
    {
        if (targetsActive)
        {
            foreach (PoweredObject target in flavorArray)
            {
                target.ShutOff();
            }

            if (isToggle)
                targetsActive = false;
        }
        else
        {
            foreach (PoweredObject target in flavorArray)
            {
                target.TurnOn();
            }

            if (isToggle)
                targetsActive = true;
        }
        //burnSFXPlayer.Play();
    }
}
