using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class MarshPlantFlavorInput : PlantFlavorInput
{
    CapsuleCollider2D boxCollider;
    public Sprite openSprite;
    public Sprite closedSprite;

    public AudioClip closeSFX;
    public AudioClip openSFX;
    private AudioSource sfxPlayer;
    private Animator marshAnimator;

    // Start is called before the first frame update
    void Start()
    {
        InitializeDictionary();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterData = GetComponent<AIData>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponentInChildren<CapsuleCollider2D>();
        sfxPlayer = GetComponent<AudioSource>();
        marshAnimator = GetComponent<Animator>();
        //recieverCollider = GetComponent<CircleCollider2D>();
    }

    public override void RespondToIngredients(bool fedByPlayer)
    {
        //handle spicy
        if (flavorCountDictionary[RecipeData.Flavors.Spicy] > 0)
        {
            isFed = true;
            OpenPlant();
        }
    }

    private void Update()
    {
        //ClosePlant();
        //OpenPlant();
    }

    // prevent player
    public override void ClosePlant()
    {
        //spriteRenderer.sprite = closedSprite;
        marshAnimator.Play("Close");
        sfxPlayer.clip = closeSFX;
        sfxPlayer.Play();
        if (!isFed)
        {
            boxCollider.enabled = true;
        }
    }

    // allow player through
    public override void OpenPlant()
    {
        //spriteRenderer.sprite = openSprite;
        marshAnimator.Play("Open");
        sfxPlayer.clip = openSFX;
        sfxPlayer.Play();
        if (isFed)
        {
            boxCollider.enabled = false;
            spriteRenderer.color = new Color(0.6f, 0.6f, 0.6f, 0.5f);
        }
    }
}