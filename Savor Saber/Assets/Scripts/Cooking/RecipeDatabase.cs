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

            Debug.Log("Checking recipe: " + currentRecipe.displayName);

            //iterate over each flavor of the current recipe 
            for(int currentFlavor = 0; currentFlavor < currentRecipe.flavors.Length; currentFlavor++)
            {

                //If we failed to find the last ingredient, abandon this recipe and try the next
                if (recipeLookupFailed)
                {
                    Debug.Log("Ingredient missing, recipe failed. Try next.");
                    break;
                }

                Debug.Log("Checking for presence of flavor " + currentRecipe.flavors[currentFlavor]);

                //iterate over each ingredient on the active skewer
                for (int currentIngredient = 0; currentIngredient < ingredientArray.Length; currentIngredient++)
                {
                    Debug.Log("Scanning ingredient " + currentIngredient + " of active skewer");
                    //if the flavor of the current ingredient matches the recipe flavor currently being tested...
                    if((ingredientArray[currentIngredient] == null ? 0 : ingredientArray[currentIngredient].flavors & currentRecipe.flavors[currentFlavor]) > 0)
                    {
                        //this flavor is on the skewer, so continue but remove it from the skewer so it isn't counted twice
                        ingredientArray[currentIngredient] = null;
                        numberOfMatches++;
                        recipeLookupFailed = false;

                        //break the loop to check the next flavor on the recipe
                        break;
                    }
                    recipeLookupFailed = true;
                }
                
            }
            //if you got here without failing, the recipe is a match, so return it
            if (!recipeLookupFailed)
            {
                Debug.Log("found recipe match: " + currentRecipe.displayName);
                return currentRecipe;
            }
        }

        //if you get through all the recipes without a match, return null
        Debug.Log("No matches found");
        return null;
    }

}
