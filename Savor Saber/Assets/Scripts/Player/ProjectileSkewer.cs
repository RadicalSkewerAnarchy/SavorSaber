using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkewer : BaseProjectile
{

    public RecipeData cookedRecipeData;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "AffectedBySkewer")
        {
            cookedRecipeData.ApplyEffectToTarget(collision.gameObject);
        }

    }
}
