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

    // Not currently being used
    /*
    [System.Flags]
    public enum Effects
    {
        None = 0,
        Love = 1, 
        Fire = 2, 
        Freeze = 4, 
        Electric = 8,
        Earth = 16,
        Explosive = 32
    }
    */

    /// <summary>
    /// The name of the recipe in game
    /// </summary>
    public string displayName;

    /// <summary>
    /// Whether or not the recipe is complex (does it require a campfire?)
    /// Complex recipes require more than one flavor
    /// </summary>
    public bool complexRecipe;

    /// <summary>
    /// List up to three flavor components
    /// </summary>
    public Flavors[] flavors = new Flavors[3];

    /// <summary>
    /// Radius of circle in which to apply effects
    /// </summary>
    [Range(0f, 20f)]
    public float areaOfEffectRadius = 0f;

    /// <summary>
    /// Duration in seconds of mind control effect
    /// </summary>
    [Range(0f,360f)]
    public float mindControlDuration = 0f;

    /// <summary>
    /// Duration in seconds of burn effect
    /// </summary>
    [Range(0f, 360f)]
    public float burnDuration = 0f;

    /// <summary>
    /// Duration in seconds of freeze effect
    /// </summary>
    [Range(0f, 360f)]
    public float freezeDuration = 0f;

    /// <summary>
    /// Duration in seconds of shock effect
    /// </summary>
    [Range(0f, 360f)]
    public float shockDuration = 0f;

    /// <summary>
    /// Duration in seconds of earth effect
    /// </summary>
    [Range(0f, 360f)]
    public float earthDuration = 0f;

    /// <summary>
    /// Strength of knockback effect
    /// </summary>
    [Range(0f, 10f)]
    public float knockbackForce = 0f;

    /// <summary>
    /// Code for what happens when player eats this cooked recipe
    /// </summary>
    public void ApplyEffectToSelf()
    {
        
    }
    
    /// <summary>
    /// Code for what happens when this recipe is thrown at a target
    /// </summary>
    public void ApplyEffectToTarget(GameObject target)
    {
        // take in the target that was hit as an argument
        // if an effect's value is >0, call that monster's corresponding function
        // which can be nothing (e.g., trying to burn a monster that's immune to
        // fire would do nothing.
    }
}
