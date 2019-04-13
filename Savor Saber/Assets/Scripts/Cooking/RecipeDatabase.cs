using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RecipeDatabase : MonoBehaviour
{

    /// <summary>
    /// Array of recipe data that make up the "cook book" 
    /// Right now, this means this has to be attached as a script to a GameObject so that
    /// designers can drag the scriptable objects they create into the array. Not sure if 
    /// there's a better way to do this.
    /// </summary>
    public RecipeData[] recipes;

    // ingredient data
    public Dictionary<string, IngredientData> allIngredients;
    public string[] ingredientNames;
    public IngredientData[] ingredientDatas;

    private void Start()
    {
        allIngredients = new Dictionary<string, IngredientData>();
        for ( var i = 0; i < ingredientNames.Length; i++)
        {
            string s = ingredientNames[i];
            IngredientData d = ingredientDatas[i];
            allIngredients.Add(s, d);
        }
    }

    /// <summary>
    /// Would be called when the player cooks their food, probably in the inventory 
    /// manager script.
    /// This will take in a stack (inventory skewer) and compare the flavors on it to the
    /// recipe book. If it finds a match, it returns that RecipeData.
    /// 
    /// </summary>
    public RecipeData CompareToRecipes(Stack<IngredientData> currentSkewer)
    {
        //create a backup copy of the array to be restored, as elements are removed to prevent
        //double-checking.
        IngredientData[] ingredientArray = currentSkewer.ToArray();
        IngredientData[] ingredientArrayCopy = new IngredientData[ingredientArray.Length];
        Array.Copy(ingredientArray, ingredientArrayCopy, ingredientArray.Length);
        
        bool recipeLookupFailed = false;

        //iterate over all recipes
        for(int i = 0; i < recipes.Length; i++)
        {
            RecipeData currentRecipe = recipes[i];
            recipeLookupFailed = false;
            Array.Copy(ingredientArrayCopy, ingredientArray, ingredientArray.Length);

            //Debug.Log("Checking recipe: " + currentRecipe.displayName);

            //iterate over each flavor of the current recipe 
            for(int currentFlavor = 0; currentFlavor < currentRecipe.flavors.Length; currentFlavor++)
            {

                //If we failed to find the last ingredient, abandon this recipe and try the next
                if (recipeLookupFailed)
                {
                    //Debug.Log("Ingredient missing, recipe failed. Try next.");
                    break;
                }

                //Debug.Log("Checking for presence of flavor " + currentRecipe.flavors[currentFlavor]);

                //iterate over each ingredient on the active skewer
                for (int currentIngredient = 0; currentIngredient < ingredientArray.Length; currentIngredient++)
                {
                    //Debug.Log("Scanning ingredient " + currentIngredient + " of active skewer");
                    //if the flavor of the current ingredient matches the recipe flavor currently being tested...
                    if((ingredientArray[currentIngredient] == null ? 0 : ingredientArray[currentIngredient].flavors & currentRecipe.flavors[currentFlavor]) > 0)
                    {
                        //this flavor is on the skewer, so continue but remove it from the skewer so it isn't counted twice
                        ingredientArray[currentIngredient] = null;
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

    public RecipeData CompareToSimpleRecipes(Stack<IngredientData> currentSkewer)
    {
        //create a backup copy of the array to be restored, as elements are removed to prevent
        //double-checking.
        IngredientData[] ingredientArray = currentSkewer.ToArray();
        IngredientData[] ingredientArrayCopy = new IngredientData[ingredientArray.Length];
        Array.Copy(ingredientArray, ingredientArrayCopy, ingredientArray.Length);

        bool recipeLookupFailed = false;

        //iterate over all recipes
        for (int i = 0; i < recipes.Length; i++)
        {
            RecipeData currentRecipe = recipes[i];
            recipeLookupFailed = false;
            Array.Copy(ingredientArrayCopy, ingredientArray, ingredientArray.Length);

            //Debug.Log("Checking recipe: " + currentRecipe.displayName);

            if (currentRecipe.complexRecipe)
            {
                //Debug.Log("Can only check simple recipes on a short cook, skipping...");
                continue;
            }

            //iterate over each flavor of the current recipe 
            for (int currentFlavor = 0; currentFlavor < currentRecipe.flavors.Length; currentFlavor++)
            {

                //If we failed to find the last ingredient, abandon this recipe and try the next
                if (recipeLookupFailed)
                {
                    //Debug.Log("Ingredient missing, recipe failed. Try next.");
                    break;
                }

                //Debug.Log("Checking for presence of flavor " + currentRecipe.flavors[currentFlavor]);

                //iterate over each ingredient on the active skewer
                for (int currentIngredient = 0; currentIngredient < ingredientArray.Length; currentIngredient++)
                {
                    //Debug.Log("Scanning ingredient " + currentIngredient + " of active skewer");
                    //if the flavor of the current ingredient matches the recipe flavor currently being tested...
                    if ((ingredientArray[currentIngredient] == null ? 0 : ingredientArray[currentIngredient].flavors & currentRecipe.flavors[currentFlavor]) > 0)
                    {
                        //this flavor is on the skewer, so continue but remove it from the skewer so it isn't counted twice
                        ingredientArray[currentIngredient] = null;
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
