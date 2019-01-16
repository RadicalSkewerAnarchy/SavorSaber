using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeDatabase : MonoBehaviour
{

    /// <summary>
    /// Array of recipe data that make up the "cook book" 
    /// Right now, this means this has to be attached as a script to a GameObject so that
    /// designers can drag the scriptable objects they create into the array. Not sure if 
    /// there's a better way to do this.
    /// </summary>
    public RecipeData[] recipes;

    /// <summary>
    /// Would be called when the player cooks their food, probably in the inventory 
    /// manager script.
    /// This will take in a stack (inventory skewer) and compare the flavors on it to the
    /// recipe book. If it finds a match, it returns that RecipeData.
    /// 
    /// </summary>
    public RecipeData CompareToBasicRecipes( /* Stack<IngredientData> currentSkewer */)
    {
        //for each element of recipes array
            // for each flavor in that recipe
                // is that flavor present in currentSkewer? If yes, continue to see if 
                // it's a full match. If not, check the next recipe
            // if you get to the end of a recipe with no flavors missing on the skewer,
            // it's a match, so return this recipe 

        return new RecipeData();
    }

}
