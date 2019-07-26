using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathUtils;

public class FlavorInputManagerStateChanger : FlavorInputManager
{
    /// <summary>
    /// Which state the fruitant is currently in
    /// </summary>
    protected RecipeData.Flavors flavorState = RecipeData.Flavors.None;

    //public GameObject shieldTemplate;
    //public GameObject teslaTemplate;
    //public GameObject ProjectileTemplate;

    /// <summary>
    /// Whatever entity is currently being spawned as part of the fruitant's state
    /// </summary>
    public GameObject currentActionEffect;

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

    public override void Feed(IngredientData[] ingredientArray, bool fedByPlayer)
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

            // mod hunger
            if (characterData != null)
            {
                characterData.InstantiateSignal(0.5f, "Hunger", -0.5f, false, true);
            }
                

            for (int f = 1; f <= 64; f = f << 1)
            {

                if ((f & (int)ingredient.flavors) > 0)
                {
                    //for the purposes of this prototype, we only check the first flavor present.
                    RecipeData.Flavors foundFlavor = (RecipeData.Flavors)f;
                    SetFlavorState(foundFlavor);
                    break;
                }
            }
        }

        RespondToIngredients(fedByPlayer);
        SpawnReward(ingredientArray, fedByPlayer);
    }


    public override void SpawnReward(IngredientData[] ingredientArray, bool fedByPlayer)
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

    protected void SetFlavorState(RecipeData.Flavors state)
    {
        if(state == RecipeData.Flavors.Spicy)
        {
            if(currentActionEffect != null)
                Destroy(currentActionEffect);
            CurryBalls(true);
        }
        else if(state == RecipeData.Flavors.Sour)
        {
            if (currentActionEffect != null)
                Destroy(currentActionEffect);
            currentActionEffect = Instantiate(electricFieldTemplate, transform.position, Quaternion.identity);
            currentActionEffect.transform.parent = transform;
        }
        else if(state == RecipeData.Flavors.Salty)
        {
            if (currentActionEffect != null)
                Destroy(currentActionEffect);
            currentActionEffect = Instantiate(saltShieldTemplate, transform.position, Quaternion.identity);
            currentActionEffect.transform.parent = transform;
            currentActionEffect.transform.localScale = new Vector3(shieldSize, shieldSize);
        }
    }

    public override void RespondToIngredients(bool fedByPlayer)
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

        // reset flavor dicts
        ResetDictionary();
    }

    #region CURRY
    protected override void CurryBalls(bool favorite)
    {
        // the amount of time that a fruitant is charmed
        Debug.Log("CURRIED");
        var spice = flavorCountDictionary[RecipeData.Flavors.Spicy];
        int shots = 3 + spice + (favorite ? 3 : 0);
        int pellets = 1 + spice + (favorite ? 2 : 1);
        dotTicLength = 0.5f;
        StartCoroutine(ExecuteCurry(dotTicLength, shots, pellets, true));
    }

    protected IEnumerator ExecuteCurry(float time, int shots, int pellets, bool repeat)
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
            for (int i = 0; i < pellets; i++)
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
        if (repeat)
        {
            CurryBalls(true);
        }
        yield return null;
    }
    #endregion
}
