using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attatch this component to an object to make it a monster
/// </summary>
[RequireComponent((typeof(Collider2D)), typeof(Rigidbody2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource), typeof(Health), typeof(DropOnDeath))]
public class Monster : MonoBehaviour
{
    /// <summary>name of the monster in-game</summary>
    public string displayName;

    public AudioClip deathSfx;
    public GameObject deathSfxPlayer;
    
    private void Start()
    {
        var health = GetComponent<Health>();
        // Drop items and die if health is 0 or lower        
        health.AddEvent((hp) => { if (hp <= 0) Kill(); });
    }

    public void Kill()
    {
        var deathSoundObj = Instantiate(deathSfxPlayer, transform.position, transform.rotation);
        deathSoundObj.GetComponent<PlayAndDestroy>().Play(deathSfx);
        GetComponent<DropOnDeath>().Drop();
        Destroy(gameObject);
    }
}
