using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Use to give an entity a melee attack 
/// </summary>
/// [RequireComponent(typeof(Animator))]
public class AttackMelee : AttackBase
{

    #region fields

    /// <summary>
    /// Controllers to get direction state from
    /// </summary>
    protected PlayerController controller;
    protected SpriteRenderer spriteRenderer;
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

    public Control control;
    public InputAxis axis;

    protected GameObject currAttackObject = null;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        dependecies = GetComponents<AttackBase>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<PlayerController>();
    }


    private void Awake()
    {
        LoadAssetBundles();
        //defaultAttackSound = sfx_bundle.LoadAsset<AudioClip>(name);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (InputManager.GetButtonDown(control, axis))
        {
            //Get the first attack from dependecies that is attacking, else null
            AttackBase activeAttack = GetActiveAttack();        
            if(activeAttack == null)
            {
                RecalculatePosition();
                Attack();
            }
            else if(activeAttack.CanBeCanceled && activeAttack.CancelPriority <= CancelPriority)
            {
                //Debug.Log("Cancelling into" + this.ToString());
                activeAttack.Cancel();
                RecalculatePosition();
                Attack();
            }
        }
    }

    /// <summary>
    /// Adjusts the position of the attack spawner based on current direction state
    /// </summary>
    private void RecalculatePosition()
    {
        //get center offset (due to pivot changes) 
        //float spriteHeight = spriteRenderer.bounds.size.y / 32f; //characters will always be 32ppu
        //Debug.Log(spriteHeight);
        Vector2 center = spriteRenderer.bounds.center;

        Direction direction;

        //get direction from whichever controller component this entity has
        direction = controller.Direction;

        // move spawn point into position
        if (direction == Direction.East)
        {
            attackCapsuleDirection = CapsuleDirection2D.Horizontal;
            attackCapsuleRotation = 0;
            attackSpawnPoint = new Vector2(transform.position.x + (meleeRange / 2f), center.y);
        }
        else if (direction == Direction.West)
        {
            attackCapsuleDirection = CapsuleDirection2D.Horizontal;
            attackCapsuleRotation = 0;
            attackSpawnPoint = new Vector2(transform.position.x - (meleeRange / 2f), center.y);
        }
        else if (direction == Direction.North)
        {
            attackCapsuleDirection = CapsuleDirection2D.Vertical;
            attackCapsuleRotation = 0;
            attackSpawnPoint = new Vector2(transform.position.x, center.y + (meleeRange / 2f));
        }
        else if (direction == Direction.South)
        {
            attackCapsuleDirection = CapsuleDirection2D.Vertical;
            attackCapsuleRotation = 0;
            attackSpawnPoint = new Vector2(transform.position.x, center.y - (meleeRange / 2f));
        }
        //intermediate directions
        else if (direction == Direction.NorthWest)
        {
            attackCapsuleDirection = CapsuleDirection2D.Horizontal;
            attackCapsuleRotation = -45f;
            attackSpawnPoint = new Vector2(transform.position.x - (meleeRange / 2f), center.y + (meleeRange / 2f));
        }
        else if (direction == Direction.NorthEast)
        {
            attackCapsuleDirection = CapsuleDirection2D.Horizontal;
            attackCapsuleRotation = 45f;
            attackSpawnPoint = new Vector2(transform.position.x + (meleeRange / 2f), center.y + (meleeRange / 2f));
        }
        else if (direction == Direction.SouthWest)
        {
            attackCapsuleDirection = CapsuleDirection2D.Horizontal;
            attackCapsuleRotation = 45f;
            attackSpawnPoint = new Vector2(transform.position.x - (meleeRange / 2f), center.y - (meleeRange / 2f));
        }
        else if (direction == Direction.SouthEast)
        {
            attackCapsuleDirection = CapsuleDirection2D.Horizontal;
            attackCapsuleRotation = -45f;
            attackSpawnPoint = new Vector2(transform.position.x + (meleeRange / 2f), center.y - (meleeRange / 2f));
        }
    }

    public override void Attack()
    {
        if (controller.riding)
            return;

        CanBeCanceled = true;
        //animation stuff
        if(attackSound != null && audioSource != null)
        {
            audioSource.clip = attackSound;
            audioSource.Play();
            //Debug.Log("Playing override sound");
        }
        else if(attackSound == null && audioSource != null)
        {
            audioSource.clip = defaultAttackSound;
            audioSource.Play();
            //Debug.Log("Playing default sound");
        }
        animator.Play(attackName,0,0);
        //animator.Play(attackName);

        //spawn the attack at the spawn point and give it its dimensions
        Attacking = true;
        GameObject newAttack = Instantiate(attack, attackSpawnPoint, Quaternion.identity, transform);
        CapsuleCollider2D newAttackCollider = newAttack.GetComponent<CapsuleCollider2D>();

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

    public override void Cancel()
    {       
        base.Cancel();
        Destroy(currAttackObject);
    }
    /// <summary>
    /// Disables the attacking state after the attack duration has elapsed.
    /// Despawning the attack will be handled by the attack itself
    /// </summary>
    protected IEnumerator EndAttackAfterSeconds(float time, GameObject newAttack)
    {
        currAttackObject = newAttack;
        yield return new WaitForSeconds(time);
        Attacking = false;
        CanBeCanceled = false;
        currAttackObject = null;
        Destroy(newAttack);

        yield return null;
    }
}
