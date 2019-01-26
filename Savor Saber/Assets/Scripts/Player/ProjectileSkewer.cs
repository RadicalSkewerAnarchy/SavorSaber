using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkewer : BaseProjectile
{

    public RecipeData cookedRecipeData;

    // Start is called before the first frame update
    void Start()
    {
        projectileCollider = GetComponent<CapsuleCollider2D>();
        projectileCollider.size = new Vector2(projectileLength, projectileWidth);

        Debug.Log("Shooting " + direction);

        // set projectile velocity vector
        if (direction == Direction.East)
        {
            directionVector = new Vector2(1, 0);
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
        else if (direction == Direction.West)
        {
            directionVector = new Vector2(-1, 0);
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
        else if (direction == Direction.North)
        {
            directionVector = new Vector2(0, 1);
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
            //projectileRotation = 90;
        }
        else if (direction == Direction.South)
        {
            directionVector = new Vector2(0, -1);
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
            //projectileRotation = -90;
        }
        else if (direction == Direction.NorthWest)
        {
            directionVector = new Vector2(-1, 1).normalized;
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
            //projectileRotation = -45;
        }
        else if (direction == Direction.NorthEast)
        {
            directionVector = new Vector2(1, 1).normalized;
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
            //projectileRotation = 45;
        }
        else if (direction == Direction.SouthWest)
        {
            directionVector = new Vector2(-1, -1).normalized;
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
            //projectileRotation = 45;
        }
        else if (direction == Direction.SouthEast)
        {
            directionVector = new Vector2(1, -1).normalized;
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
            //projectileRotation = -45;
        }

        transform.Rotate(new Vector3(0, 0, projectileRotation));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(directionVector * projectileSpeed * Time.deltaTime);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            cookedRecipeData.ApplyEffectToTarget(collision.gameObject);
        }

    }
}
