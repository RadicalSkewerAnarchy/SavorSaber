using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Monster))]
[RequireComponent(typeof(CharacterData))]
public class GhostPepperFlavorInput : FlavorInputManager
{
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
            characterData.health += flavorCountDictionary[RecipeData.Flavors.Spicy];
            flavorCountDictionary[RecipeData.Flavors.Spicy] = 0;
        }
        CheckCharmEffect();
    }
}
