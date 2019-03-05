using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trigger a cutscene on being fed something.
/// </summary>
public class EventTriggerFlavorInput : FlavorInputManager
{
    public EventTrigger scene;
    public override void Feed(IngredientData[] ingredientArray)
    {
        Debug.Log("Raindeer feed");
        //flavorCountDictionary[flavor] += magnitude;
        if (ingredientArray.Length >= 0 && !scene.IsActive)
        {
            scene.Trigger();
        }        
    }
}
