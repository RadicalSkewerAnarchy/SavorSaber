using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Base class for melee attacks.
/// </summary>
[RequireComponent(typeof(CapsuleCollider2D))]
public abstract class MeleeAttack : MonoBehaviour
{

    /// <summary>
    /// The collider for the melee attack.
    /// </summary>
    protected CapsuleCollider2D meleeCollider;

    /// <summary>
    /// How much damage the attack does. Currently a float in case we want.
    /// finer damage values?
    /// </summary>
    public float meleeDamage = 1f;

    /// <summary>
    /// Number of units in front of the character this attack can reach.
    /// </summary>
    public float meleeRange = 1f;

    /// <summary>
    /// How many units wide this attack is.
    /// </summary>
    public float meleeWidth = 1f;

    /// <summary>
    /// How many seconds the collider will remain active.
    /// </summary>
    public float attackDuration = 0.5f;

    /// <summary>
    /// To prevent attack action while still attacking.
    /// </summary>
    protected bool endSignalSent = false;
    protected bool attacking = false;

    public abstract void Attack();
}
