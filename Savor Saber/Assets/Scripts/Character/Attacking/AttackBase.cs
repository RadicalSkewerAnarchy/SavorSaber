﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class AttackBase : MonoBehaviour
{
    /// <summary> To prevent attack action while still attacking (unless CanBeCancelled)
    /// Also checked by player inventory - if it used a ranged attack, 
    /// clear the current skewer. </summary>
    public bool Attacking { get; protected set; }
    /// <summary> If this is true, other attacks with equal or higher priority can cancel out of this one</summary>
    public bool CanBeCanceled { get; protected set; }
    /// <summary> Attacks can only cancel attacks with equal or lower priority </summary>
    public int CancelPriority = 0;
    /// <summary> The attacks this attack shouldn't overlap (will always include self)
    /// Set in the start of any derived class using GetComponents<AttackBase>() </summary>
    protected AttackBase[] dependecies;

    protected AttackBase GetActiveAttack()
    {
        return dependecies.FirstOrDefault((at) => at.Attacking);
    }
    public abstract void Attack();
    public virtual void Cancel()
    {
        StopAllCoroutines();
        Attacking = false;
        CanBeCanceled = false;
    }
}