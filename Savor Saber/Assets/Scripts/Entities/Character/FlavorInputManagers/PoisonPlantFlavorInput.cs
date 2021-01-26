﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPlantFlavorInput : PlantFlavorInput
{
    private Animator poisonAnimator;
    private AudioSource deathAudioPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        poisonAnimator = GetComponent<Animator>();
        deathAudioPlayer = GetComponent<AudioSource>();
    }


    public override void ClosePlant()
    {
        isOpen = false;
        CircleCollider2D poison = GetComponentInChildren<CircleCollider2D>();
        poison.enabled = true;
        CapsuleCollider2D baseCollider = GetComponentInChildren<CapsuleCollider2D>();
        baseCollider.enabled = true;
        ParticleSystem poisonParticles = GetComponent<ParticleSystem>();
        poisonAnimator.Play("Gas");
        poisonParticles.Play();
    }

    public override void OpenPlant()
    {
        isOpen = true;
        CircleCollider2D poison = GetComponentInChildren<CircleCollider2D>();
        poison.enabled = false;
        CapsuleCollider2D baseCollider = GetComponentInChildren<CapsuleCollider2D>();
        baseCollider.enabled = false;
        ParticleSystem poisonParticles = GetComponent<ParticleSystem>();
        poisonAnimator.Play("Dead");
        poisonParticles.Stop();
        deathAudioPlayer.Play();
        //spriteRenderer.color = new Color(0.6f, 0.6f, 0.6f, 0.5f);
    }
}