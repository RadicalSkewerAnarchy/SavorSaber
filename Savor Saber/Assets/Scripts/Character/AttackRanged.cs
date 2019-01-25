using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use to give an entity a melee attack 
/// </summary>
/// [RequireComponent(typeof(Animator))]
public class AttackRanged : MonoBehaviour
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
    public BaseProjectile projectile;

    /// <summary>
    /// The name of the attack, used to determine animation states
    /// </summary>
    public string attackName;

    /// <summary>
    /// Minimum interval between shots.
    /// </summary>
    public float attackDuration = 0.5f;

    //what input axis, if any, should be accepted to trigger this attack
    public string inputAxis;

    /// <summary>
    /// To prevent attack action while still attacking.
    /// </summary>
    protected bool endSignalSent = false;
    protected bool attacking = false;

    #endregion

    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();

        //has to have either a monster controller or player controller
        playerController = GetComponent<UpdatedController>();
        if (playerController == null)
            monsterController = GetComponent<MonsterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        RecalculatePosition();
        if (Input.GetButtonDown(inputAxis) && !attacking)
        {
            Attack();
        }
    }

    /// <summary>
    /// Adjusts the position of the attack spawner based on current direction state
    /// </summary>
    private void RecalculatePosition()
    {
        Direction direction;

        //get direction from whichever controller component this entity has
        if (playerController != null)
        {
            direction = playerController.direction;
        }
        else
        {
            direction = monsterController.direction;
        }

        //Debug.Log(direction);

        // move spawn point into position
        if (direction == Direction.East && !attacking)
        {

        }
        else if (direction == Direction.West && !attacking)
        {

        }
        else if (direction == Direction.North && !attacking)
        {

        }
        else if (direction == Direction.South && !attacking)
        {

        }
        //intermediate directions
        else if (direction == Direction.NorthWest && !attacking)
        {

        }
        else if (direction == Direction.NorthEast && !attacking)
        {

        }
        else if (direction == Direction.SouthWest && !attacking)
        {

        }
        else if (direction == Direction.SouthEast && !attacking)
        {

        }
    }

    public virtual void Attack()
    {


        //animation stuff
        animator.SetTrigger("Attack");
        animator.SetTrigger(attackName);

        //spawn the attack at the spawn point and give it its dimensions
        attacking = true;
 
        StartCoroutine(EndAttackAfterSeconds(attackDuration));
    }

    /// <summary>
    /// Disables the attacking state after the attack duration has elapsed.
    /// Despawning the attack will be handled by the attack itself
    /// </summary>
    protected IEnumerator EndAttackAfterSeconds(float time)
    {
        endSignalSent = true;
        yield return new WaitForSeconds(time);
        attacking = false;

        yield return null;
    }
}
