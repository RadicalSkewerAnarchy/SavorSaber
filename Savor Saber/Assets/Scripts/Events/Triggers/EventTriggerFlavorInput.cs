﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Trigger a cutscene on being fed something.
/// </summary>
public class EventTriggerFlavorInput : FlavorInputManager
{
    public EventTrigger scene;
    public void Feed(IngredientData[] ingredientArray)
    {
        Debug.Log("Raindeer feed");
        if (ingredientArray.Length >= 0 && !scene.IsActive)
        {
            foreach(var ing in ingredientArray)
            {
                if(favoriteIngredients.Any((i) => i == ing.displayName))
                {
                    scene.Trigger();
                    return;
                }
            }
        }        
    }
}
