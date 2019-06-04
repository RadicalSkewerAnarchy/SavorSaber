using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPlantFlavorInput : PlantFlavorInput
{
    private Animator poisonAnimator;
    private AudioSource deathAudioPlayer;
    // Start is called before the first frame update
    void Start()
    {
        InitializeDictionary();
        spriteRenderer = GetComponent<SpriteRenderer>();
        poisonAnimator = GetComponent<Animator>();
        deathAudioPlayer = GetComponent<AudioSource>();
    }

    public override void RespondToIngredients(bool fedByPlayer)
    {
        //handle spicy
        if (flavorCountDictionary[RecipeData.Flavors.Spicy] > 0)
        {
            isFed = true;
            OpenPlant();
        }
        // reset so you have to feed it both flavors at once
        // nevermind
        ResetDictionary();
    }

    private void Update()
    {
    }

    public override void ClosePlant()
    {

    }
    public override void OpenPlant()
    {
        CircleCollider2D poison = GetComponentInChildren<CircleCollider2D>();
        poison.enabled = false;
        ParticleSystem poisonParticles = GetComponent<ParticleSystem>();
        poisonAnimator.Play("Dead");
        poisonParticles.Stop();
        deathAudioPlayer.Play();
        //spriteRenderer.color = new Color(0.6f, 0.6f, 0.6f, 0.5f);
    }
}