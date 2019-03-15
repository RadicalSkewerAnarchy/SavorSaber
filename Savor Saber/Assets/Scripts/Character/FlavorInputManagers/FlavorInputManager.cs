﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlavorInputManager : MonoBehaviour
{
    /// <summary>
    /// How much of each flavor this entity has been fed
    /// </summary>
    protected Dictionary<RecipeData.Flavors, int> flavorCountDictionary = new Dictionary<RecipeData.Flavors, int>();
    protected Dictionary<string, int> ingredientCountDictionary = new Dictionary<string, int>();
    protected CharacterData characterData;
    protected SpriteRenderer spriteRenderer;

    public float dotTicLength = 1;
    public string[] favoriteIngredients;
    public RecipeData.Flavors favoriteFlavors;
    public int charmThreshhold = 1;
    public GameObject rewardItem;
    public AudioClip rewardSFX;

    private bool fedFavoriteIngredient = false;

    private void Start()
    {
        InitializeDictionary();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterData = GetComponent<CharacterData>();
    }

    public void InitializeDictionary()
    {
        flavorCountDictionary.Add(RecipeData.Flavors.Sweet, 0);
        flavorCountDictionary.Add(RecipeData.Flavors.Sour, 0);
        flavorCountDictionary.Add(RecipeData.Flavors.Spicy, 0);
        flavorCountDictionary.Add(RecipeData.Flavors.Salty, 0);
        flavorCountDictionary.Add(RecipeData.Flavors.Savory, 0);
        flavorCountDictionary.Add(RecipeData.Flavors.Bitter, 0);
        flavorCountDictionary.Add(RecipeData.Flavors.Acquired, 0);
    }

    /// <summary>
    /// Resets the count of each flavor to 0
    /// </summary>
    public void ResetDictionary()
    {
        flavorCountDictionary[RecipeData.Flavors.Sweet] = 0;
        flavorCountDictionary[RecipeData.Flavors.Sour] = 0;
        flavorCountDictionary[RecipeData.Flavors.Spicy] = 0;
        flavorCountDictionary[RecipeData.Flavors.Salty] = 0;
        flavorCountDictionary[RecipeData.Flavors.Savory] = 0;
        flavorCountDictionary[RecipeData.Flavors.Bitter] = 0;
        flavorCountDictionary[RecipeData.Flavors.Acquired] = 0;
    }

    public virtual void Feed(IngredientData[] ingredientArray)
    {
        for(int i = 0; i < ingredientArray.Length; i++)
        {
            IngredientData ingredient = ingredientArray[i];
            if (ingredientCountDictionary.ContainsKey(ingredient.displayName))
            {
                ingredientCountDictionary[ingredient.displayName] = ingredientCountDictionary[ingredient.displayName] + 1;
                Debug.Log("Ate one " + ingredient.displayName);
            }
            else
            {
                ingredientCountDictionary.Add(ingredient.displayName, 1);
                Debug.Log("Ate one " + ingredient.displayName);
            }

            for (int f = 1; f <= 64; f = f << 1)
            {

                if ((f & (int)ingredient.flavors) > 0)
                {
                    RecipeData.Flavors foundFlavor = (RecipeData.Flavors)f;
                    flavorCountDictionary[foundFlavor] = flavorCountDictionary[foundFlavor] + 1;
                    Debug.Log(ingredient.displayName + " has flavor " + foundFlavor);
                }
            }
        }
        RespondToIngredients();
    }

    public virtual void RespondToIngredients()
    {
        CheckCharmEffect();
    }

    protected void CheckCharmEffect()
    {
        foreach(string favoriteIngredient in favoriteIngredients)
        {
            if (favoriteIngredient != null &&
            ingredientCountDictionary.ContainsKey(favoriteIngredient) &&
            ingredientCountDictionary[favoriteIngredient] >= charmThreshhold)
            {
                Debug.Log(this.gameObject + " fed favorite ingredient!");
                fedFavoriteIngredient = true;

                if (characterData != null)
                {
                    characterData.moods["Hunger"] = 0;
                    if (rewardItem != null)
                        Instantiate(rewardItem, transform.position, Quaternion.identity);
                }
                AudioSource rewardSFXPlayer = GetComponent<AudioSource>();
                if(rewardSFXPlayer != null)
                {
                    rewardSFXPlayer.clip = rewardSFX;
                    rewardSFXPlayer.Play();
                }
                ingredientCountDictionary[favoriteIngredient] = 0;
            }
        }


        for (int f = 1; f <= 64; f = f << 1)
        {
            //only compare entries in favorite flavors
            if ((f & (int)favoriteFlavors) > 0)
            {
                RecipeData.Flavors foundFlavor = (RecipeData.Flavors)f;
                if (flavorCountDictionary[foundFlavor] >= charmThreshhold)
                {
                    Debug.Log(this.gameObject + " charmed by feeding favorite flavor!");
                    fedFavoriteIngredient = true;
                    if (characterData != null)
                    {
                        characterData.moods["Hunger"] = 0;
                        if (rewardItem != null)
                            Instantiate(rewardItem, transform.position, Quaternion.identity);
                    }
                    AudioSource rewardSFXPlayer = GetComponent<AudioSource>();
                    if (rewardSFXPlayer != null)
                    {
                        rewardSFXPlayer.clip = rewardSFX;
                        rewardSFXPlayer.Play();
                    }
                    flavorCountDictionary[foundFlavor] = 0;
                }
            }
        }
    }

    public void DamageOverTime(int numTics, float ticLength)
    {
        bool killingBlow = false;
        if (numTics > 0)
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

    /// <summary>
    /// Used by the skewer projectile to make sure it's not feeding a favorite ingredient
    /// Avoids overlapping sfx
    /// </summary>
    public bool FedFavorite()
    {
        if (fedFavoriteIngredient)
        {
            fedFavoriteIngredient = false;
            return true;
        }
        else return false;
    }
}
