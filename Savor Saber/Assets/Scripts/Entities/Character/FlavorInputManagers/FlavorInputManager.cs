using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathUtils;

public class FlavorInputManager : MonoBehaviour
{
    #region Feeding
    public bool canTransformWhenFed = false;
    public bool isCompanion = false;
    protected Dictionary<RecipeData.Flavors, int> flavorCountDictionary = new Dictionary<RecipeData.Flavors, int>();
    protected Dictionary<IngredientData, int> ingredientCountDictionary = new Dictionary<IngredientData, int>();
    public IngredientData[] favoriteIngredients;
    public IngredientData[] rejectedIngredients;

    protected bool fedFavoriteIngredient = false;

    public AudioClip rewardSFX;
    public AudioClip rejectSFX;

    #endregion

    #region Components
    protected AudioSource sfxPlayer;
    protected AIData characterData;
    protected SpriteRenderer spriteRenderer;
    #endregion

    #region Other
    protected PointVector pv = new PointVector();
    public float dotTicLength = 1;
    [SerializeField]
    protected GameObject rejectedObjectTemplate;
    [SerializeField]
    protected ParticleSystem spawnParticles;
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

    public virtual void Feed(IngredientData ingredient, bool fedByPlayer, CharacterData feederData)
    {
        Debug.Log(this.gameObject.name + " is eating " + ingredient.displayName);
        bool rejected = false;
        //check to see if we should reject this
        foreach (IngredientData rejectedIngredient in rejectedIngredients)
        {
            if (ingredient == rejectedIngredient)
            {
                rejected = true;
                //spit out the rejected object
                SpawnRejectedIngredient(ingredient);
            }
        }
        //if we didn't reject it, heal and check if we should morph
        if (!rejected)
        {
            characterData.DoHeal(99);

            if (canTransformWhenFed && ingredient.monster != null)
            {
                GameObject newMorph = Instantiate(ingredient.monster, transform.position, Quaternion.identity);
                //if this is the companion, keep it in the party
                if (isCompanion)
                {
                    //Debug.Log("Entering 'is companion' field of Feed()");
                    PlayerData somaData = (PlayerData)feederData;
                    somaData.JoinTeam(newMorph, 1, true);
                    somaData.SetCurrentFormIngreident(ingredient);
                    FlavorInputManager newFIM = newMorph.GetComponent<FlavorInputManager>();
                    newFIM.isCompanion = true;
                    newFIM.PlaySpawnParticles();

                    newMorph.transform.parent = transform.parent;
                }
                Destroy(this.gameObject);
            }
        }
    }


    public virtual void PlaySpawnParticles()
    {
        if (spawnParticles != null)
            spawnParticles.Play();
        else
            Debug.Log("FlavorInputManager Error: No particle system");
    }

    //OLD FEEDING CODE - OUTDATED, KEPT FOR LEGACY PURPOSES
    public virtual void Feed(IngredientData[] ingredientArray, bool fedByPlayer)
    {

        bool healed = false;
        bool rejected = false;
        foreach(IngredientData data in ingredientArray)
        {
            bool isFavorite = false;
            bool isReject = false;
            foreach(IngredientData favoriteIngredient in favoriteIngredients)
            {
                if(data == favoriteIngredient)
                {
                    //healing with a favorite ingredient is very effective
                    characterData.DoHeal(99);
                    healed = true;
                    isFavorite = true;
                }
            }
            foreach (IngredientData rejectedIngredient in rejectedIngredients)
            {
                if (data == rejectedIngredient)
                {
                    rejected = true;
                    isReject = true;
                    SpawnRejectedIngredient(rejectedIngredient);
                    //spit out the rejected object
                    //GameObject rejectedObject = Instantiate(rejectedObjectTemplate, transform.position, Quaternion.identity);
                    //SpriteRenderer rejectedSR = rejectedObject.GetComponent<SpriteRenderer>();
                    //SkewerableObject rejectedSO = rejectedObject.GetComponent<SkewerableObject>();
                    //rejectedSR.sprite = rejectedIngredient.image;
                    //rejectedSO.data = rejectedIngredient;
                }
            }
            //heal mildly if neither favorite nor reject
            if(!isFavorite && !isReject)
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
        else if(rejected && healed)
        {
            if (sfxPlayer != null)
            {
                sfxPlayer.clip = rewardSFX;
                sfxPlayer.Play();
            }
            Debug.Log("wtf, why would you feed me this");
        }
    }

    protected void SpawnRejectedIngredient(IngredientData data)
    {
        GameObject rejectedObject = Instantiate(rejectedObjectTemplate, transform.position, Quaternion.identity);
        SpriteRenderer rejectedSR = rejectedObject.GetComponent<SpriteRenderer>();
        SkewerableObject rejectedSO = rejectedObject.GetComponent<SkewerableObject>();
        rejectedSR.sprite = data.image;
        rejectedSO.data = data;
        sfxPlayer.clip = rejectSFX;
        sfxPlayer.Play();
    }
 
    #region SUGAR
    [HideInInspector]
    public int sugarCount = 0;
    [HideInInspector]
    public bool rushed = false;
    public void SugarStack(int amount)
    {
        if (rushed)
        {
            sugarCount += amount;
        }
        else
        {
            rushed = true;
            sugarCount += amount;
            StartCoroutine(SugarRush(3));
        }
    }
    
    public IEnumerator SugarRush(float time)
    {
        //things to happen before delay
        float baseSpeed = characterData.Speed;
        float baseAttackDuration = characterData.Behavior.attackDuration;
        float baseCoolDown = characterData.Behavior.attackCooldown;
        characterData.Speed = baseSpeed * 2;
        characterData.Behavior.attackCooldown = baseCoolDown / 2;
        characterData.Behavior.attackDuration = baseAttackDuration / 2;
        while (sugarCount > 0)
        {
            yield return new WaitForSeconds(time);
            sugarCount--;
        }
        //things to happen after delay
        characterData.Speed = baseSpeed;
        characterData.Behavior.attackCooldown = baseCoolDown;
        characterData.Behavior.attackDuration = baseAttackDuration;
        rushed = false;
        yield return null;
    }
    #endregion
    #region CURRY

    public virtual void CurryBalls (bool favorite)
    {
        // the amount of time that a fruitant is charmed
        int shots = 2;
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
                Vector2 dir = Vector2.ClampMagnitude(pv.Ang2Vec((split * i) + (s * 30)), 1f); //+ Random.Range(-split/4, split/4)

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
}
