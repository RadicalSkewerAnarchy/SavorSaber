using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (flavorCountDictionary[RecipeData.Flavors.Sweet] > 0)
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

            Destroy(this.gameObject);

        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //attack radius is set by the amount of Savory/Umami on the skewer
        attackRadius = 2 * flavorCountDictionary[RecipeData.Flavors.Savory] + 0.5f;

        //general hitting 
        if (collision.gameObject.tag != "Player" && flavorCountDictionary != null)
        { 

            //call the target's feeding function, if it has one
            FlavorInputManager flavorInput = collision.gameObject.GetComponent<FlavorInputManager>();
            if (flavorInput != null)
            {
                for (int f = 1; f <= 64; f = f << 1)
                {
                    //feed all flavors into the target's flavor input manager
                    //and add them to the signal's dictionary of moods
                    if (flavorCountDictionary[(RecipeData.Flavors)f] > 0)
                    {
                        flavorInput.Feed((RecipeData.Flavors)f, flavorCountDictionary[(RecipeData.Flavors)f]);

                        //if sweet, also instantiate AI signal
                        if(f == (int)RecipeData.Flavors.Sweet)
                        {
                            signal = Instantiate(dropItem, transform.position, Quaternion.identity);
                            signalApplication = signal.GetComponent<SignalApplication>();
                            moodMod.Add("Friendliness", flavorCountDictionary[(RecipeData.Flavors)f] / 3);
                            signalApplication.SetSignalParameters(null, attackRadius, moodMod, true, true);
                            
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
}
