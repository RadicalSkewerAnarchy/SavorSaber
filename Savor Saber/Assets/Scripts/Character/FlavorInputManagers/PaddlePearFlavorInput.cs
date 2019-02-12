using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PaddlePearFlavorInput : FlavorInputManager
{
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        InitializeDictionary();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public override void Feed(RecipeData.Flavors flavor, int magnitude)
    {
        Debug.Log("Paddlepear ate " + magnitude + " " + flavor);
        flavorCountDictionary[flavor] += magnitude;

        //handle sweet
        if(flavorCountDictionary[RecipeData.Flavors.Sweet] >= 4)
        {
            Debug.Log("Friendliness up!");
            spriteRenderer.color = new Color(1f, 0.2f, 0.8f);

            //when a status effect is applied, reset the dictionary to avoid massive buildup
            ResetDictionary();
        }
    }
}
