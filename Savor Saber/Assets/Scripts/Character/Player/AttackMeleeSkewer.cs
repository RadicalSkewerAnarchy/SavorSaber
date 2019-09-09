using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public CrosshairController crosshair;

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

        if (crosshair == null)
            crosshair = GameObject.Find("Crosshair").GetComponent<CrosshairController>();
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

        //animation and sound stuff
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
        OverrideDirection(attackCapsuleRotation);
        animator.Play(attackName,0,0);

        //spawn the attack at the spawn point and give it its dimensions
        Attacking = true;
        CanBeCanceled = true;
        GameObject newAttack = Instantiate(attack, attackSpawnPoint, Quaternion.identity);
        CapsuleCollider2D newAttackCollider = newAttack.GetComponent<CapsuleCollider2D>();
        if (!use360Targeting)
        {
            newAttackCollider.direction = attackCapsuleDirection;
            newAttack.transform.Rotate(new Vector3(0, 0, attackCapsuleRotation));
        }
        else
        {
            newAttackCollider.direction = CapsuleDirection2D.Horizontal;
            newAttack.transform.Rotate(new Vector3(0, 0, attackCapsuleRotation));
        }



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
    /// Gets the rotation of the projectile for the target direction
    /// </summary>
    protected float GetRotation(Vector2 target)
    {
        //get center offset (due to pivot changes) 
        Vector2 center = spriteRenderer.bounds.center;

        Vector2 distance = new Vector2(target.x - center.x, target.y - center.y);
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
    /// functions to calculate the spawn position of the attack
    /// </summary>
    protected override void RecalculatePosition()
    {
        //get center offset (due to pivot changes) 
        Vector2 center = spriteRenderer.bounds.center;

        Vector2 target = GetCursorTarget();
        Vector2 difference = new Vector2(target.x - center.x, target.y - center.y).normalized;
        attackSpawnPoint = (Vector2)center + (difference * meleeRange);
        attackCapsuleRotation = GetRotation(attackSpawnPoint);
    }

    private void RecalculatePositionOld()
    {

        //get center offset (due to pivot changes) 
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

    /// <summary>
    /// Sets the animation direction based on what direction the player attacked in
    /// </summary>
    private void OverrideDirection(float rotation)
    {
        Debug.Log("Overriding direction towards " + rotation + " degrees");
        //case 1: facing East
        if(rotation < 22.5f || rotation >= 337.5f)
        {
            controller.Direction = Direction.East;
        }
        //case 2: facing NorthEast
        else if(rotation >= 22.5f && rotation < 67.5f)
        {
            controller.Direction = Direction.NorthEast;
        }
        //case 3: facing North
        else if (rotation >= 67.5f && rotation < 112.5f)
        {
            controller.Direction = Direction.North;
        }
        //case 4: facing NorthWest
        else if (rotation >= 112.5f && rotation < 157.5f)
        {
            controller.Direction = Direction.NorthWest;
        }
        //case 5: facing West
        else if (rotation >= 157.5f && rotation < 202.5f)
        {
            controller.Direction = Direction.West;
        }
        //case 6: facing SouthWest
        else if (rotation >= 202.5f && rotation < 247.5f)
        {
            controller.Direction = Direction.SouthWest;
        }
        //case 2: facing South
        else if (rotation >= 247.5f && rotation < 292.5f)
        {
            controller.Direction = Direction.South;
        }
        //case 2: facing SouthEast
        else if (rotation >= 292.5f && rotation < 337.5f)
        {
            controller.Direction = Direction.SouthEast;
        }
    }

    /// <summary>
    /// Returns the position in world space of the targeting cursor
    /// </summary>
    private Vector2 GetCursorTarget()
    {
        if (InputManager.ControllerMode)
        {
            Vector2 target = Camera.main.ScreenToWorldPoint(crosshair.gameObject.transform.position);
            return target;
        }
        else
        {
            Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return target;
        }

    }
}
