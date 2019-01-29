using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    protected UpdatedController playerController;
    protected MonsterMovement monsterController;

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
    public string inputAxis;

    #endregion

    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
        dependecies = GetComponents<AttackBase>();
        //has to have either a monster controller or player controller
        playerController = GetComponent<UpdatedController>();
        if (playerController == null)
            monsterController = GetComponent<MonsterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(inputAxis))
        {
            //Get the first attack from dependecies that is attacking, else null
            AttackBase activeAttack = dependecies.FirstOrDefault((at) => at.Attacking);
            if (activeAttack == null)
                Attack();
            else if (activeAttack.CanCancel)
            {
                activeAttack.Cancel();
                Attack();
            }
        }
    }

    public override void Attack()
    {
        Direction direction = playerController?.direction ?? monsterController.direction;

        //animation stuff
        animator.Play(attackName);

        //spawn the attack at the spawn point and give it its direction
        GameObject newAttack = Instantiate(projectile, transform.position, Quaternion.identity);
        BaseProjectile projectileData = newAttack.GetComponent<BaseProjectile>();
        projectileData.direction = direction;

        //give the spawned projectile its effect data, if applicable
        if(effectRecipeData != null)
        {
            projectileData.effectRecipeData = effectRecipeData;
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
        CanCancel = false;
        yield return null;
    }
}
