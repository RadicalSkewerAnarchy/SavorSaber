using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarshPlantFlavorInput : PlantFlavorInput
{
    BoxCollider2D boxCollider;
    CircleCollider2D recieverCollider;
    // Start is called before the first frame update
    void Start()
    {
        InitializeDictionary();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterData = GetComponent<CharacterData>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponentInChildren<BoxCollider2D>();
        recieverCollider = GetComponent<CircleCollider2D>();
    }

    public override void RespondToIngredients()
    {
        //handle spicy
        if (flavorCountDictionary[RecipeData.Flavors.Spicy] >= 0)
        {
            isFed = true;
            OpenPlant();
        }
    }

    private void Update()
    {
    }

    public override void ClosePlant()
    {
        boxCollider.enabled = true;
        spriteRenderer.color = Color.red;
    }
    public override void OpenPlant()
    {
        boxCollider.enabled = false;
        spriteRenderer.color = Color.white;
    }
}