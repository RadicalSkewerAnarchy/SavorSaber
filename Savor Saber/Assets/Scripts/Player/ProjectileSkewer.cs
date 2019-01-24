using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkewer : Projectile
{

    public RecipeData cookedRecipeData;

    // Start is called before the first frame update
    void Start()
    {
        //set data values
        projectileDamage = 0;
        projectileLength = 2;
        projectileWidth = 1;

        //set the dimensions of the hitbox to match the projectile's width and height
        projectileCollider = GetComponent<CapsuleCollider2D>();
        projectileCollider.direction = CapsuleDirection2D.Horizontal;
        projectileCollider.size = new Vector2(projectileLength, projectileWidth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "AffectedBySkewer")
        {
            cookedRecipeData.ApplyEffectToTarget(collision.gameObject);
        }

    }
}
