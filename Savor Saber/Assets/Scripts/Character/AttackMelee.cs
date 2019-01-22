using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use to give an entity a melee attack 
/// </summary>
/// [RequireComponent(typeof(Animator))]
public class AttackMelee : MonoBehaviour
{

    #region fields

    /// <summary>
    /// Controllers to get direction state from
    /// </summary>
    protected UpdatedController playerController;
    protected MonsterMovement monsterController;

    private Animator animator;

    /// <summary>
    /// field for the attack prefab to be spawned when attacking
    /// </summary>
    public GameObject attack;

    /// <summary>
    /// The collider orientation for the melee attack.
    /// </summary>
    private CapsuleDirection2D capsuleDirection;

    /// <summary>
    /// where the attack collider will be spawned
    /// </summary>
    private Vector2 attackSpawnPoint;

    /// <summary>
    /// How much damage the attack does. Currently a float in case we want.
    /// finer damage values?
    /// </summary>
    public float meleeDamage = 1f;

    /// <summary>
    /// Number of units in front of the character this attack can reach.
    /// </summary>
    public float meleeRange = 1f;

    /// <summary>
    /// How many units wide this attack is.
    /// </summary>
    public float meleeWidth = 1f;

    /// <summary>
    /// How many seconds the collider will remain active.
    /// </summary>
    public float attackDuration = 0.5f;

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
    }

    /// <summary>
    /// Adjusts the position of the attack spawner based on current direction state
    /// </summary>
    private void RecalculatePosition()
    {
        //Debug.Log(controller.directionState);
        Direction direction;

        //get direction from whichever controller component this entity has
        if(playerController != null)
        {
            direction = playerController.direction;
        }
        else
        {
            direction = monsterController.direction;
        }

        // move spawn point into position
        if (direction == Direction.East && !attacking)
        {
            capsuleDirection = CapsuleDirection2D.Horizontal;
            attackSpawnPoint = new Vector2(transform.position.x + (meleeRange / 2f), transform.position.y);
        }
        else if (direction == Direction.West && !attacking)
        {
            capsuleDirection = CapsuleDirection2D.Horizontal;
            attackSpawnPoint = new Vector2(transform.position.x - (meleeRange / 2f), transform.position.y);
        }
        else if (direction == Direction.North && !attacking)
        {
            capsuleDirection = CapsuleDirection2D.Vertical;
            attackSpawnPoint = new Vector2(transform.position.x, transform.position.y + (meleeRange / 2f));
        }
        else if (direction == Direction.South && !attacking)
        {
            capsuleDirection = CapsuleDirection2D.Vertical;
            attackSpawnPoint = new Vector2(transform.position.x, transform.position.y - (meleeRange / 2f));
        }
    }

    public virtual void Attack()
    {
        attacking = true;
        Instantiate(attack, attackSpawnPoint, Quaternion.identity);
        StartCoroutine(EndAttackAfterSeconds(attackDuration));
    }

    /// <summary>
    /// Disables the attacking state after the attack duration has elapsed.
    /// Despawning the attack will be handled by the attack itself
    /// </summary>
    IEnumerator EndAttackAfterSeconds(float time)
    {
        endSignalSent = true;
        yield return new WaitForSeconds(time);
        attacking = false;

        yield return null;
    }
}
