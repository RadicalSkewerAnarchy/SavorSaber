using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarshPlantFlavorInput : PlantFlavorInput
{
    BoxCollider2D boxCollider;

    public Sprite closedSprite;
    public Sprite openSprite;

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
            OpenPlant();
            isFed = true;
            spriteRenderer.color = new Color(0.6f, 0.6f, 0.6f, 0.5f);
        }
    }

    private void Update()
    {
    }

    public override void ClosePlant()
    {
        if (!isFed)
        {
            boxCollider.enabled = true;
            //spriteRenderer.color = Color.red;
            spriteRenderer.sprite = closedSprite;
        }

    }
    public override void OpenPlant()
    {
        if (!isFed)
        {
            boxCollider.enabled = false;
            //spriteRenderer.color = Color.white;
            spriteRenderer.sprite = openSprite;
        }

    }
}