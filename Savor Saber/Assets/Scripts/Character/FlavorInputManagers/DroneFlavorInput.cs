using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CharacterData))]
public class DroneFlavorInput : FlavorInputManager
{
    private int slowDuration = 6;
    private bool slowed;
    private int slowTimer = 0;
    private WaitForSeconds OneSecondTic = new WaitForSeconds(1);

    // Start is called before the first frame update
    void Start()
    {
        InitializeDictionary();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterData = GetComponent<CharacterData>();
    }

    public override void RespondToIngredients()
    {
        //handle spicy - Does damage over time
        if (flavorCountDictionary[RecipeData.Flavors.Spicy] >= 0)
        {
            spriteRenderer.color = Color.red;
            DamageOverTime(2 * flavorCountDictionary[RecipeData.Flavors.Spicy], dotTicLength);

            flavorCountDictionary[RecipeData.Flavors.Spicy] = 0;
        }

        //handle sweet - Pacifies drone
        if (flavorCountDictionary[RecipeData.Flavors.Sweet] >= 0)
        {
            flavorCountDictionary[RecipeData.Flavors.Sweet] = 0;
        }

        //handle sour - Does more damage to high-health targets
        if(flavorCountDictionary[RecipeData.Flavors.Sour] >= 0)
        {
            float health = characterData.health;
            int damage = 1 + 1 * (int)(characterData.health / 4);
            characterData.DoDamage(damage);
            flavorCountDictionary[RecipeData.Flavors.Sour] = 0;
        }

        //handle salty - Reduce Speed
        if (flavorCountDictionary[RecipeData.Flavors.Salty] >= 0)
        {
            slowed = true;
            if (!slowed)
                StartCoroutine(SlowDown());
            else
                slowTimer += slowDuration;
            flavorCountDictionary[RecipeData.Flavors.Salty] = 0;
        }

    }

    private IEnumerator SlowDown()
    {
        float baseSpeed = characterData.Speed;
        characterData.Speed = baseSpeed / (1 + flavorCountDictionary[RecipeData.Flavors.Salty]);

        slowTimer = slowDuration;
        while(slowTimer-- > 0)
        {
            yield return OneSecondTic;
        }

        characterData.Speed = baseSpeed;
    }
}
