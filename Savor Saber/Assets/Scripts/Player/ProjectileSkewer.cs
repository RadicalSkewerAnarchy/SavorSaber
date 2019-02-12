using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkewer : BaseProjectile
{

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
            if (dropItem != null)
                Instantiate(dropItem, transform.position, Quaternion.identity);

            Destroy(this.gameObject);

        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //if it has an effect recipe (i.e. cooked/crafted item)
        if (collision.gameObject.tag == "Monster" && effectRecipeData != null)
        {
            effectRecipeData.ApplyEffectToTarget(collision.gameObject);           
        }

        //if it has a flavor dictionary (i.e. uncooked, regular attack)
        if (collision.gameObject.tag == "Monster" && flavorCountDictionary != null)
        {
            //call the target's feeding function
            FlavorInputManager flavorInput = collision.gameObject.GetComponent<FlavorInputManager>();
            if(flavorInput != null)
            {
                for (int f = 1; f <= 64; f = f << 1)
                {
                    if(flavorCountDictionary[(RecipeData.Flavors)f] > 0)
                    {
                        flavorInput.Feed((RecipeData.Flavors)f, flavorCountDictionary[(RecipeData.Flavors)f]);
                    }
                }
            }
        }

        //general hitting 
        if (collision.gameObject.tag != "Player")
        {
            if (dropItem != null)
                Instantiate(dropItem, transform.position, Quaternion.identity);

            if (!penetrateTargets)
                Destroy(this.gameObject);
        }

    }
}
