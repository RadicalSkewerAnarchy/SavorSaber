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
            //Debug.Log("Skewer collided with " + collision.gameObject);

            if (ingredientArray != null)
            {
                //check to see if the thing we hit has a FlavorInputManager
                FlavorInputManager flavorInput = collision.gameObject.GetComponent<FlavorInputManager>();
                if (flavorInput != null)
                {
                    //Debug.Log("Flavor input of " + collision.gameObject + " not null");
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
                    if (spawnBonusEffectOnMiss && bonusEffectTemplate != null)
                    {
                        GameObject bonus = Instantiate(bonusEffectTemplate, transform.position, Quaternion.identity);
                        SkewerBonusEffect effect = bonus.GetComponent<SkewerBonusEffect>();
                        if (effect != null)
                            effect.SetTarget(collision.gameObject, bonusEffectMagnitude);
                        dropping = true;
                    }

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

}
