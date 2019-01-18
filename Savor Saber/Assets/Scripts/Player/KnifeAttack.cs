using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UpdatedController))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class KnifeAttack : MeleeAttack
{

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<UpdatedController>();
        meleeCollider = GetComponent<CapsuleCollider2D>();

        //Set attack data values
        meleeDamage = 1f;
        meleeRange = 2f;
        meleeWidth = 1f;
        attackDuration = 0.5f;
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
    /// Performs the melee attack action.
    /// </summary>
    public override void Attack()
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
