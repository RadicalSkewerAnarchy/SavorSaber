using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

[RequireComponent(typeof(Inventory))]
public class AttackRangedThrowSkewer : AttackRanged
{
    public int chargeLevels = 3;
    public int energyCostToThrow = 1;
    public float chargeTime = 1;
    public float cooldown = 2;
    private float cooldownTicLength = 0.05f;
    [SerializeField]
    private Slider cooldownSlider;
    private bool ready = true;
    public AudioClip[] chargeSounds = new AudioClip[3];
    [HideInInspector]
    public int currLevel = 0;
    private float normalInterval;
    private Inventory inv;
    private PlaySFX sfxPlayer;
    public CrosshairController crosshair;
    private WaitForSeconds tic;
    private ProjectileSkewer skewerProjectileData;

    /// <summary>
    /// how much of each flavor is present on the skewer
    /// </summary>
    public Dictionary<RecipeData.Flavors, int> flavorCountDictionary;
    public IngredientData[] ingredientArray;

    private PlayerController playerController; //this is insconsistent with the player's melee attacks but it has to be, because controller is already an EntityController
    //and I don't want to fuck it up by changing it to a playercontroller

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        tic = new WaitForSeconds(cooldownTicLength);
        animator = GetComponent<Animator>();       
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<EntityController>();
        playerController = GetComponent<PlayerController>();
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

        //animation cancel
        if (Attacking && (InputManager.GetButtonDown(Control.Knife, InputAxis.Slash) || InputManager.GetButtonDown(Control.Skewer, InputAxis.Skewer)) && !playerController.uiOpen)
        {
            //Debug.Log("Animation cancel");
            StopAllCoroutines();
            r.color = Color.white;
            currLevel = 0;
            inv.CanSwap = true;
            chargedAttack = false;
            Attacking = false;
            CanBeCanceled = false;
            return;
        }
        
        //conditions to throw: Must have ingredients and energy, Skewer has contents, not in UI
        if (!Attacking && ready && InputManager.GetButtonDown(control, axis) && (!inv.ActiveSkewerEmpty()) && !playerController.uiOpen)
        {
            if (inv.GetEnergy() < energyCostToThrow)
            {
                sfxPlayer.Play(failSound);
                return;
            }
            StopAllCoroutines();
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
            return;

        }
        if (InputManager.GetButtonUp(control, axis) && ready && !playerController.uiOpen)
        {
            if (!Attacking)
            {
                //Debug.Log("Got throw button up, but attacking bool is false");
                return;
            }
            inv.SubtractEnergy(energyCostToThrow);
            StopAllCoroutines();
            flavorCountDictionary = new Dictionary<RecipeData.Flavors, int>(inv.GetActiveFlavorDictionary());
            //find the top ingredient
            ingredientArray = inv.GetActiveSkewer().ToArray();

            r.color = Color.white;
            currLevel = 0;
            Attack(crosshair.GetTarget());

            //set ingredient data if applicable
            if (ingredientArray != null)
            {
                skewerProjectileData.ingredientArray = new IngredientData[ingredientArray.Length];
                Array.Copy(ingredientArray, skewerProjectileData.ingredientArray, ingredientArray.Length);
            }

            inv.CanSwap = true;
            Attacking = false;
            chargedAttack = false;

            ready = false;
            cooldownSlider.gameObject.SetActive(true);
            StartCooldown(cooldown);
        }
    }

    /// <summary>
    /// Overloaded version of Attack() that takes in a target vector and uses that to get its rotation.
    /// </summary>
    public override void Attack(Vector2 targetVector)
    {
        if (!chargedAttack)
        {
            //true center of sprite
            center = r.bounds.center;
        }

        //sound stuff
        if (attackSound != null && audioSource != null)
        {
            audioSource.clip = attackSound;
            audioSource.Play();

        }
        else if (attackSound == null && audioSource != null)
        {
            audioSource.clip = defaultAttackSound;
            audioSource.Play();
        }


        //get directional/rotational information
        //float projectileRotation = GetRotation(direction);
        float projectileRotation = GetRotation(targetVector);
        Vector2 directionVector = GetTargetVector(targetVector);
        Direction direction = DirectionMethods.FromVec2(directionVector);
        Vector2 spawnPositionModifier = directionVector.normalized * 0.75f;

        //spawn the attack at the spawn point and give it its data
        GameObject newAttack = Instantiate(projectile, center + spawnPositionModifier, Quaternion.identity);
        skewerProjectileData = newAttack.GetComponent<ProjectileSkewer>();
        skewerProjectileData.attacker = this.gameObject;
        skewerProjectileData.myCharData = GetComponent<CharacterData>();
        skewerProjectileData.direction = direction;
        skewerProjectileData.directionVector = directionVector;
        skewerProjectileData.projectileDamage += extraDamage;
        newAttack.transform.Rotate(new Vector3(0, 0, projectileRotation));


        //play animations
        //OverrideDirection(projectileRotation);
        if (controller != null)
            controller.Direction = direction;
        if (animator != null)
            animator.Play(attackName);

        Attacking = true;
        StartCoroutine(EndAttackAfterSeconds(attackDuration));
    }

    private void StartCooldown(float cd)
    {
        //Debug.Log(cd);
        if(cd <= 0)
        {
            ready = true;
            cooldownSlider.gameObject.SetActive(false);
            return;
        }
        else
        {
            float nextCD = cd - cooldownTicLength;
            StartCoroutine(CooldownTimer(nextCD));
        }
    }

    private IEnumerator CooldownTimer(float cd)
    {
        yield return tic;
        cooldownSlider.value = cd / cooldown;
        StartCooldown(cd);
        yield return null;
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
