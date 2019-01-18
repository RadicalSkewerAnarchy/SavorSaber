using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UpdatedController))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class MeleeAttack : MonoBehaviour
{
    /// <summary>
    /// Reference to the movement controller that will tell us what directional
    /// state we're in. This should be a component of the same GameObject.
    /// </summary>
    private UpdatedController controller;

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
    /// The collider for the melee attack.
    /// </summary>
    private CapsuleCollider2D meleeCollider;

    /// <summary>
    /// To prevent attack action while still attacking.
    /// </summary>
    private bool endSignalSent = false;
    private bool slashing = false;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<UpdatedController>();
        meleeCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(controller.directionState);

        // move melee collider into position
        string directionState = controller.directionState;
        if (directionState == "East")
        {
            meleeCollider.direction = CapsuleDirection2D.Horizontal;
            meleeCollider.size = new Vector2(meleeRange, meleeWidth);
            meleeCollider.offset = new Vector2(meleeRange / 2, 0);
        }
        else if(directionState == "West")
        {
            meleeCollider.direction = CapsuleDirection2D.Horizontal;
            meleeCollider.size = new Vector2(meleeRange, meleeWidth);
            meleeCollider.offset = new Vector2(meleeRange / -2, 0);
        }
        else if (directionState == "North")
        {
            meleeCollider.direction = CapsuleDirection2D.Vertical;
            meleeCollider.size = new Vector2(meleeWidth, meleeRange);
            meleeCollider.offset = new Vector2(0, meleeRange / 2);
        }
        else if (directionState == "South")
        {
            meleeCollider.direction = CapsuleDirection2D.Vertical;
            meleeCollider.size = new Vector2(meleeWidth, meleeRange);
            meleeCollider.offset = new Vector2(0, meleeRange / -2);
        }

        // Attack if player hits button and not already attacking
        if (Input.GetButtonDown("Fire1")  && !slashing)
        {
            Attack();
        }
    }

    /// <summary>
    /// Performs the melee attack action
    /// </summary>
    public void Attack()
    {
        slashing = true;
        meleeCollider.enabled = true;
        StartCoroutine(EndAttackAfterSeconds(attackDuration));

    }

    /// <summary>
    /// Disables the attacking state after the attack duration has elapsed.
    /// </summary>
    IEnumerator EndAttackAfterSeconds(float time)
    {
        endSignalSent = true;

        yield return new WaitForSeconds(time);

        slashing = false;
        meleeCollider.enabled = false;

        yield return null;
    }
}
