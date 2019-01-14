using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attatch this component to an object to make it a monster
/// Monsters are required to have a Rigidboy2D and a DropOnDeath component
/// Monsters should probably have a collider
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(DropOnDeath), typeof(SpriteRenderer))]
public class Monster : MonoBehaviour
{
    public string displayName;
    // put anything else necessary for all monsters here

    // Stats/health should be added in as a component (or added as an interface) DropOnDeath.Drop should be added
    // as a trigger to stats on health <= 0
}
