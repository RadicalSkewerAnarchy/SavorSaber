using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class MarshPlantFlavorInput : PlantFlavorInput
{
    CapsuleCollider2D boxCollider;
    CircleCollider2D myCollider;
    public Sprite openSprite;
    public Sprite closedSprite;

    public AudioClip closeSFX;
    public AudioClip openSFX;
    private AudioSource sfxPlayerPl;
    private Animator marshAnimator;

    // Start is called before the first frame update
    void Start()
    {
        InitializeDictionary();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterData = GetComponent<AIData>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<CircleCollider2D>();
        boxCollider = GetComponentInChildren<CapsuleCollider2D>();
        sfxPlayerPl = GetComponent<AudioSource>();
        marshAnimator = GetComponent<Animator>();

        OpenPlant();
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
        sfxPlayerPl.clip = closeSFX;
        sfxPlayerPl.Play();
        boxCollider.enabled = false;
        isOpen = false;
    }

    // allow player through
    public override void OpenPlant()
    {
        //spriteRenderer.sprite = openSprite;
        marshAnimator.Play("Open");
        sfxPlayerPl.clip = openSFX;
        sfxPlayerPl.Play();
        boxCollider.enabled = true;
        isOpen = true;
    }
}