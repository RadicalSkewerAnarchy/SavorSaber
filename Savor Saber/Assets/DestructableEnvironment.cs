using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// on interact with a sword
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DestructableEnvironment : MonoBehaviour
{
    public int health;
    [Range(0, 100)]
    public int dropChance = 100;
    public GameObject dropOnDestroy;
    public Sprite normalSprite;
    public Sprite destroyedSprite;
    public float respawnTime;
    public bool destroyed = false;
    public ParticleSystem particles = null;

    private SpriteRenderer spr;

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        spr.sprite = normalSprite;
    }

    public void Destroy()
    {
        if (destroyed)
            return;
        destroyed = true;
        spr.sprite = destroyedSprite;
        float thresh = (float)dropChance / 100;
        if(particles != null)
            particles.Play();
        if(Random.value <= thresh)
            Instantiate(dropOnDestroy, transform.position, Quaternion.identity);
        StartCoroutine(Regrow());
    }
    private IEnumerator Regrow()
    {
        yield return new WaitForSeconds(respawnTime);
        spr.sprite = normalSprite;
        destroyed = false;
    }
}
