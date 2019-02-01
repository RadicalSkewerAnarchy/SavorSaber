using System.Collections;
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
    /// Should this projectile penetrate targets?
    /// </summary>
    public bool penetrateTargets = false;

    [System.NonSerialized]
    public float projectileRotation;

    /// <summary>
    /// Recipe data for any effects to be applied. Projectile gets this from
    /// the attack script. 
    /// </summary>
    [System.NonSerialized]
    public RecipeData effectRecipeData = null;

    /// <summary>
    /// Direction and rotation fields
    /// </summary>
    [System.NonSerialized]
    public Direction direction;
    [System.NonSerialized]
    public Vector2 directionVector;
    [System.NonSerialized]
    public CapsuleCollider2D projectileCollider;

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

    protected void SetGeometry()
    {
        if (direction == Direction.East)
        {
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
        else if (direction == Direction.West)
        {
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
        else if (direction == Direction.North)
        {
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
        else if (direction == Direction.South)
        {
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
        else if (direction == Direction.NorthWest)
        {
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
        else if (direction == Direction.NorthEast)
        {
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
        else if (direction == Direction.SouthWest)
        {
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
        else if (direction == Direction.SouthEast)
        {
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Projectile trigger entered");

        if (!penetrateTargets)
            Destroy(this.gameObject);

    }
}
