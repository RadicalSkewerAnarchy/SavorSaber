using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// on interact with a sword
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DestructableEnvironment : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;
    private int health;
    public int Health
    {
        get => health;
        set
        {
            // Play Damage Effects if our health is being lowered
            if (value < health)
            {
                if (particles != null)
                    particles.Play();
                if (sfx != null)
                    sfx.Play();
                StartCoroutine(Wiggle(wiggleTime, wiggleSpeed, wiggleAmplitude));
            }           
            // Destroy if health is <= 0
            if (value <= 0)
            {
                health = 0;
                Destroy();
                return;
            }
            // else set health to value
            health = value;
        }
    }
    [HideInInspector]
    public bool destroyed = false;
    public GameObject dropOnDestroy;
    [Range(0, 100)]
    public int dropChance = 100;
    public Sprite destroyedSprite;
    public bool allowRegrow = true;
    // The time it takes for a destructible object to regrow (if regrowth is allowed)
    public float respawnTime;
    public bool skewerable = false;
    public bool rooted = true;
    // Does the object remain solid when destroyed?
    public bool staySolid = false;
    [Header("Visual Customization")]
    public ParticleSystem particles = null;
    // Wiggle Parameters
    public float wiggleTime = 2f;
    public float wiggleSpeed = 0.2f;
    [Range(0, 0.3f)]
    public float wiggleAmplitude = 0.05f;


    private Vector2 origin;
    private SpriteRenderer spr;
    private Sprite normalSprite;
    private AudioSource sfx;
    private Animator anim;
    new private Collider2D collider;
    

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        normalSprite = spr.sprite;
        sfx = GetComponent<AudioSource>();
        health = maxHealth;
        origin = transform.position;
        anim = GetComponent<Animator>();
        if (anim != null)
            anim.enabled = true;
        collider = GetComponent<Collider2D>();
    }

    public void DestroyAndRegrow(float time, bool spawn = true)
    {
        bool temp = allowRegrow;
        allowRegrow = false;
        Destroy(spawn);
        StartCoroutine(Regrow(time));
        allowRegrow = temp;
    }

    public void Destroy(bool spawn = true)
    {
        if (destroyed)
            return;
        destroyed = true;

        if (anim != null)
            anim.enabled = false;

        spr.sprite = destroyedSprite;

        if(spawn)
        {
            float thresh = (float)dropChance / 100;
            if (dropOnDestroy != null && Random.value <= thresh)
                Instantiate(dropOnDestroy, transform.position, Quaternion.identity);
        }

        if (allowRegrow)
        {
            StartCoroutine(Regrow(respawnTime));
        }

        collider.enabled = staySolid;
    }

    private IEnumerator Regrow(float time)
    {
        yield return new WaitForSeconds(time);
        collider.enabled = !staySolid;
        spr.sprite = normalSprite;
        if(anim != null)
            anim.enabled = true;
        health = maxHealth;
        destroyed = false;
    }

    private IEnumerator Wiggle(float time, float speed, float amplitude)
    {
        var speedCount = 0f;
        var tick = time;
        while (tick > 0)
        {
            //Debug.Log("wiggling");
            yield return new WaitForSeconds(Time.deltaTime);

            if (rooted)
                transform.position = origin + new Vector2(amplitude * Mathf.Sin(speedCount), 0);
            else
                transform.position = transform.position + new Vector3(amplitude * Mathf.Sin(speedCount), 0);

            speedCount += 15000 * Time.deltaTime;
            tick -= speed;
        }
        if (rooted)
            transform.position = origin;
        yield return null;
    }
}
