using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRangedAttack : AttackBase
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Attack()
    {

    }
}
