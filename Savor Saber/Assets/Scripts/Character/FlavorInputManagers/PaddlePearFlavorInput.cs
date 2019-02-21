using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Monster))]
[RequireComponent(typeof(CharacterData))]
public class PaddlePearFlavorInput : FlavorInputManager
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
            spriteRenderer.color = Color.red;
            DamageOverTime(2 * flavorCountDictionary[RecipeData.Flavors.Spicy], dotTicLength);
            //Only reset the spicy stat or else no other stats will work
            flavorCountDictionary[RecipeData.Flavors.Spicy] = 0;
        }
        if(flavorCountDictionary[RecipeData.Flavors.Sweet] > 0)
        {
            characterData.moods["Friendliness"] = 0.125f * flavorCountDictionary[RecipeData.Flavors.Sweet];
        }


    }

    public void DamageOverTime(int numTics, float ticLength)
    {
        bool killingBlow = false;
        if(numTics > 0)
        {
            //test to see if this tic will inflict a killing blow
            if (characterData.health - 1 <= 0)
                killingBlow = true;
           
            characterData.DoDamage(1);
            Debug.Log("Health reduced to " + characterData.health + " by DoT effect");

            if (killingBlow)
                return;

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
