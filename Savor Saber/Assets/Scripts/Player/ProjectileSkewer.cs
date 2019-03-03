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

            Destroy(this.gameObject);

        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //attack radius is set by the amount of Savory/Umami on the skewer
        if(flavorCountDictionary != null)
        {
            attackRadius = 2 * flavorCountDictionary[RecipeData.Flavors.Savory] + 0.5f;
        }
        else
        {
            attackRadius = 2.5f;
        }

        if (ingredientArray != null)
        {
            FlavorInputManager flavorInput = collision.gameObject.GetComponent<FlavorInputManager>();
            if (flavorInput != null)
            {
                flavorInput.Feed(ingredientArray);
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
}
