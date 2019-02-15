using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trigger a cutscene on being fed something.
/// </summary>
public class CutsceneTriggerFlavorInput : FlavorInputManager
{
    public Cutscene scene;
    public override void Feed(RecipeData.Flavors flavor, int magnitude)
    {
        //flavorCountDictionary[flavor] += magnitude;
        if (magnitude > 0)
            scene.Activate();
    }
}
