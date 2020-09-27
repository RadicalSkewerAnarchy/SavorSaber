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
    public CrosshairController crosshair;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        animator = GetComponent<Animator>();       
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<EntityController>();
        normalInterval = (1 / (float)chargeLevels) - 0.001f;
        inv = GetComponent<Inventory>();
        r = GetComponent<SpriteRenderer>();
        sfxPlayer = GetComponent<PlaySFX>();

        if (crosshair == null)
            crosshair = GameObject.Find("Crosshair").GetComponent<CrosshairController>();
    }

    private void Awake()
    {
        LoadAssetBundles();
        //defaultAttackSound = sfx_bundle.LoadAsset<AudioClip>(name);
    }

    // Update is called once per frame
    void Update()
    {
        // if the player is riding, attacking just feeds the mount
        if (((PlayerController)controller).riding)
        { 
            if (InputManager.GetButtonDown(control, axis) && !inv.ActiveSkewerEmpty())
            {
                // feed the mount
                StopAllCoroutines();
                effectRecipeData = inv.GetActiveEffect();
                flavorCountDictionary = new Dictionary<RecipeData.Flavors, int>(inv.GetActiveFlavorDictionary());
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
            return;
        }

        //animation cancel
        if (Attacking && (InputManager.GetButtonDown(Control.Knife, InputAxis.Slash) || InputManager.GetButtonDown(Control.Skewer, InputAxis.Skewer)))
        {
            StopAllCoroutines();
            r.color = Color.white;
            currLevel = 0;
            inv.CanSwap = true;
            chargedAttack = false;
            Attacking = false;
            CanBeCanceled = false;
            return;
        }
        
        //conditions to throw: Must have ingredients
        if (!Attacking && InputManager.GetButtonDown(control, axis) && (!inv.ActiveSkewerEmpty()))
        {
            chargedAttack = true;
            center = r.bounds.center;

            AttackBase activeAttack = GetActiveAttack();
            if (activeAttack == null)
                StartCoroutine(Charge());            
            else if (activeAttack.CanBeCanceled)
            {
                activeAttack.Cancel();
                StartCoroutine(Charge());
            }
        }
        if (InputManager.GetButtonUp(control, axis) && Attacking)
        {
            StopAllCoroutines();
            effectRecipeData = inv.GetActiveEffect();
            flavorCountDictionary = new Dictionary<RecipeData.Flavors, int>(inv.GetActiveFlavorDictionary());
            ingredientArray = inv.GetActiveSkewer().ToArray();

            r.color = Color.white;
            currLevel = 0;
            Attack(crosshair.GetTarget());
            inv.ClearActiveRecipe();
            inv.ClearActiveSkewer();
            inv.CanSwap = true;
            Attacking = false;
            chargedAttack = false;
        }
    }


    private IEnumerator Charge()
    {
        PlayerController pc = controller as PlayerController;
        pc.freezeDirection = true;
        Attacking = true;
        CanBeCanceled = false;
        inv.CanSwap = false;
        for (currLevel = 0; currLevel < chargeLevels - 1; ++currLevel)
        {
            //Debug.Log("Charge Level Equals: " + currLevel);
            animator.Play(attackName + "Charge", 0, normalInterval * (currLevel + 1));
            sfxPlayer.Play(chargeSounds[currLevel]);
            float time = 0;
            while (time < chargeTime)
            {
                controller.Direction = DirectionMethods.FromVec2(GetTargetVector(crosshair.GetTarget()));
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
            controller.Direction = DirectionMethods.FromVec2(GetTargetVector(crosshair.GetTarget()));
            yield return new WaitForEndOfFrame();
            r.color = new Color(r.color.r, r.color.g + colorInc >= 1 ? 0 : r.color.g + colorInc, r.color.b + colorInc >= 1 ? 0 : r.color.b + colorInc);
        }
        pc.freezeDirection = false;
    }
}
