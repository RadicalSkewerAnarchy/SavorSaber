using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trigger a cutscene on being fed something.
/// </summary>
public class EventTriggerFlavorInput : FlavorInputManager
{
    public EventTrigger scene;
    public override void Feed(RecipeData.Flavors flavor, int magnitude)
    {
        //flavorCountDictionary[flavor] += magnitude;
        if (magnitude > 0 && !scene.IsActive)
        {
            scene.Trigger();
        }        
    }
}
