using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A specialized version of the attack melee script that passes an inventory reference to its child
/// Used only for skewer attacks
/// </summary>
[RequireComponent(typeof(Inventory))]
public class AttackMeleeSkewer : AttackMelee
{
    /// <summary>
    /// Reference to the player's inventory
    /// </summary>
    [System.NonSerialized]
    public Inventory inventory;

    public bool use360Targeting = true;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
        dependecies = GetComponents<AttackBase>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<PlayerController>();
    }
    
    private void Awake()
    {
        LoadAssetBundles();
        //defaultAttackSound = sfx_bundle.LoadAsset<AudioClip>("sfx_damage");

    }

    void LateUpdate()
    {
        if (InputManager.GetButtonDown(control, axis))
        {
            //Get the first attack from dependecies that is attacking, else null
            AttackBase activeAttack = GetActiveAttack();
            if (activeAttack == null)
            {
                if (use360Targeting)
                    RecalculatePosition();
                else
                    RecalculatePositionOld();
                Attack();
            }
            else if (activeAttack.CanBeCanceled && activeAttack.CancelPriority <= CancelPriority)
            {
                //Debug.Log("Cancelling into" + this.ToString());
                activeAttack.Cancel();
                if (use360Targeting)
                    RecalculatePosition();
                else
                    RecalculatePositionOld();
                Attack();
            }
        }
    }

    public override void Attack()
    {
        if (controller.riding)
            return;

        //animation stuff
        if (attackSound != null && audioSource != null)
        {
            audioSource.clip = attackSound;
            audioSource.Play();
            //Debug.Log("Playing override sound");
        }
        else if (attackSound == null && audioSource != null)
        {
            audioSource.clip = defaultAttackSound;
            audioSource.Play();
            Debug.Log("Playing default sound");
        }
        animator.Play(attackName,0,0);

        //spawn the attack at the spawn point and give it its dimensions
        Attacking = true;
        CanBeCanceled = true;
        GameObject newAttack = Instantiate(attack, attackSpawnPoint, Quaternion.identity);
        CapsuleCollider2D newAttackCollider = newAttack.GetComponent<CapsuleCollider2D>();

        newAttackCollider.direction = attackCapsuleDirection;
        newAttack.transform.Rotate(new Vector3(0, 0, attackCapsuleRotation));

        //send inventory reference
        PlayerSkewerAttack skewerAttack = newAttack.GetComponent<PlayerSkewerAttack>();
        skewerAttack.inventory = inventory;


        if (newAttackCollider.direction == CapsuleDirection2D.Horizontal)
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
    /// alternate function to recalculate the position of the attack spawner based on mouse position
    /// </summary>
    protected override void RecalculatePosition()
    {
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 difference = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized;
        attackSpawnPoint = (Vector2)transform.position + (difference * meleeRange);
    }

    protected virtual void RecalculatePositionOld()
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
}
