using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileSkewer : BaseProjectile
{

    //SignalApplication signalApplication;
    //GameObject signal;
    Dictionary<string, float> moodMod = new Dictionary<string, float>();
    bool detonating = false;
    public GameObject audioPlayer;
    public AudioClip sweetSFX;
    public AudioClip spicySFX;
    public bool fed = false;


    // Start is called before the first frame update
    void Start()
    {
        projectileCollider = GetComponent<CapsuleCollider2D>();
        projectileCollider.size = new Vector2(projectileLength, projectileWidth);

        // set projectile velocity vector
        SetGeometry();
        spawnPosition = transform.position;

        //penetrateTargets = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(directionVector * projectileSpeed * Time.deltaTime, Space.World);

        if (Vector2.Distance(transform.position, spawnPosition) >= range && range > 0)
        {
            //should be obsolete since we're setting the moods of affected creatures directly now
            /*
            if (flavorCountDictionary != null && flavorCountDictionary[RecipeData.Flavors.Sweet] > 0)
            {
                //attack radius is set by the amount of Savory/Umami on the skewer
                attackRadius = 2 * flavorCountDictionary[RecipeData.Flavors.Savory] + 0.5f;

                signal = Instantiate(dropItem, transform.position, Quaternion.identity);
                signalApplication = signal.GetComponent<SignalApplication>();
                moodMod.Add("Friendliness", flavorCountDictionary[RecipeData.Flavors.Sweet] / 3);
                moodMod.Add("Fear", flavorCountDictionary[RecipeData.Flavors.Sweet] / -3);
                moodMod.Add("Hostility", flavorCountDictionary[RecipeData.Flavors.Sweet] / -3);
                signalApplication.SetSignalParameters(null, attackRadius, moodMod, true, true);
                
            }
            */
            //SetAOE();

            Destroy(this.gameObject);

        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!fed)
        { 
            if (collision.tag == "ThrowThrough" || collision.tag == "SkewerableObject")
                return;
            Debug.Log("Skewer collided with " + collision.gameObject);
            //attack radius is set by the amount of Savory/Umami on the skewer

            if (ingredientArray != null)
            {
                FlavorInputManager flavorInput = collision.gameObject.GetComponent<FlavorInputManager>();
                if (flavorInput != null)
                {
                    if (flavorCountDictionary[RecipeData.Flavors.Savory] > 0 && !detonating)
                    {
                        detonating = true;
                        SetAOE();
                    }
                    else
                    {
                        // FEEEED MEEEE
                        flavorInput.Feed(ingredientArray);
                        fed = true;
                        Destroy(this.gameObject);
                        //=================================
                        bool fedFavorite = flavorInput.FedFavorite();
                        if (!fedFavorite && GetMajorityFlavor() == RecipeData.Flavors.Sweet)
                        {
                            GameObject sfx = Instantiate(audioPlayer, transform.position, Quaternion.identity);
                            sfx.GetComponent<PlayAndDestroy>().Play(sweetSFX);
                        }
                        else if (!fedFavorite && GetMajorityFlavor() == RecipeData.Flavors.Spicy)
                        {
                            GameObject sfx = Instantiate(audioPlayer, transform.position, Quaternion.identity);
                            sfx.GetComponent<AudioSource>().volume = 0.5f;
                            sfx.GetComponent<PlayAndDestroy>().Play(spicySFX);
                        }
                    }

                }
            }
            if (!penetrateTargets)
                Destroy(this.gameObject);
        }
    }

    //save space in earlier checks
    private bool IsCollisionMonster(Collider2D collision)
    {
        return collision.gameObject.tag == "Prey" || collision.gameObject.tag == "Predator";
    }

    private void SetAOE()
    {
        CircleCollider2D AOECircle = GetComponentInChildren<CircleCollider2D>();
        ProjectileSkewerAOE AOEData = GetComponentInChildren<ProjectileSkewerAOE>();
        AudioSource boomSFX = GetComponentInChildren<AudioSource>();

        if (boomSFX != null)
            boomSFX.Play();

        if (flavorCountDictionary[RecipeData.Flavors.Savory] > 0)
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
        AOECircle.radius = flavorCountDictionary[RecipeData.Flavors.Savory] + 0.5f;

        transform.GetChild(1).transform.parent = transform.GetChild(0).transform;
        transform.GetChild(0).transform.parent = null;

    }

    private void ExplodeEffects()
    {
        Animator AOEAnimator = GetComponentInChildren<Animator>();
        ParticleSystem AOEParticles = GetComponentInChildren<ParticleSystem>();
        if (GetMajorityFlavor(RecipeData.Flavors.Savory) == RecipeData.Flavors.Sweet)
            AOEAnimator.SetBool("ExplodeSweet", true);
        else if (GetMajorityFlavor(RecipeData.Flavors.Savory) == RecipeData.Flavors.Spicy)
            AOEAnimator.SetBool("ExplodeSpicy", true);
        else
            AOEAnimator.SetBool("ExplodeSavory", true);
        AOEParticles.Play();
    }

    private RecipeData.Flavors GetMajorityFlavor(RecipeData.Flavors ignore)
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

    private RecipeData.Flavors GetMajorityFlavor()
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
}
