using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathUtils;

public class TutorialFlavorInput : FlavorInputManager
{

    #region Deprecated Fields
    //private int amountRewardItem = 2;
    #endregion
    [Header("Tutorial cutscene settings")]
    public TutorialPearFeedManager feedManager;
    public int limitPerFruitant = 1;
    private int amountFed = 0;
    private bool countingActive = false;

    private void Start()
    {
        InitializeDictionary();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sfxPlayer = GetComponent<AudioSource>();

        // differentiate between ai and char data
        characterData = GetComponent<AIData>();
        if (characterData == null)
            characterData = (AIData)GetComponent<CharacterData>();

        /*
        if (this.gameObject.tag == "ElectricAoE"){
            electricFieldEffect = Instantiate(electricFieldTemplate, transform.position, Quaternion.identity, gameObject.transform);
            electricFieldEffect.GetComponent<PoweredObjectCharger>().enabled = false;
        }*/

        // set favorite food speech bubble
        if (this.tag == "Prey")
        {
            FavoriteFoodBubble ffb = GetComponentInChildren<FavoriteFoodBubble>();
            ffb.fruitant = this.gameObject;
            ffb.favoriteFood1 = favoriteIngredients[0];
        }
    }

    //just a temporary conversion from the old feeding to the new feeding, so the tutorial still works
    public override void Feed(IngredientData ingredient, bool fedByPlayer)
    {
        IngredientData[] ingredientArray = { ingredient };
        Feed(ingredientArray, fedByPlayer);
    }

    public override void Feed(IngredientData[] ingredientArray, bool fedByPlayer)
    {
        bool healed = false;
        bool rejected = false;
        foreach (IngredientData data in ingredientArray)
        {
            bool isFavorite = false;
            bool isReject = false;
            foreach (IngredientData favoriteIngredient in favoriteIngredients)
            {
                if (data == favoriteIngredient)
                {
                    characterData.DoHeal(99);
                    healed = true;
                    isFavorite = true;

                    //FOR TUTORIAL: count how much this feeding adds to the total
                    if(countingActive && (amountFed < limitPerFruitant))
                    {
                        feedManager.Feed(1);
                        amountFed++;
                    }
                }
            }
            foreach (IngredientData rejectedIngredient in rejectedIngredients)
            {
                if (data == rejectedIngredient)
                {
                    rejected = true;
                    isReject = true;

                    //spit out the rejected object
                    GameObject rejectedObject = Instantiate(rejectedObjectTemplate, transform.position, Quaternion.identity);
                    SpriteRenderer rejectedSR = rejectedObject.GetComponent<SpriteRenderer>();
                    SkewerableObject rejectedSO = rejectedObject.GetComponent<SkewerableObject>();
                    rejectedSR.sprite = rejectedIngredient.image;
                    rejectedSO.data = rejectedIngredient;
                }
            }
            //heal mildly if neither favorite nor reject
            if (!isFavorite && !isReject)
            {
                characterData.DoHeal(3);
                healed = true;
            }
        }

        //play audio based on what happened during this feeding
        if (healed && !rejected)
        {
            if (sfxPlayer != null)
            {
                sfxPlayer.clip = rewardSFX;
                sfxPlayer.Play();
            }
        }
        else if (rejected && !healed)
        {
            if (sfxPlayer != null)
            {
                sfxPlayer.clip = rejectSFX;
                sfxPlayer.Play();
            }
            Debug.Log("wtf, why would you feed me this");
        }
        else if (rejected && healed)
        {
            if (sfxPlayer != null)
            {
                sfxPlayer.clip = rewardSFX;
                sfxPlayer.Play();
            }
            Debug.Log("wtf, why would you feed me this");
        }
    }

    public void EnableCounting()
    {
        countingActive = true;
    }
}