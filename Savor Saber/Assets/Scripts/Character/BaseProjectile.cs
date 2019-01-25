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

    protected CapsuleCollider2D projectileCollider;

    // Start is called before the first frame update
    void Start()
    {
        projectileCollider = GetComponent<CapsuleCollider2D>();
        projectileCollider.size = new Vector2(projectileLength, projectileWidth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Projectile trigger entered");
    }
}
