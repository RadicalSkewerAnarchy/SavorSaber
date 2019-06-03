using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathUtils;

public class FlavorInputManager : MonoBehaviour
{
    #region Feeding
    protected Dictionary<RecipeData.Flavors, int> flavorCountDictionary = new Dictionary<RecipeData.Flavors, int>();
    protected Dictionary<string, int> ingredientCountDictionary = new Dictionary<string, int>();
    public string[] favoriteIngredients;
    public RecipeData.Flavors favoriteFlavors;
    public int charmThreshhold = 1;
    private bool fedFavoriteIngredient = false;
    public GameObject rewardItem;
    public int amountRewardItem = 2;
    public AudioClip rewardSFX;
    #endregion

    #region Components
    private AudioSource sfxPlayer;
    protected AIData characterData;
    protected SpriteRenderer spriteRenderer;
    #endregion

    #region Other
    private PointVector pv = new PointVector();
    public float dotTicLength = 1;
    private int electricBaseTime = 10;
    public GameObject electricFieldTemplate;
    public AudioClip electricSFX;
    private bool isElectric = false;
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

    public virtual void Feed(IngredientData[] ingredientArray, bool fedByPlayer)
    {
        Debug.Log("Skewer of size " + ingredientArray.Length);
        for(int i = 0; i < ingredientArray.Length; i++)
        {
            IngredientData ingredient = ingredientArray[i];
            if (ingredientCountDictionary.ContainsKey(ingredient.displayName))
            {
                ingredientCountDictionary[ingredient.displayName] = ingredientCountDictionary[ingredient.displayName] + 1;
                //Debug.Log("Ate one " + ingredient.displayName);
            }
            else
            {
                ingredientCountDictionary.Add(ingredient.displayName, 1);
                //Debug.Log("Ate one " + ingredient.displayName);
            }

            // mod hunger
            if (characterData != null)
            {
                characterData.InstantiateSignal(0.5f, "Hunger", -0.3f, false, true);
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

        RespondToIngredients(fedByPlayer);
        SpawnReward(ingredientArray, fedByPlayer);
    }


    protected void SpawnReward(IngredientData[] ingredientArray, bool fedByPlayer)
    {
        if (!fedByPlayer)
            return;
        // looking only at favorite ingredients...
        bool moreFriendly = (characterData != null);
        if (moreFriendly && fedByPlayer)
            characterData.InstantiateSignal(1f, "Friendliness", 0.1f * ingredientArray.Length, true, true);

        foreach (string favoriteIngredient in favoriteIngredients)
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
        if (flavorCountDictionary[RecipeData.Flavors.Savory] > 0)
        {
            if (favoriteFlavors != RecipeData.Flavors.Savory)
            {
                // nothing for now
            }
        }
        //handle sour
        if (flavorCountDictionary[RecipeData.Flavors.Sour] > 0)
        {
            if (favoriteFlavors != RecipeData.Flavors.Savory)
            {
                if (fedByPlayer && !isElectric)
                {
                    StartCoroutine(ElectricTimer(electricBaseTime * flavorCountDictionary[RecipeData.Flavors.Sour]));
                }
            }
        }
        // reset flavor dicts
        ResetDictionary();
    }

    #region CHARM
    protected void CheckCharmEffect(bool favorite)
    {
        // the amount of time that a fruitant is charmed
        Debug.Log("CHARMED");
        float time = flavorCountDictionary[RecipeData.Flavors.Sweet] * (favorite ? 60f : 30f);
        StartCharm(time);
        //characterData.DoDamage(-3);
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
    protected void CurryBalls (bool favorite)
    {
        // the amount of time that a fruitant is charmed
        Debug.Log("CURRIED");
        var spice = flavorCountDictionary[RecipeData.Flavors.Spicy];
        int shots =   spice + spice*(favorite ? 3 : 2) + (spice == 3 ? 3 : 0);
        int pellets = spice + (favorite ? 2 : 1) + (spice == 3 ? 1 : 0);
        dotTicLength = 0.5f;
        StartCoroutine(ExecuteCurry(dotTicLength, shots, pellets));
    }

    protected IEnumerator ExecuteCurry(float time, int shots, int pellets)
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
        sr.color = Color.red;
        while (s > 0)
        {
            yield return new WaitForSeconds(time);
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
                projectileData.projectileSpeed = 3f;
                projectileData.range = 4f;
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
    protected IEnumerator ElectricTimer(float time)
    {
        //things to happen before delay
        isElectric = true;
        GameObject electricField = Instantiate(electricFieldTemplate, transform.position, Quaternion.identity);
        electricField.transform.parent = gameObject.transform;
        sfxPlayer.clip = electricSFX;
        sfxPlayer.Play();

        yield return new WaitForSeconds(time);

        //things to happen after delay
        Destroy(electricField);
        isElectric = false;
        yield return null;
    }
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
#endregion