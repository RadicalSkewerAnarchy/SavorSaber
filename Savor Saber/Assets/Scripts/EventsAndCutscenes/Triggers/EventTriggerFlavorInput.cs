using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Trigger a cutscene on being fed something.
/// </summary>
public class EventTriggerFlavorInput : FlavorInputManager
{
    public EventTrigger scene;
    public override void Feed(IngredientData[] ingredientArray, bool fedbyPlayer)
    {
        if (ingredientArray.Length >= 0 && !scene.IsActive)
        {
            foreach(var ing in ingredientArray)
            {
                if(favoriteIngredients.Any((i) => i == ing))
                {
                    scene.Trigger();
                    return;
                }
            }
        }        
    }

    public override void Feed(IngredientData ingredient, bool fedByPlayer, CharacterData feederData)
    {

        foreach(IngredientData favorite in favoriteIngredients)
        {
            if (ingredient == favorite && !scene.IsActive)
            {
                scene.Trigger();
                return;
            }
        }

    }
}
