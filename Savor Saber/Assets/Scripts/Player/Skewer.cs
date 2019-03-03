using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for packaging a stack of ingredients and a completed recipe.
/// </summary>
public class Skewer
{
    //fields
    public Stack<IngredientData> ingredientStack = new Stack<IngredientData>();

    public RecipeData finishedRecipe = null;

    /// <summary>
    /// how much of each flavor is present on the skewer
    /// </summary>
    public Dictionary<RecipeData.Flavors, int> flavorCountDictionary = new Dictionary<RecipeData.Flavors, int>();
    public Dictionary<string, int> ingredientCountDictionary = new Dictionary<string, int>();


    //methods

    public void InitializeDictionary()
    {
        flavorCountDictionary.Add(RecipeData.Flavors.Sweet, 0);
        flavorCountDictionary.Add(RecipeData.Flavors.Sour, 0);
        flavorCountDictionary.Add(RecipeData.Flavors.Spicy, 0);
        flavorCountDictionary.Add(RecipeData.Flavors.Salty, 0);
        flavorCountDictionary.Add(RecipeData.Flavors.Savory, 0);
        flavorCountDictionary.Add(RecipeData.Flavors.Bitter, 0);
        flavorCountDictionary.Add(RecipeData.Flavors.Acquired, 0);
    }

    /// <summary>
    /// Resets the count of each flavor to 0
    /// </summary>
    public void ResetDictionary()
    {
        flavorCountDictionary[RecipeData.Flavors.Sweet] = 0;
        flavorCountDictionary[RecipeData.Flavors.Sour] = 0;
        flavorCountDictionary[RecipeData.Flavors.Spicy] = 0;
        flavorCountDictionary[RecipeData.Flavors.Salty] = 0;
        flavorCountDictionary[RecipeData.Flavors.Savory] = 0;
        flavorCountDictionary[RecipeData.Flavors.Bitter] = 0;
        flavorCountDictionary[RecipeData.Flavors.Acquired] = 0;
    }

    public int GetCount()
    {
        return ingredientStack.Count;
    }

    public void PushIngredient(IngredientData ingredient)
    {
        ingredientStack.Push(ingredient);

        // Sweet = 1, Acquired = 64
        for (int f = 1; f <= 64; f = f << 1)
        {

            if ((f & (int)ingredient.flavors) > 0)
            {
                RecipeData.Flavors foundFlavor = (RecipeData.Flavors)f;
                flavorCountDictionary[foundFlavor] = flavorCountDictionary[foundFlavor] + 1;
                
            }
        }
        if (!ingredientCountDictionary.ContainsKey(ingredient.displayName))
        {
            ingredientCountDictionary.Add(ingredient.displayName, 1);
        }
        else
        {
            ingredientCountDictionary[ingredient.displayName] = ingredientCountDictionary[ingredient.displayName] + 1;
        }
    }

    public IngredientData PopIngredient()
    {
        IngredientData poppedIngredient = ingredientStack.Pop();
        ingredientCountDictionary[poppedIngredient.displayName] = ingredientCountDictionary[poppedIngredient.displayName] - 1;

        for (int f = 1; f <= 64; f = f << 1)
        {

            if ((f & (int)poppedIngredient.flavors) > 0)
            {
                RecipeData.Flavors foundFlavor = (RecipeData.Flavors)f;
                flavorCountDictionary[foundFlavor] = flavorCountDictionary[foundFlavor] - 1;

            }
        }

        return poppedIngredient;
    }

    public IngredientData[] ToArray()
    {
        return ingredientStack.ToArray();
    }

    public bool IsCooked()
    {
        return finishedRecipe != null;
    }

    public void ClearItems()
    {
        ingredientStack.Clear();
    }
    public void ClearRecipe()
    {
        finishedRecipe = null;
    }
}
