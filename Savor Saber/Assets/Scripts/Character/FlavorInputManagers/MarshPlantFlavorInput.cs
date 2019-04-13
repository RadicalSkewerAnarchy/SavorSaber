using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarshPlantFlavorInput : PlantFlavorInput
{
    BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        InitializeDictionary();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterData = GetComponent<CharacterData>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponentInChildren<BoxCollider2D>();
        //recieverCollider = GetComponent<CircleCollider2D>();
    }

    public override void RespondToIngredients()
    {
        //handle spicy
        if (flavorCountDictionary[RecipeData.Flavors.Spicy] > 0)
        {
            isFed = true;
        }
    }

    private void Update()
    {
        ClosePlant();
        OpenPlant();
    }

    // prevent player
    public override void ClosePlant()
    {
        if (!isFed)
        {
            boxCollider.enabled = true;
        }

    }

    // allow player through
    public override void OpenPlant()
    {
        if (isFed)
        {
            boxCollider.enabled = false;
            spriteRenderer.color = new Color(0.6f, 0.6f, 0.6f, 0.5f);
        }

    }
}