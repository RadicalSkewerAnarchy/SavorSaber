using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Inventory))]
public class AttackRangedThrowSkewer : AttackRanged
{
    public int chargeLevels = 3;
    public float chargeTime = 1;
    public AudioClip[] chargeSounds = new AudioClip[3];
    [HideInInspector]
    public int currLevel = 0;
    private float normalInterval;
    private Inventory inv;
    private PlaySFX sfxPlayer;
    //SpriteRenderer r;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        dependecies = GetComponents<AttackBase>();
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<EntityController>();
        normalInterval = (1 / (float)chargeLevels) - 0.001f;
        inv = GetComponent<Inventory>();
        r = GetComponent<SpriteRenderer>();
        sfxPlayer = GetComponent<PlaySFX>();


    }

    private void Awake()
    {
        LoadAssetBundles();
        //defaultAttackSound = sfx_bundle.LoadAsset<AudioClip>(name);
    }

    // Update is called once per frame
    void Update()
    {
        //conditions to throw: Must either have ingredients OR a cooked recipe
        if (InputManager.GetButtonDown(control) && (!inv.ActiveSkewerEmpty() || inv.ActiveSkewerCooked()))
        {
            chargedAttack = true;
            center = r.bounds.center;

            //Get the first attack from dependecies that is attacking, else null
            AttackBase activeAttack = dependecies.FirstOrDefault((at) => at.Attacking);
            if (activeAttack == null)
                StartCoroutine(Charge());            
            else if (activeAttack.CanBeCanceled)
            {
                activeAttack.Cancel();
                StartCoroutine(Charge());
            }
        }
        if (InputManager.GetButtonUp(control) && Attacking)
        {
            StopAllCoroutines();
            effectRecipeData = inv.GetActiveEffect();
            flavorCountDictionary = new Dictionary<RecipeData.Flavors, int>(inv.GetActiveFlavorDictionary());
            ingredientCountDictionary = new Dictionary<string, int>(inv.GetActiveIngredientDictionary());
            ingredientArray = inv.GetActiveSkewer().ToArray();

            r.color = Color.white;
            currLevel = 0;
            Attack();
            inv.ClearActiveRecipe();
            inv.ClearActiveSkewer();
            inv.CanSwap = true;
            Attacking = false;
            chargedAttack = false;
        }
    }

    private IEnumerator Charge()
    {
        Attacking = true;
        CanBeCanceled = false;
        inv.CanSwap = false;
        //inv.ClearActiveSkewer();
        //inv.ClearActiveRecipe();
        for (currLevel = 0; currLevel < chargeLevels - 1; ++currLevel)
        {
            //Debug.Log("Charge Level Equals: " + currLevel);
            animator.Play(attackName + "Charge", 0, normalInterval * (currLevel + 1));
            sfxPlayer.Play(chargeSounds[currLevel]);
            float time = 0;
            while (time < chargeTime)
            {
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
        }
        sfxPlayer.Play(chargeSounds[currLevel]);
        animator.Play(attackName + "Charge", 0, normalInterval * (currLevel + 1));
        // PLACEHOLDER EFFECT
        float colorInc = 0.05f;
        while (Attacking)
        {
            yield return new WaitForEndOfFrame();
            r.color = new Color(r.color.r, r.color.g + colorInc >= 1 ? 0 : r.color.g + colorInc, r.color.b + colorInc >= 1 ? 0 : r.color.b + colorInc);
        }
    }
}
