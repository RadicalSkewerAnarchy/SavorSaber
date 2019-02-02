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
        Debug.Log("Spawn position: " + spawnPosition);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(directionVector * projectileSpeed * Time.deltaTime, Space.World);

        if (Vector2.Distance(transform.position, spawnPosition) >= range && range > 0)
        {
            Debug.Log("Exceeded max range, destroying projectile");
            //Debug.Log("Start position: " + spawnPosition);
            //Debug.Log("End position: " + transform.position);

            Destroy(this.gameObject);

        }
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
