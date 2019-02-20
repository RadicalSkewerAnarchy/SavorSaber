﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Monster))]
[RequireComponent(typeof(CharacterData))]
public class GhostPepperFlavorInput : FlavorInputManager
{
    SpriteRenderer spriteRenderer;
    CharacterData characterData;
    public AudioClip hurtSfx;
    public GameObject deathSfxPlayer;

    private float dotTicLength = 1;
    // Start is called before the first frame update
    void Start()
    {
        InitializeDictionary();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterData = GetComponent<CharacterData>();
    }


    public override void Feed(RecipeData.Flavors flavor, int magnitude)
    {
        Debug.Log("Paddlepear ate " + magnitude + " " + flavor);
        flavorCountDictionary[flavor] += magnitude;

        //handle spicy
        if (flavorCountDictionary[RecipeData.Flavors.Spicy] >= 0)
        {
            characterData.health += flavorCountDictionary[RecipeData.Flavors.Spicy];
            flavorCountDictionary[RecipeData.Flavors.Spicy] = 0;
        }
        if(flavorCountDictionary[RecipeData.Flavors.Sweet] > 0)
        {
            characterData.moods["Friendliness"] = 0.125f * flavorCountDictionary[RecipeData.Flavors.Sweet];
        }


    }

    public void DamageOverTime(int numTics, float ticLength)
    {
        if(numTics > 0)
        {
            characterData.health -= 1;
            Debug.Log("Health reduced to " + characterData.health + " by DoT effect");

            if (characterData.health <= 0)
            {
                //Debug.Log("Killing Monster");
                Monster targetMonster = GetComponent<Monster>();
                targetMonster.Kill();
                return;
            }

            var deathSoundObj = Instantiate(deathSfxPlayer, transform.position, transform.rotation);
            deathSoundObj.GetComponent<PlayAndDestroy>().Play(hurtSfx);

            StartCoroutine(ExecuteAfterSeconds(ticLength, numTics));
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    protected IEnumerator ExecuteAfterSeconds(float time, int tic)
    {
        //things to happen before delay

        yield return new WaitForSeconds(time);

        //things to happen after delay
        DamageOverTime(tic - 1, dotTicLength);

        yield return null;
    }
}