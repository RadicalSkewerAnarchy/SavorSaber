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

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
        dependecies = GetComponents<AttackBase>();
        animator = GetComponent<Animator>();

        //has to have either a monster controller or player controller
        playerController = GetComponent<UpdatedController>();
        if (playerController == null)
            monsterController = GetComponent<MonsterMovement>();
    }

    public override void Attack()
    {
        //animation stuff
        animator.Play(attackName);

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
}
