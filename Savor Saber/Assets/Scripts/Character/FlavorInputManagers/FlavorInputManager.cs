﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathUtils;

public class FlavorInputManager : MonoBehaviour
{
    #region Feeding
    protected Dictionary<RecipeData.Flavors, int> flavorCountDictionary = new Dictionary<RecipeData.Flavors, int>();
    protected Dictionary<IngredientData, int> ingredientCountDictionary = new Dictionary<IngredientData, int>();
    public IngredientData[] favoriteIngredients;
    public RecipeData.Flavors favoriteFlavors;
    public int charmThreshhold = 1;
    protected bool fedFavoriteIngredient = false;
    public GameObject rewardItem;
    public int amountRewardItem = 2;
    public AudioClip rewardSFX;
    public AudioClip rejectSFX;

    // timers
    public float charmTime = 0;
    public float shieldSize = 1;
    #endregion

    #region Components
    protected AudioSource sfxPlayer;
    protected AIData characterData;
    protected SpriteRenderer spriteRenderer;
    #endregion

    #region Other
    protected PointVector pv = new PointVector();
    public float dotTicLength = 1;
    protected int electricBaseTime = 10;
    public GameObject electricFieldTemplate;
    public GameObject saltShieldTemplate;
    public AudioClip electricSFX;
    protected bool isElectricSupercharged = false;
    GameObject electricFieldEffect;
    #endregion

