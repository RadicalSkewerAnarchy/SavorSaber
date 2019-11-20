using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathUtils;

public class TutorialFlavorInput : FlavorInputManager
{

    #region Deprecated Fields
    private int amountRewardItem = 2;
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

    public override void Feed(IngredientData[] ingredientArray, bool fedByPlayer)
    {
        #region Old feeding checks
        /*
        for(int i = 0; i < ingredientArray.Length; i++)
        {
            IngredientData ingredient = ingredientArray[i];
            if (ingredientCountDictionary.ContainsKey(ingredient))
            {
                ingredientCountDictionary[ingredient] = ingredientCountDictionary[ingredient] + 1;
            }
            else
            {
                ingredientCountDictionary.Add(ingredient, 1);
            }
            */
        // ingredients don't have flavors anymore, so we no longer need to check flavors
        //restore this if we want to check flavors again
        /*
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
    */
        //RespondToIngredients(fedByPlayer);

        // fruitants no longer spawn a reward
        //SpawnReward(ingredientArray, fedByPlayer);
        #endregion

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


    public override void RespondToIngredients(bool fedByPlayer)
    {

        // heal the fruitant
        if (fedByPlayer && characterData != null)
            characterData.DoHeal(flavorCountDictionary.Count * 2);

        //should I reject anything I ate?
        foreach (var rejectedIngredient in rejectedIngredients)
        {
            // if the ingredients on the skewer are my favorites...
            if (ingredientCountDictionary.ContainsKey(rejectedIngredient))
            {
                // am i actually being fed this...
                float amountOnSkewer = ingredientCountDictionary[rejectedIngredient];

                // play audio
                if (sfxPlayer != null)
                {
                    sfxPlayer.clip = rejectSFX;
                    sfxPlayer.Play();
                    Debug.Log("wtf, why would you feed me this");

                    //spit out the rejected object
                    GameObject rejectedObject = Instantiate(rejectedObjectTemplate, transform.position, Quaternion.identity);
                    SpriteRenderer rejectedSR = rejectedObject.GetComponent<SpriteRenderer>();
                    SkewerableObject rejectedSO = rejectedObject.GetComponent<SkewerableObject>();
                    rejectedSR.sprite = rejectedIngredient.image;
                    rejectedSO.data = rejectedIngredient;
                }
            }
        }

        // reset dicts
        ResetDictionary();
    }

    public void EnableCounting()
    {
        countingActive = true;
    }
}