using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Monster))]
[RequireComponent(typeof(CharacterData))]
public class PaddlePearFlavorInput : FlavorInputManager
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
            spriteRenderer.color = Color.red;
            DamageOverTime(2 * flavorCountDictionary[RecipeData.Flavors.Spicy], dotTicLength);
            //Only reset the spicy stat or else no other stats will work
            flavorCountDictionary[RecipeData.Flavors.Spicy] = 0;
        }
        if (flavorCountDictionary[RecipeData.Flavors.Sweet] > 0)
        {
            characterData.moods["Friendliness"] = 0.125f * flavorCountDictionary[RecipeData.Flavors.Sweet];
        }
    }
}
