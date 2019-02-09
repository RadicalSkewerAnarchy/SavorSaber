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
        Debug.Log("thrown skewer collided with " + collision.name);
        if (collision.gameObject.tag == "Monster" && effectRecipeData != null)
        {
            effectRecipeData.ApplyEffectToTarget(collision.gameObject);           
        }
        if(collision.gameObject.tag != "Player")
        {
            if (dropItem != null)
                Instantiate(dropItem, transform.position, Quaternion.identity);

            if (!penetrateTargets)
                Destroy(this.gameObject);
        }

    }
}
