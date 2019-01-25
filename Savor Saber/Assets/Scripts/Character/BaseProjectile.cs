﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Use to designate a GameObject as a projectile launched by a RangedAttack script
/// Should be used as a base class, with derived classes for specific entities' attacks
/// </summary>
[RequireComponent(typeof(CapsuleCollider2D))]
public class BaseProjectile : MonoBehaviour
{
    /// <summary>
    /// How fast the projectile travels
    /// </summary>
    public float projectileSpeed;

    /// <summary>
    /// How long the projectile is
    /// </summary>
    public float projectileLength;

    /// <summary>
    /// How wide the projectile is
    /// </summary>
    public float projectileWidth;

    /// <summary>
    /// How much damage the projectile does, if any
    /// </summary>
    public float projectileDamage;

    /// <summary>
    /// How much the projectile should knock back its target
    /// </summary>
    public float projectileKnockback;

    /// <summary>
    /// Direction and rotation fields
    /// </summary>
    [System.NonSerialized]
    public Direction direction;
    private Vector2 directionVector;
    private float projectileRotation;

    protected CapsuleCollider2D projectileCollider;

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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Projectile trigger entered");
    }
}
