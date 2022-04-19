using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// Use to give an entity a melee attack 
/// </summary>
/// [RequireComponent(typeof(Animator))]
public class AttackRanged : AttackBase
{

    #region fields

    /// <summary>
    /// Controllers to get direction state from
    /// </summary>
    protected EntityController controller;
    protected SpriteRenderer r;

    /// <summary>
    /// fields related to compensating for non-standard pivots
    /// </summary>
    protected Vector2 center;
    protected bool chargedAttack = false;

    protected Animator animator;

    /// <summary>
    /// field for the projectile prefab to be spawned when attacking
    /// </summary>
    public GameObject projectile;

    /// <summary>
    /// The data for what effects this attack should have, if any.
    /// </summary>
    [HideInInspector]
    public RecipeData effectRecipeData = null;

    /// <summary>
    /// how much of each flavor is present on the skewer
    /// </summary>
    public Dictionary<RecipeData.Flavors, int> flavorCountDictionary;
    public IngredientData[] ingredientArray;

    /// <summary>
    /// The name of the attack, used to determine animation states
    /// </summary>
    public string attackName;

    /// <summary>
    /// Minimum interval between shots.
    /// </summary>
    public float attackDuration = 0.5f;

    /// <summary>
    /// what input axis, if any, should be accepted to trigger this attack
    /// </summary>
    public Control control;
    public InputAxis axis;

    /// <summary>
    /// Any damage buffs to be applied to the projectile
    /// </summary>
    [HideInInspector]
    public int extraDamage = 0;

    #endregion

    #region Bonus effect fields
    //Trust bonus effects
    [Header("Trust bonus effects")]
    public GameObject spicyTemplate;
    public GameObject sourTemplate;
    protected RecipeData.Flavors flavor = RecipeData.Flavors.None;
    protected int flavorMagnitude = 1;
    public bool spawnEffectOnMiss = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        animator = GetComponent<Animator>();      
        r = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<EntityController>();

    }

    private void Awake()
    {
        LoadAssetBundles();
        //defaultAttackSound = sfx_bundle.LoadAsset<AudioClip>(name);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetButtonDown(control, axis))
        {
            //Get the first attack from dependecies that is attacking, else null
            AttackBase activeAttack = GetActiveAttack();
            if (activeAttack == null)
                Attack();
            else if (activeAttack.CanBeCanceled)
            {
                activeAttack.Cancel();
                Attack();
            }
        }
    }

    public override void Attack()
    {

        // if it was a charged attack, centerpoint is found when charge starts, not
        // when attack is released.
        if (!chargedAttack)
        {
            //true center of sprite
            center = r.bounds.center;
        }

        //animation stuff
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
        animator.Play(attackName,0,0);

        Direction direction = controller.Direction;
        if (controller is PlayerController)
            if ((controller as PlayerController).riding)
                direction = Direction.South;
        float projectileRotation = direction.ToAngleDeg();
        Vector2 directionVector = direction.ToVector2();

        //spawn the attack at the spawn point and give it its data
        GameObject newAttack = Instantiate(projectile, center, Quaternion.identity);
        Physics2D.IgnoreCollision(newAttack.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        BaseProjectile projectileData = newAttack.GetComponent<BaseProjectile>();
        projectileData.direction = direction;
        projectileData.myCharData = GetComponent<CharacterData>();
        projectileData.directionVector = directionVector;
        projectileData.projectileDamage += extraDamage;
        newAttack.transform.Rotate(new Vector3(0, 0, projectileRotation));

        //give the spawned projectile its effect data, if applicable
        if (effectRecipeData != null)
        {
            projectileData.effectRecipeData = effectRecipeData;
        }
        if(ingredientArray != null)
        {
            projectileData.ingredientArray = new IngredientData[ingredientArray.Length];
            Array.Copy(ingredientArray, projectileData.ingredientArray, ingredientArray.Length);
        }
        if (flavorCountDictionary != null)
        {
            projectileData.flavorCountDictionary = new Dictionary<RecipeData.Flavors, int>(flavorCountDictionary);
        }

        Attacking = true;
        StartCoroutine(EndAttackAfterSeconds(attackDuration));
    }

    /// <summary>
    /// Overloaded version of Attack() that takes in a target vector and uses that to get its rotation.
    /// </summary>
    public virtual void Attack(Vector2 targetVector)
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
        BaseProjectile projectileData = newAttack.GetComponent<BaseProjectile>();
        projectileData.myCharData = GetComponent<CharacterData>();
        projectileData.direction = direction;
        projectileData.directionVector = directionVector;
        projectileData.projectileDamage += extraDamage;
        newAttack.transform.Rotate(new Vector3(0, 0, projectileRotation));

        //give the spawned projectile its effect data, if applicable (DEPRECATED)
        if (effectRecipeData != null)
        {
            projectileData.effectRecipeData = effectRecipeData;
        }

        //set any bonus effects to be spawned
        if (flavor == RecipeData.Flavors.Spicy && spicyTemplate != null)
            projectileData.SetBonusEffect(spicyTemplate, flavorMagnitude);
        else if (flavor == RecipeData.Flavors.Sour && sourTemplate != null)
            projectileData.SetBonusEffect(sourTemplate, flavorMagnitude);
        else if(flavor == RecipeData.Flavors.Sweet || flavor == RecipeData.Flavors.Salty || flavor == RecipeData.Flavors.None)
            projectileData.SetBonusEffect(null, flavorMagnitude);

        projectileData.spawnBonusEffectOnMiss = spawnEffectOnMiss;

        //set ingredient data if applicable
        if (ingredientArray != null)
        {
            projectileData.ingredientArray = new IngredientData[ingredientArray.Length];
            Array.Copy(ingredientArray, projectileData.ingredientArray, ingredientArray.Length);
        }
        //DEPRECATED
        if(flavorCountDictionary != null)
        {
            projectileData.flavorCountDictionary = new Dictionary<RecipeData.Flavors, int>(flavorCountDictionary);
        }


        //play animations
        //OverrideDirection(projectileRotation);
        controller.Direction = direction;
        animator.Play(attackName);

        Attacking = true;
        StartCoroutine(EndAttackAfterSeconds(attackDuration));
    }

    /// <summary>
    /// Disables the attacking state after the attack duration has elapsed.
    /// Despawning the attack will be handled by the attack itself
    /// </summary>
    protected IEnumerator EndAttackAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        Attacking = false;
        CanBeCanceled = false;
        yield break;
    }

    protected float GetRotation(Vector2 target)
    {
        Vector2 distance =new Vector2(target.x - transform.position.x, target.y - transform.position.y);
        float arctan = Mathf.Atan(distance.y / distance.x);
        float angle = (float)(arctan * (180 / Math.PI));

        //account for Unity trig stuff
        //probably not the best way to do this but it works :V
        if (target.x < center.x)
            angle += 180;
        else if (target.x > center.x && target.y < center.y)
            angle += 360;

        return angle;
    }

    /// <summary>
    /// Get the vector pointing at a target's position
    /// </summary>
    protected Vector2 GetTargetVector(Vector2 targetVector)
    {
        return new Vector2(targetVector.x - transform.position.x, targetVector.y - transform.position.y).normalized;
    }

    //function for the Trust Meter to assign the correct
    public void SetFlavor(RecipeData.Flavors flav, int mag = 1)
    {
        flavor = flav;
        flavorMagnitude = mag;
    }
}
