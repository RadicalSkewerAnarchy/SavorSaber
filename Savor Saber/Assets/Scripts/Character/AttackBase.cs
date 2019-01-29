using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBase : MonoBehaviour
{
    /// <summary> To prevent attack action while still attacking.
    /// Also checked by player inventory - if it used a ranged attack, 
    /// clear the current skewer. </summary>
    public bool Attacking { get; protected set; }
    /// <summary> If this is true, other attacks can cancel out of this one</summary>
    public bool CanCancel { get; protected set; }
    /// <summary> The attacks this attack shouldn't overlap (will always include self)
    /// Set in the start of any derived class using GetComponents<AttackBase>() </summary>
    protected AttackBase[] dependecies;

    public abstract void Attack();
    public virtual void Cancel()
    {
        StopAllCoroutines();
        Attacking = false;
        CanCancel = false;
    }
}
