using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A scriptable object class designed to hold the data for an ingredient
/// </summary>
[CreateAssetMenu(fileName = "newIngredient", menuName = "IngredientData")]
public class IngredientData : ScriptableObject
{
    [System.Flags]
    public enum Types
    {
        None   = 0,
        Grain  = 2,
        Fruit  = 4,
        Veggie = 8,
        Meat   = 16,
        Dairy  = 32,
        Candy  = 64,
    }
    /// <summary>The name of the ingredient in game</summary>
    public string displayName;
    /// <summary>The image of the ingredient in the inventory</summary>
    public Texture2D image;
    /// <summary>The categories of ingredient this ingredient fits into (flags)</summary>
    public Types types;
    /// <summary>The amount the ingredient heals</summary>
    public int healValue;
}
