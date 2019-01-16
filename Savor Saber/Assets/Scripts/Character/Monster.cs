using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attatch this component to an object to make it a monster
/// Monsters are required to have a Rigidboy2D and a DropOnDeath component
/// Monsters should probably have a collider
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(DropOnDeath), typeof(SpriteRenderer))]
[RequireComponent(typeof(Health))]
public class Monster : MonoBehaviour
{
    /// <summary>name of the monster in-game</summary>
    public string displayName;
    
    private void Start()
    {
        var health = GetComponent<Health>();
        var drop = GetComponent<DropOnDeath>();
        //Drop items and die if health is 0 or lower
        Health.HealthEvent onDeath = (hp) =>
        {
            if (hp <= 0)
            {
                drop.Drop();
                Destroy(gameObject);
            }
        };
        health.AddEvent(onDeath);
    }
}
