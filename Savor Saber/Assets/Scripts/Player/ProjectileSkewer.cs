﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileSkewer : BaseProjectile
{

    SignalApplication signalApplication;
    GameObject signal;
    Dictionary<string, float> moodMod = new Dictionary<string, float>();

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
        Debug.Log("Skewer collided with " + collision.gameObject);
        //attack radius is set by the amount of Savory/Umami on the skewer

        //feed via explosion if it has Umami
        if(flavorCountDictionary[RecipeData.Flavors.Savory] > 0)
        {
            SetAOE();
        }
        else
        {
            if (ingredientArray != null)
            {
                FlavorInputManager flavorInput = collision.gameObject.GetComponent<FlavorInputManager>();
                if (flavorInput != null)
                {
                    flavorInput.Feed(ingredientArray);
                }
            }
        }


        
        if (!penetrateTargets)
            Destroy(this.gameObject);

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
        AOEAnimator.SetBool("Explode", true);
        AOEParticles.Play();
    }
}
