using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarshPlantFlavorInput : PlantFlavorInput
{
    BoxCollider2D boxCollider;
    public Sprite openSprite;
    public Sprite closedSprite;

    // Start is called before the first frame update
    void Start()
    {
        InitializeDictionary();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterData = GetComponent<AIData>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponentInChildren<BoxCollider2D>();
        //recieverCollider = GetComponent<CircleCollider2D>();
    }

    public override void RespondToIngredients(bool fedByPlayer)
    {
        //handle spicy
        if (flavorCountDictionary[RecipeData.Flavors.Spicy] > 0)
        {
            isFed = true;
            OpenPlant();
        }
    }

    private void Update()
    {
        //ClosePlant();
        //OpenPlant();
    }

    // prevent player
    public override void ClosePlant()
    {
        spriteRenderer.sprite = closedSprite;
        if (!isFed)
        {
            boxCollider.enabled = true;
        }
    }

    // allow player through
    public override void OpenPlant()
    {
        spriteRenderer.sprite = openSprite;
        if (isFed)
        {
            boxCollider.enabled = false;
            spriteRenderer.color = new Color(0.6f, 0.6f, 0.6f, 0.5f);
        }
    }
}