using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlavorInputManager : MonoBehaviour
{
    /// <summary>
    /// How much of each flavor this entity has been fed
    /// </summary>
    public Dictionary<RecipeData.Flavors, int> flavorCountDictionary = new Dictionary<RecipeData.Flavors, int>();


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

    public virtual void Feed(RecipeData.Flavors flavor, int magnitude)
    {
        flavorCountDictionary[flavor] += magnitude;
    }
}
