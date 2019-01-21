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
    public RecipeData CompareToRecipes(Stack<IngredientData> currentSkewer)
    {
        //for each element of recipes array
            // for each flavor in that recipe
                // is that flavor present in currentSkewer? If yes, continue to see if 
                // it's a full match. If not, check the next recipe
                // if you get to the end of a recipe with no flavors missing on the skewer,
                // it's a match, so return this recipe 
        // else return null

        IngredientData[] ingredientArray = currentSkewer.ToArray();
        IngredientData[] ingredientArrayCopy = ingredientArray;
        int numberOfMatches = 0;
        bool recipeLookupFailed = false;

        //iterate over all recipes
        for(int i = 0; i < recipes.Length; i++)
        {
            RecipeData currentRecipe = recipes[i];
            numberOfMatches = 0;
            recipeLookupFailed = false;
            ingredientArray = ingredientArrayCopy;

            //iterate over each flavor of the current recipe 
            for(int f = 0; f < currentRecipe.flavors.Length; f++)
            {
                //If we failed to find the last ingredient, abandon this recipe and try the next
                if (recipeLookupFailed)
                {
                    break;
                }
                //iterate over each ingredient on the active skewer
                for(int s = 0; s < ingredientArray.Length; s++)
                {
                    //if the flavor of the current ingredient matches the recipe flavor currently being tested...
                    if((ingredientArray[s].flavors & currentRecipe.flavors[f]) > 0)
                    {
                        //this flavor is on the skewer, so continue but remove it from the skewer so it isn't counted twice
                        ingredientArray[s] = null;
                        numberOfMatches++;
                        recipeLookupFailed = false;

                        //break the loop to check the next flavor on the recipe
                        break;
                    }
                    recipeLookupFailed = true;
                }
                
            }
            //if you got here without failing, the recipe is a match, so return it
            if(!recipeLookupFailed) return currentRecipe;
        }

        //if you get through all the recipes without a match, return null
        return null;
    }

}