    private void Start()
    {
        InitializeDictionary();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sfxPlayer = GetComponent<AudioSource>();

        // differentiate between ai and char data
        characterData = GetComponent<AIData>();
        if (characterData == null)
            characterData = (AIData)GetComponent<CharacterData>();

        /* if (this.gameObject.tag == "ElectricAoE"){
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

    public void InitializeDictionary()
    {
        flavorCountDictionary.Add(RecipeData.Flavors.Sweet, 0);
        flavorCountDictionary.Add(RecipeData.Flavors.Sour, 0);
        flavorCountDictionary.Add(RecipeData.Flavors.Spicy, 0);
        flavorCountDictionary.Add(RecipeData.Flavors.Salty, 0);
        flavorCountDictionary.Add(RecipeData.Flavors.Umami, 0);
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
        flavorCountDictionary[RecipeData.Flavors.Umami] = 0;
        flavorCountDictionary[RecipeData.Flavors.Bitter] = 0;
        flavorCountDictionary[RecipeData.Flavors.Acquired] = 0;

        ingredientCountDictionary.Clear();
    }

    public virtual void Feed(IngredientData[] ingredientArray, bool fedByPlayer)
    {
        //Debug.Log("Skewer of size " + ingredientArray.Length);
        for(int i = 0; i < ingredientArray.Length; i++)
        {
            IngredientData ingredient = ingredientArray[i];
            if (ingredientCountDictionary.ContainsKey(ingredient))
            {
                ingredientCountDictionary[ingredient] = ingredientCountDictionary[ingredient] + 1;
                //Debug.Log("Ate one " + ingredient.displayName);
            }
            else
            {
                ingredientCountDictionary.Add(ingredient, 1);
                //Debug.Log("Ate one " + ingredient.displayName);
            }

            // favorite flavors
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

        RespondToIngredients(fedByPlayer);

        // fruitants no longer spawn a reward
        //SpawnReward(ingredientArray, fedByPlayer);
    }

    public virtual void SpawnReward(IngredientData[] ingredientArray, bool fedByPlayer)
    {
        if (!fedByPlayer)
            return;

        // looking only at favorite ingredients...
        bool moreFriendly = (characterData != null);
        if (moreFriendly && fedByPlayer)
            characterData.InstantiateSignal(1f, "Friendliness", 0.1f * ingredientArray.Length, true, true);

        foreach (var favoriteIngredient in favoriteIngredients)
        {
            // if the ingredients on the skewer are my favorites...
            if (ingredientCountDictionary.ContainsKey(favoriteIngredient))
            {
                // am i actually being fed this...
                float amountOnSkewer = ingredientCountDictionary[favoriteIngredient];
                if (amountOnSkewer > 0)
                {
                    //Debug.Log(this.gameObject + " fed favorite ingredient! = how many? " + amountOnSkewer);
                    fedFavoriteIngredient = true;

                    if (rewardItem != null)
                    {
                        int spawned = 0;
                        for (int j = 0; j < amountOnSkewer; j++)
                        {
                            for (int i = 0; i < amountRewardItem; i++)
                            {
                                Instantiate(rewardItem, transform.position, Quaternion.identity);
                                spawned++;
                            }
                        }
                        Debug.Log("Spawned: " + spawned);
                    }
                }

                if (sfxPlayer != null)
                {
                    sfxPlayer.clip = rewardSFX;
                    sfxPlayer.Play();
                }

                ingredientCountDictionary[favoriteIngredient] = 0;
            }
        }
    }


    public virtual void RespondToIngredients(bool fedByPlayer)
    {

        // heal the fruitant
        if (fedByPlayer && characterData != null)
            characterData.DoHeal(flavorCountDictionary.Count * 2);

        foreach (var favoriteIngredient in favoriteIngredients)
        {
            // if the ingredients on the skewer are my favorites...
            if (ingredientCountDictionary.ContainsKey(favoriteIngredient))
            {
                // am i actually being fed this...
                float amountOnSkewer = ingredientCountDictionary[favoriteIngredient];

                // play audio
                if (sfxPlayer != null)
                {
                    sfxPlayer.clip = rewardSFX;
                    sfxPlayer.Play();
                }
            }
        }

        // reset dicts
        ResetDictionary();
    }

    #region Flavor Responses
    #region CHARM
    protected void CheckCharmEffect(bool favorite)
    {
        // the amount of time that a fruitant is charmed
        Debug.Log("CHARMED");
        StopCoroutine("ExecuteCharm");
        float time = flavorCountDictionary[RecipeData.Flavors.Sweet] * (favorite ? 40f : 20f);
        charmTime += time;

        if(!PlayerController.instance.riding)
            StartCharm(charmTime);

        //characterData.InstantiateSignal(1f, "Friendliness", 0.5f, true, true);
    }

    protected void StartCharm(float time)
    {
        // get protocols

        //Debug.Log("should be CHARMED for " + time + " seconds");
        MonsterProtocols proto = characterData.getProtocol();
        MonsterChecks check = characterData.getChecks();
        // initiate conga
        characterData.currentProtocol = AIData.Protocols.Conga;

        if (characterData.path != null)
            characterData.path.Clear();

        // set leader to Soma
        check.specialLeader = PlayerController.instance.gameObject;
        // add self to player's party
        PlayerController.instance.GetComponent<PlayerData>().party.Add(gameObject);
        // get ready to stop it
        StartCoroutine(ExecuteCharm(time));
    }

    protected void StopCharm()
    {
        // get protocols
        MonsterProtocols proto = characterData.getProtocol();
        MonsterChecks check = characterData.getChecks();
        // no leader
        check.ResetSpecials();
        check.specialLeader = null;
        check.congaPosition = -1;
        // remove self from player's party
        PlayerController.instance.GetComponent<PlayerData>().party.Remove(gameObject);
        // initiate conga
        characterData.currentProtocol = AIData.Protocols.Lazy;

        Debug.Log("no longer CHARMED");
    }

    protected IEnumerator ExecuteCharm(float time)
    {
        //things to happen before delay
        yield return new WaitForSeconds(time);
        //things to happen after delay
        StopCharm();
        yield return null;
    }
    #endregion

    #region CURRY
    public virtual void CurryBalls (bool favorite)
    {
        // the amount of time that a fruitant is charmed
        int shots = 5;
        int pellets = 5;
        dotTicLength = 0.5f;
        StartCoroutine(ExecuteCurry(dotTicLength, shots, pellets));
    }

    protected virtual IEnumerator ExecuteCurry(float time, int shots, int pellets)
    {
        //things to happen before delay
        GameObject newAttack;
        MonsterBehavior behave = this.GetComponent<MonsterBehavior>();

        // null check on curryball projectile
        if (behave.projectile == null)
            yield return null;

        // spawn the amount of shots with the amount of pellets
        var s = shots;

        // toggle between red and more red
        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        float colorLerp = 0.25f;
        sr.color = Color.Lerp(Color.yellow, Color.red, colorLerp);
        while (s > 0)
        {
            colorLerp += Time.deltaTime * 15;
            sr.color = Color.Lerp(Color.yellow, Color.red, colorLerp);

            yield return new WaitForSeconds(time);
            colorLerp = 0.5f;
            // spawn curry balls
            var split = 360 / pellets;
            for(int i = 0; i < pellets; i++)
            {
                // spawn curry ball at an angle
                newAttack = Instantiate(behave.projectile, transform.position, Quaternion.identity);
                Vector2 dir = Vector2.ClampMagnitude(pv.Ang2Vec((split * i) + (s * 30)), 1f);/*+ Random.Range(-split/4, split/4)*/

                BaseProjectile projectileData = newAttack.GetComponent<BaseProjectile>();

                projectileData.directionVector = dir;
                projectileData.penetrateTargets = true;
                projectileData.attacker = this.gameObject;
            }
            s--;
        }
        //things to happen after delay
        Debug.Log("no longer CURRIED");
        spriteRenderer.color = Color.white;
        yield return null;
    }
    #endregion

    #region SOUR
    /*protected IEnumerator ElectricTimer(float time)
    {

        //things to happen before delay
        isElectricSupercharged = true;
        var powerCharger = electricFieldEffect.GetComponent<PoweredObjectCharger>();
        sfxPlayer.clip = electricSFX;
        sfxPlayer.Play();
        powerCharger.enabled = true;
        yield return new WaitForSeconds(time);

        //things to happen after delay
        powerCharger.GetComponent<SpriteRenderer>().color = new Color(255,255,255,255);
        powerCharger.enabled = false;
        isElectricSupercharged = false;
        yield return null;
    }*/
    #endregion

    #region SALT
    protected void SaltyShield(bool favorite)
    {
        float time = flavorCountDictionary[RecipeData.Flavors.Salty] * (favorite ? 20f : 10f);

        GameObject shield = Instantiate(saltShieldTemplate, transform.position, Quaternion.identity);
        shield.transform.parent = gameObject.transform;
        shield.transform.localScale = new Vector3(shieldSize, shieldSize);

        SaltShield ss = shield.GetComponent<SaltShield>();
        ss.fruit = this.gameObject;
        ss.lifetime = time;
    }
    #endregion
    #endregion


    public void DamageOverTime(int numTics, float ticLength)
    {
        bool killingBlow = false;
        if (numTics > 0)
        {
            //test to see if this tic will inflict a killing blow
            if (characterData.health - 1 <= 0)
                killingBlow = true;

            characterData.DoDamage(1);
            //Debug.Log("Health reduced to " + characterData.health + " by DoT effect");

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


#region Old Code
/*foreach(string favoriteIngredient in favoriteIngredients)
        {
            if (favoriteIngredient != null &&
            ingredientCountDictionary.ContainsKey(favoriteIngredient) &&
            ingredientCountDictionary[favoriteIngredient] >= charmThreshhold)
            {
                Debug.Log(this.gameObject + " fed favorite ingredient!");
                fedFavoriteIngredient = true;

                if (characterData != null)
                {
                    // more juice
                    // give more things for feeding the fruitant
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
        }*/


/*for (int f = 1; f <= 64; f = f << 1)
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
}*/

/* OLD FLAVOR REPSONSES
 * fruitants no have specific responses based being fed their favorites

    //handle spicy
    if (flavorCountDictionary[RecipeData.Flavors.Spicy] > 0)
    {
        if (fedByPlayer)
        {
            CurryBalls((favoriteFlavors == RecipeData.Flavors.Spicy));
        }
    }
    //handle sweet
    if (flavorCountDictionary[RecipeData.Flavors.Sweet] > 0)
    {
        if (fedByPlayer)
        {
            CheckCharmEffect((favoriteFlavors == RecipeData.Flavors.Sweet));
        }

    }
    //handle umami
    if (flavorCountDictionary[RecipeData.Flavors.Umami] > 0)
    {
        if (favoriteFlavors != RecipeData.Flavors.Umami)
        {
            // nothing for now
        }
    }
    //handle sour
    if (flavorCountDictionary[RecipeData.Flavors.Sour] > 0)
    {
        if (fedByPlayer)
        {
            StartCoroutine(ElectricTimer(electricBaseTime * flavorCountDictionary[RecipeData.Flavors.Sour] * (favoriteFlavors == RecipeData.Flavors.Sour ? 2 : 1)));
        }
    }
    //handle salty
    if (flavorCountDictionary[RecipeData.Flavors.Salty] > 0)
    {
        if (fedByPlayer)
        {
            SaltyShield(favoriteFlavors == RecipeData.Flavors.Salty);
        }
    }

    */
#endregion
