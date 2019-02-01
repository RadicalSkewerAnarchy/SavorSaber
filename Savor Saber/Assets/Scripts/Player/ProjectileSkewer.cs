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
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(directionVector * projectileSpeed * Time.deltaTime, Space.World);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Monster" && effectRecipeData != null)
        {
            Debug.Log("Thrown skewer hit target with effect " + effectRecipeData.displayName);
            effectRecipeData.ApplyEffectToTarget(collision.gameObject);
            if (!penetrateTargets)
                Destroy(this.gameObject);
        }



    }
}
