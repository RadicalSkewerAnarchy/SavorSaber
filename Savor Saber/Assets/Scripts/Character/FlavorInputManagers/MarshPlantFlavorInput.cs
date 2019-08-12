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

    public GameObject swallowed;
    public bool canSwallow = true;

    public AudioClip closeSFX;
    public AudioClip openSFX;
    private AudioSource sfxPlayerPl;
    private Animator marshAnimator;

    // Start is called before the first frame update
    private void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        characterData = GetComponent<AIData>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<CircleCollider2D>();
        boxCollider = GetComponentInChildren<CapsuleCollider2D>();
        sfxPlayerPl = GetComponent<AudioSource>();
        marshAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        InitializeDictionary();

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
    }

    // prevent player
    public override void ClosePlant()
    {
        //spriteRenderer.sprite = closedSprite;
        marshAnimator.Play("Close");
        marshAnimator.Update(0);
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
        marshAnimator.Update(0);
        sfxPlayerPl.clip = openSFX;
        sfxPlayerPl.Play();
        boxCollider.enabled = true;
        isOpen = true;
    }

    public IEnumerator Pollinating(float time)
    {
        ClosePlant();
        canSwallow = false;
        swallowed.SetActive(false);

        yield return new WaitForSeconds(time);

        Debug.Log(this.name + " Spitting Out --> " + swallowed.name);

        swallowed.SetActive(true);
        FlavorInputManager fim = swallowed.GetComponent<FlavorInputManager>();
        fim.SpawnSingle();
        swallowed = null;
        OpenPlant();

        StartCoroutine(ReSwallow(3));

        yield return null;
    }

    public IEnumerator ReSwallow(float time)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.red;

        yield return new WaitForSeconds(time);
        
        sr.color = Color.white;
        canSwallow = true;

        yield return null;
    }
}