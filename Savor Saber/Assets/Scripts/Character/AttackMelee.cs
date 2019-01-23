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

    protected Animator animator;

    /// <summary>
    /// The collider orientation for the melee attack.
    /// </summary>
    protected CapsuleDirection2D attackCapsuleDirection;

    /// <summary>
    /// how much to rotate the attack capsule
    /// Needed for diagonal attack
    /// </summary>
    protected float attackCapsuleRotation;

    /// <summary>
    /// where the attack collider will be spawned
    /// </summary>
    protected Vector2 attackSpawnPoint;

    /// <summary>
    /// field for the attack prefab to be spawned when attacking
    /// </summary>
    public GameObject attack;

    /// <summary>
    /// The name of the attack, used to determine animation states
    /// </summary>
    public string attackName;

    /// <summary>
    /// Number of units in front of the character this attack can reach.
    /// </summary>
    public float meleeRange = 1f;

    /// <summary>
    /// How many units wide this attack is.
    /// </summary>
    public float meleeWidth = 1f;

    /// <summary>
    /// How many seconds the attack will remain active.
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
        if(playerController != null)
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
            attackCapsuleDirection = CapsuleDirection2D.Horizontal;
            attackCapsuleRotation = 0;
            attackSpawnPoint = new Vector2(transform.position.x + (meleeRange / 2f), transform.position.y);
        }
        else if (direction == Direction.West && !attacking)
        {
            attackCapsuleDirection = CapsuleDirection2D.Horizontal;
            attackCapsuleRotation = 0;
            attackSpawnPoint = new Vector2(transform.position.x - (meleeRange / 2f), transform.position.y);
        }
        else if (direction == Direction.North && !attacking)
        {
            attackCapsuleDirection = CapsuleDirection2D.Vertical;
            attackCapsuleRotation = 0;
            attackSpawnPoint = new Vector2(transform.position.x, transform.position.y + (meleeRange / 2f));
        }
        else if (direction == Direction.South && !attacking)
        {
            attackCapsuleDirection = CapsuleDirection2D.Vertical;
            attackCapsuleRotation = 0;
            attackSpawnPoint = new Vector2(transform.position.x, transform.position.y - (meleeRange / 2f));
        }
        //intermediate directions
        else if (direction == Direction.NorthWest && !attacking)
        {
            attackCapsuleDirection = CapsuleDirection2D.Horizontal;
            attackCapsuleRotation = -45f;
            attackSpawnPoint = new Vector2(transform.position.x - (meleeRange / 2f), transform.position.y + (meleeRange / 2f));
        }
        else if (direction == Direction.NorthEast && !attacking)
        {
            attackCapsuleDirection = CapsuleDirection2D.Horizontal;
            attackCapsuleRotation = 45f;
            attackSpawnPoint = new Vector2(transform.position.x + (meleeRange / 2f), transform.position.y + (meleeRange / 2f));
        }
        else if (direction == Direction.SouthWest && !attacking)
        {
            attackCapsuleDirection = CapsuleDirection2D.Horizontal;
            attackCapsuleRotation = 45f;
            attackSpawnPoint = new Vector2(transform.position.x - (meleeRange / 2f), transform.position.y - (meleeRange / 2f));
        }
        else if (direction == Direction.SouthEast && !attacking)
        {
            attackCapsuleDirection = CapsuleDirection2D.Horizontal;
            attackCapsuleRotation = -45f;
            attackSpawnPoint = new Vector2(transform.position.x + (meleeRange / 2f), transform.position.y - (meleeRange / 2f));
        }
    }

    public virtual void Attack()
    {


        //animation stuff
        animator.SetTrigger("Attack");
        animator.SetTrigger(attackName);

        //spawn the attack at the spawn point and give it its dimensions
        attacking = true;
        GameObject newAttack = Instantiate(attack, attackSpawnPoint, Quaternion.identity);
        CapsuleCollider2D newAttackCollider = newAttack.GetComponent<CapsuleCollider2D>();

        Debug.Log(attackCapsuleRotation);
        newAttackCollider.direction = attackCapsuleDirection;
        newAttack.transform.Rotate(new Vector3(0, 0, attackCapsuleRotation));

        if(newAttackCollider.direction == CapsuleDirection2D.Horizontal)
        {
            newAttackCollider.size = new Vector2(meleeRange, meleeWidth);
        }
        else
        {
            newAttackCollider.size = new Vector2(meleeWidth, meleeRange);
        }

        StartCoroutine(EndAttackAfterSeconds(attackDuration, newAttack));
    }

    /// <summary>
    /// Disables the attacking state after the attack duration has elapsed.
    /// Despawning the attack will be handled by the attack itself
    /// </summary>
    protected IEnumerator EndAttackAfterSeconds(float time, GameObject newAttack)
    {
        endSignalSent = true;
        yield return new WaitForSeconds(time);
        attacking = false;

        Destroy(newAttack);

        yield return null;
    }
}
