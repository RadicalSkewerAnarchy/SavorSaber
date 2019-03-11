using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlantFlavorInput : FlavorInputManager
{
    [System.NonSerialized]
    public bool isFed = false;

    [System.NonSerialized]
    public bool nearPlayer = false;

    [System.NonSerialized]
    public bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        InitializeDictionary();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterData = GetComponent<CharacterData>();
    }

    public override void RespondToIngredients()
    {
        //handle spicy
        if (flavorCountDictionary[RecipeData.Flavors.Spicy] >= 0)
        {
            isFed = true;
        }

    }

    public virtual void ClosePlant() { }
    public virtual void OpenPlant() { }
}
