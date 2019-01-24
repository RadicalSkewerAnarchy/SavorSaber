using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A scriptable object class designed to hold the data for an ingredient
/// </summary>
[CreateAssetMenu(fileName = "newIngredient", menuName = "IngredientData")]
public class IngredientData : ScriptableObject
{

    /// <summary>The name of the ingredient in game</summary>
    public string displayName;
    /// <summary>The image of the ingredient in the inventory</summary>
    public Sprite image;
    /// <summary>The categories of ingredient this ingredient fits into (flags)</summary>
    public RecipeData.Flavors flavors;
}
