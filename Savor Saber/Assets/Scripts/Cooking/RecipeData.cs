using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A scriptable object class designed to hold the data for an ingredient
/// </summary>

[CreateAssetMenu(fileName = "newRecipe", menuName = "RecipeData")]
public class RecipeData : ScriptableObject
{

    [System.Flags]
    public enum Flavors
    {
        None = 0,
        Sweet = 1,
        Spicy = 2,
        Bitter = 4,
        Sour = 8,
        Salty = 16,
        Savory = 32,
        Acquired = 64
    }

    /// <summary>The name of the recipe in game</summary>
    public string displayName;

    /// <summary>
    /// Whether or not the recipe is complex (does it require a campfire?)
    /// Complex recipes require more than one flavor
    /// </summary>
    public bool complexRecipe;

    /// <summary>List up to three flavor components</summary>
    
    /// this is stored as an array now rather than a stack like the inventory. This is
    /// partially because scriptable objects can't serialize stacks, but also because 
    /// skewers store ingredients, not flavors, and order may not matter for recipes? 
    public Flavors[] flavors = new Flavors[3];

}
