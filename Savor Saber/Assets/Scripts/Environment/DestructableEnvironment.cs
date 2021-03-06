﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// on interact with a sword
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DestructableEnvironment : MonoBehaviour
{
    public bool allowRegrow = true;
    public int health;
    private int healthReset;
    [Range(0, 100)]
    public int dropChance = 100;
    public GameObject dropOnDestroy;
    public Sprite normalSprite;
    public Sprite destroyedSprite;
    public float respawnTime;
    public bool destroyed = false;
    public ParticleSystem particles = null;
    public float wiggleTime = 2f;
    public float wiggleSpeed = 0.2f;
    [Range(0, 0.3f)]
    public float wiggleAmplitude = 0.05f;

    public bool skewerable = false;
    public bool rooted = true;
    public bool staySolid = false;

    private Vector2 origin;

    private SpriteRenderer spr;
    private AudioSource src;
    private Animator anim;
    public bool animateWhileAlive = true;

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        spr.sprite = normalSprite;
        src = GetComponent<AudioSource>();
        healthReset = health;
        origin = this.transform.position;
        anim = GetComponent<Animator>();
        if (anim != null)
            anim.enabled = (animateWhileAlive ? true : false);
    }

    public void Destroy()
    {
        if (destroyed)
            return;

        if (particles != null)
            particles.Play();
        if (src != null)
            src.Play();

        StartCoroutine(Wiggle(wiggleTime, wiggleSpeed, wiggleAmplitude));

        if (health > 0)
            return;
        destroyed = true;

        if (anim != null)
            anim.enabled = !animateWhileAlive;

        spr.sprite = destroyedSprite;
        float thresh = (float)dropChance / 100;
        if (dropOnDestroy != null && Random.value <= thresh)
            Instantiate(dropOnDestroy, transform.position, Quaternion.identity);

        if (allowRegrow)
        {
            StartCoroutine(Regrow());
        }

        this.GetComponent<Collider2D>().enabled = staySolid;
    }
    private IEnumerator Regrow()
    {
        yield return new WaitForSeconds(respawnTime);
        this.GetComponent<Collider2D>().enabled = !staySolid;
        spr.sprite = normalSprite;
        if(anim != null)
            anim.enabled = animateWhileAlive;
        health = healthReset;
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
                this.transform.position = origin + new Vector2(amplitude * Mathf.Sin(speedCount), 0);
            else
                this.transform.position = this.transform.position + new Vector3(amplitude * Mathf.Sin(speedCount), 0);

            speedCount += 15000 * Time.deltaTime;
            tick -= speed;
        }
        if (rooted)
            this.transform.position = origin;
        yield return null;
    }
}
