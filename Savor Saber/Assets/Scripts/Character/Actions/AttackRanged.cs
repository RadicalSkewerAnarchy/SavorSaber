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

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        dependecies = GetComponents<AttackBase>();
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
        float projectileRotation = GetRotation(direction);
        Vector2 directionVector = GetDirectionVector(direction);

        //spawn the attack at the spawn point and give it its data
        GameObject newAttack = Instantiate(projectile, center, Quaternion.identity);
        Physics2D.IgnoreCollision(newAttack.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        BaseProjectile projectileData = newAttack.GetComponent<BaseProjectile>();
        projectileData.direction = direction;
        projectileData.directionVector = directionVector;
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
    public void Attack(Vector2 targetVector)
    {
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
        animator.Play(attackName);

        Direction direction = controller.Direction;
        //float projectileRotation = GetRotation(direction);
        float projectileRotation = GetRotation(targetVector);
        Vector2 directionVector = GetTargetVector(targetVector);

        //spawn the attack at the spawn point and give it its data
        GameObject newAttack = Instantiate(projectile, center, Quaternion.identity);
        BaseProjectile projectileData = newAttack.GetComponent<BaseProjectile>();
        projectileData.direction = direction;
        projectileData.directionVector = directionVector;
        newAttack.transform.Rotate(new Vector3(0, 0, projectileRotation));

        //give the spawned projectile its effect data, if applicable
        if (effectRecipeData != null)
        {
            projectileData.effectRecipeData = effectRecipeData;
        }
        if (ingredientArray != null)
        {
            projectileData.ingredientArray = new IngredientData[ingredientArray.Length];
            Array.Copy(ingredientArray, projectileData.ingredientArray, ingredientArray.Length);
        }
        if(flavorCountDictionary != null)
        {
            projectileData.flavorCountDictionary = new Dictionary<RecipeData.Flavors, int>(flavorCountDictionary);
        }

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
        yield return null;
    }

    /// <summary>
    /// Get the rotation associated with the current facing
    /// </summary>
    protected float GetRotation(Direction direction)
    {
        float projectileRotation = 0f; 

        // set projectile velocity vector
        if (direction == Direction.East)
        {
            projectileRotation = 0f;
        }
        else if (direction == Direction.West)
        {
            projectileRotation = 180f;
        }
        else if (direction == Direction.North)
        {

            projectileRotation = 90f;
        }
        else if (direction == Direction.South)
        {

            projectileRotation = -90;
        }
        else if (direction == Direction.NorthWest)
        {
            projectileRotation = 135;
        }
        else if (direction == Direction.NorthEast)
        {
            projectileRotation = 45;
        }
        else if (direction == Direction.SouthWest)
        {
            projectileRotation = 225;
        }
        else if (direction == Direction.SouthEast)
        {
            projectileRotation = 315;
        }

        return projectileRotation;
    }

    protected float GetRotation(Vector2 target)
    {
        Vector2 distance =new Vector2(target.x - transform.position.x, target.y - transform.position.y);
        float arctan = Mathf.Atan(distance.y / distance.x);
        float angle = (float)(arctan * (180 / Math.PI));
        Debug.Log("Projectile rotation: " + angle);
        return angle;
    }

    /// <summary>
    /// Get the vector associated with the current facing
    /// </summary>
    protected Vector2 GetDirectionVector(Direction direction)
    {
        Vector2 directionVector;
        if (direction == Direction.East)
        {
            directionVector = new Vector2(1, 0);       
        }
        else if (direction == Direction.West)
        {
            directionVector = new Vector2(-1, 0);    
        }
        else if (direction == Direction.North)
        {
            directionVector = new Vector2(0, 1);
        }
        else if (direction == Direction.South)
        {
            directionVector = new Vector2(0, -1);
        }
        else if (direction == Direction.NorthWest)
        {
            directionVector = new Vector2(-1, 1).normalized;
        }
        else if (direction == Direction.NorthEast)
        {
            directionVector = new Vector2(1, 1).normalized;
        }
        else if (direction == Direction.SouthWest)
        {
            directionVector = new Vector2(-1, -1).normalized;
        }
        else if (direction == Direction.SouthEast)
        {
            directionVector = new Vector2(1, -1).normalized;
        }
        else
        {
            directionVector = new Vector2(0, 0);
        }

        return directionVector;
    }

    /// <summary>
    /// Get the vector pointing at a target's position
    /// </summary>
    protected Vector2 GetTargetVector(Vector2 targetVector)
    {
        return new Vector2(targetVector.x - transform.position.x, targetVector.y - transform.position.y).normalized;
    }
}
