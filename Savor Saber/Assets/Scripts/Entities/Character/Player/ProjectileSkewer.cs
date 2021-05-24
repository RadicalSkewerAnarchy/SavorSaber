using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileSkewer : BaseProjectile
{

    //SignalApplication signalApplication;
    //GameObject signal;
    //Dictionary<string, float> moodMod = new Dictionary<string, float>();
    //bool detonating = false;
    public GameObject audioPlayer;
    [HideInInspector]
    public bool fed = false;


    // Start is called before the first frame update
    void Start()
    {
        projectileCollider = GetComponent<CapsuleCollider2D>();
        projectileCollider.size = new Vector2(projectileLength, projectileWidth);

        // set projectile velocity vector
        SetGeometry();
        spawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        //ignore specified target classes
        foreach (string tag in tagsToIgnore)
        {
            if (go.tag == tag)
                return;
        }

        if (!fed)
        { 
            if (collision.tag == "ThrowThrough" || collision.tag == "SkewerableObject")
                return;
            Debug.Log("Skewer collided with " + collision.gameObject);

            if (ingredientArray != null)
            {
                //check to see if the thing we hit has a FlavorInputManager
                FlavorInputManager flavorInput = collision.gameObject.GetComponent<FlavorInputManager>();
                if (flavorInput != null)
                {
                    Debug.Log("Flavor input of " + collision.gameObject + " not null");
                    flavorInput.Feed(ingredientArray[0], true, myCharData);
                    fed = true;

                    //if this is an enemy, check if we should be spawning a bonus effect
                    if(collision.gameObject.tag == "Predator" && !dropping && bonusEffectTemplate != null)
                    {
                        GameObject bonus = Instantiate(bonusEffectTemplate, transform.position, Quaternion.identity);
                        SkewerBonusEffect effect = bonus.GetComponent<SkewerBonusEffect>();
                        if (effect != null)
                            effect.SetTarget(collision.gameObject, bonusEffectMagnitude);
                        dropping = true;
                    }

                    Destroy(this.gameObject);
                }
                //if you hit something (and aren't penetrating) but can't feed it
                else if (!dropping && !penetrateTargets)
                {
                    SpawnDropsOnMiss();
                    Destroy(this.gameObject);
                }
            }
            else if (!penetrateTargets)
                Destroy(this.gameObject);
        }
    }

    private void SpawnDropsOnMiss()
    {
        SkewerableObject ingredient;
        GameObject drop;
        SpriteRenderer sr;
        for (int i = 0; i < ingredientArray.Length; i++)
        {
            if (ingredientArray[i] != null && dropTemplate != null)
            {
                drop = Instantiate(dropTemplate, transform.position, Quaternion.identity);
                ingredient = drop.GetComponent<SkewerableObject>();
                sr = drop.GetComponent<SpriteRenderer>();

                ingredient.data = ingredientArray[i];
                sr.sprite = ingredientArray[i].image;
            }

        }
        dropping = true;
    }
    // Code from when it mattered what flavor things were
    /*
    //save space in earlier checks
    private bool IsCollisionMonster(Collider2D collision)
    {
        return collision.gameObject.tag == "Prey" || collision.gameObject.tag == "Predator";
    }

    protected void SetAOE()
    {
        CircleCollider2D AOECircle = GetComponentInChildren<CircleCollider2D>();
        ProjectileSkewerAOE AOEData = GetComponentInChildren<ProjectileSkewerAOE>();
        AudioSource boomSFX = GetComponentInChildren<AudioSource>();

        if (boomSFX != null)
            boomSFX.Play();

        if (flavorCountDictionary[RecipeData.Flavors.Umami] > 0)
        {
            ExplodeEffects();
        }

        if (ingredientArray != null)
        {
            AOEData.ingredientArray = new IngredientData[ingredientArray.Length];
            Array.Copy(ingredientArray, AOEData.ingredientArray, ingredientArray.Length);
        }
        if (flavorCountDictionary != null)
        {
            AOEData.flavorCountDictionary = new Dictionary<RecipeData.Flavors, int>(flavorCountDictionary);
        }

        AOECircle.enabled = true;
        AOECircle.radius = flavorCountDictionary[RecipeData.Flavors.Umami] + 0.5f;

        transform.GetChild(1).transform.parent = transform.GetChild(0).transform;
        transform.GetChild(0).transform.parent = null;

    }

    protected void ExplodeEffects()
    {
        Animator AOEAnimator = GetComponentInChildren<Animator>();
        ParticleSystem AOEParticles = GetComponentInChildren<ParticleSystem>();
        if (GetMajorityFlavor(RecipeData.Flavors.Umami) == RecipeData.Flavors.Sweet)
            AOEAnimator.SetBool("ExplodeSweet", true);
        else if (GetMajorityFlavor(RecipeData.Flavors.Umami) == RecipeData.Flavors.Spicy)
            AOEAnimator.SetBool("ExplodeSpicy", true);
        else if (GetMajorityFlavor(RecipeData.Flavors.Umami) == RecipeData.Flavors.Sour)
            AOEAnimator.SetBool("ExplodeSour", true);
        else if (GetMajorityFlavor(RecipeData.Flavors.Umami) == RecipeData.Flavors.Salty)
            AOEAnimator.SetBool("ExplodeSalty", true);
        else
            AOEAnimator.SetBool("ExplodeSavory", true);
        AOEParticles.Play();
    }

    protected RecipeData.Flavors GetMajorityFlavor(RecipeData.Flavors ignore)
    {
        int highest = 0;
        int lastCount = 0;
        int majorityFlavor = 0;
        bool tied = false;
        for (int f = 1; f <= 64; f = f << 1)
        {
            if(f != (int)ignore && flavorCountDictionary[(RecipeData.Flavors)f] > highest)
            {
                highest = flavorCountDictionary[(RecipeData.Flavors)f];
                if (highest == lastCount)
                    tied = true;
                else
                    tied = false;
                lastCount = highest;
                majorityFlavor = f;
            }
        }
        if (tied)
            return RecipeData.Flavors.None;
        else
            return (RecipeData.Flavors)majorityFlavor;
    }

    protected RecipeData.Flavors GetMajorityFlavor()
    {
        int highest = 0;
        int lastCount = 0;
        int majorityFlavor = 0;
        bool tied = false;
        for (int f = 1; f <= 64; f = f << 1)
        {
            if (flavorCountDictionary[(RecipeData.Flavors)f] > highest)
            {
                highest = flavorCountDictionary[(RecipeData.Flavors)f];
                if (highest == lastCount)
                    tied = true;
                else
                    tied = false;
                lastCount = highest;
                majorityFlavor = f;
            }
        }
        if (tied)
            return RecipeData.Flavors.None;
        else
            return (RecipeData.Flavors)majorityFlavor;
    }
    */
}
