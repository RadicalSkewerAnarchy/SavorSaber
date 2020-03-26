using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlantFlavorInput : FlavorInputManager
{
    [System.NonSerialized]
    public bool isFed = false;

    [System.NonSerialized]
    public bool nearPlayer = false;

    [System.NonSerialized]
    public bool active = false;
    
    public bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        InitializeDictionary();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Feed(IngredientData ingredient, bool fedByPlayer, CharacterData feeder)
    {
        if((ingredient.flavors & RecipeData.Flavors.Spicy) > 0)
        {
            isFed = true;
            OpenPlant();
        }

    }

    public virtual void ClosePlant() { }
    public virtual void OpenPlant() { }
}
