using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UpdatedController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Inventory))]
public class PlayerMeleeAttack : MeleeAttack
{

    private string attackType; //Knife or Skewer

    /// <summary>
    /// These are temporary visuals. Remove them once animations
    /// are implemented.
    /// </summary>
    public GameObject knife;
    public GameObject skewer;

    /// <summary>
    /// Reference to the movement controller that will tell us what directional
    /// state we're in. This should be a component of the same GameObject.
    /// </summary>
    protected UpdatedController controller;
    private Animator animator;

    /// <summary>
    /// Reference to the inventory that collected objects will be added to
    /// </summary>
    protected Inventory inventory;

    /// <summary>
    /// Array of layers that the weapons should not collide with.
    /// Weapon layer is 10. Terrain is 9, for example.
    /// </summary>
    public int[] layersToIgnore;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<UpdatedController>();
        meleeCollider = GetComponent<CapsuleCollider2D>();
        inventory = GetComponent<Inventory>();


        //does not currently work. 
        for(int i = 0; i < layersToIgnore.Length; i++)
        {
            Debug.Log("Ignoring layer " + layersToIgnore[i]);
            Physics.IgnoreLayerCollision(10, layersToIgnore[i]);
        }

        RecalculatePosition();

    }

    // Update is called once per frame
    void Update()
    {
        RecalculatePosition();

        // Knife attack if player hits button1 and not already attacking
        if (Input.GetButtonDown("Fire1")  && !attacking)
        {
            attackType = "Knife";
            Attack();
        }
        else if(Input.GetButtonDown("Fire2") && !attacking)
        {
            attackType = "Skewer";
            Attack();
        }
    }
    
    /// <summary>
    /// Adjusts the position of the hitboxes based on current direction state
    /// </summary>
    private void RecalculatePosition()
    {
        //Debug.Log(controller.directionState);

        // move melee collider and sprites into position
        var direction = controller.direction;
        if (direction == Direction.East && !attacking)
        {
            meleeCollider.direction = CapsuleDirection2D.Horizontal;
            meleeCollider.size = new Vector2(meleeRange, meleeWidth);
            meleeCollider.offset = new Vector2(meleeRange / 2f, 0f);

            skewer.transform.localPosition = new Vector3(1f, 0f, 0f);
            knife.transform.localPosition = new Vector3(0.5f, 0f, 0f);
            skewer.transform.eulerAngles = knife.transform.eulerAngles = new Vector3(0f, 0f, 270f);
        }
        else if (direction == Direction.West && !attacking)
        {
            meleeCollider.direction = CapsuleDirection2D.Horizontal;
            meleeCollider.size = new Vector2(meleeRange, meleeWidth);
            meleeCollider.offset = new Vector2(meleeRange / -2f, 0f);

            skewer.transform.localPosition = new Vector3(-1f, 0f, 0f);
            knife.transform.localPosition = new Vector3(-0.5f, 0f, 0f);
            skewer.transform.eulerAngles = knife.transform.eulerAngles = new Vector3(0f, 0f, 90f);
        }
        else if (direction == Direction.North && !attacking)
        {
            meleeCollider.direction = CapsuleDirection2D.Vertical;
            meleeCollider.size = new Vector2(meleeWidth, meleeRange);
            meleeCollider.offset = new Vector2(0f, meleeRange / 2f);

            skewer.transform.localPosition = new Vector3(0f, 1f, 0f);
            knife.transform.localPosition = new Vector3(0f, 0.5f, 0f);
            skewer.transform.eulerAngles = knife.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (direction == Direction.South && !attacking)
        {
            meleeCollider.direction = CapsuleDirection2D.Vertical;
            meleeCollider.size = new Vector2(meleeWidth, meleeRange);
            meleeCollider.offset = new Vector2(0f, meleeRange / -2f);

            skewer.transform.localPosition = new Vector3(0f, -1f, 0f);
            knife.transform.localPosition = new Vector3(0f, -0.5f, 0f);
            skewer.transform.eulerAngles = knife.transform.eulerAngles = new Vector3(0f, 0f, 180f);
        }
    }

    /// <summary>
    /// Performs the melee attack action.
    /// </summary>
    public override void Attack()
    {
        animator.SetTrigger("Attack");
        animator.SetTrigger(attackType);
        if (attackType == "Knife")
            AttackWithKnife();
        else if (attackType == "Skewer")
            AttackWithSkewer();    
    }

    private void AttackWithKnife()
    {
        //set range to match this attack sub-type
        meleeRange = 1f;
        RecalculatePosition();

        attacking = true;
        meleeCollider.enabled = true;
        knife.SetActive(true);

        StartCoroutine(EndAttackAfterSeconds(attackDuration));

    }

    private void AttackWithSkewer()
    {
        //set range to match this attack sub-type
        meleeRange = 2f;
        RecalculatePosition();

        attacking = true;
        meleeCollider.enabled = true;
        skewer.SetActive(true);

        StartCoroutine(EndAttackAfterSeconds(attackDuration));
    }

    /// <summary>
    /// Disables the attacking state after the attack duration has elapsed.
    /// </summary>
    IEnumerator EndAttackAfterSeconds(float time)
    {
        endSignalSent = true;

        yield return new WaitForSeconds(time);

        attacking = false;
        meleeCollider.enabled = false;
        knife.SetActive(false);
        skewer.SetActive(false);

        yield return null;
    }

    /// <summary>
    /// Determines which type of attack hit and apply the right effect.
    /// </summary>
    /// 
    #region Trigger version
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered");
        if (attackType == "Knife")
            ApplyKnifeEffect(collision);
        else if (attackType == "Skewer")
            ApplySkewerEffect(collision);
    }

    public void ApplyKnifeEffect(Collider2D collision)
    {
        Debug.Log("Applying Knife Effect");
        if (collision.gameObject.tag == "KnifableObject")
        {
            //just instakill the monster for testing purposes
            Monster targetMonster = collision.gameObject.GetComponent<Monster>();
            targetMonster.Kill();

            CharacterData characterData = collision.gameObject.GetComponent<CharacterData>();
            if(characterData != null)
            {
                characterData.health -= (int)meleeDamage;
            }
        }



    }

    public void ApplySkewerEffect(Collider2D collision)
    {
        Debug.Log("Applying Skewer Effect");
        if (collision.gameObject.tag == "SkewerableObject" && !inventory.ActiveSkewerFull())
        {
            Debug.Log("Hit skewerable object");
            SkewerableObject targetObject = collision.gameObject.GetComponent<SkewerableObject>();

            inventory.AddToSkewer(targetObject.data);
            Destroy(collision.gameObject);
        }
    }
    #endregion

    /// <summary>
    /// Older version that fully collides with target. Results in some knockback, probably not ideal
    /// </summary>
    #region Collision2D version
    /*
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (attackType == "Knife")
            ApplyKnifeEffect(collision);
        else if (attackType == "Skewer")
            ApplySkewerEffect(collision);
    }

    public void ApplyKnifeEffect(Collision2D collision)
    {
        if (collision.gameObject.tag == "KnifableObject")
        {
            Health targetHealth = collision.gameObject.GetComponent<Health>();
            targetHealth.Hp -= (int)meleeDamage;
        }
    }

    public void ApplySkewerEffect(Collision2D collision)
    {
        Debug.Log("Applying Skewer Effect");
        if(collision.gameObject.tag == "SkewerableObject" && !inventory.ActiveSkewerFull())
        {
            Debug.Log("Hit skewerable object");
            SkewerableObject targetObject = collision.gameObject.GetComponent<SkewerableObject>();

            inventory.AddToSkewer(targetObject.data);
            Destroy(collision.gameObject);
        }
    }
    */
    #endregion
}
