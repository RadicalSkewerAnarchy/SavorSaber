using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MonsterBehavior))]

public class CustomBehavior : CustomProtocol
{
    public enum Type
    {
        Trigger,
        Loop,
        Neither,
    }

    public AnimationClip anim;
    public Type type = Type.Loop;
    protected Animator animatorBody;

    public void Start()
    {
        animatorBody = GetComponent<Animator>();
    }

    public override void Invoke()
    {
        InvokeBehavior();
    }
    public bool InvokeBehavior()
    {
        if (type == Type.Trigger)
        {
            if(!(animatorBody.GetCurrentAnimatorStateInfo(1).IsName("CustomTrigger")))
            {
                var animOverride = animatorBody.runtimeAnimatorController as AnimatorOverrideController;
                animOverride["CustomTrigger"] = anim;
                animatorBody.SetTrigger("CustomTrigger");
            }
        }
        else if (type == Type.Loop)
        {
            if (!(animatorBody.GetCurrentAnimatorStateInfo(0).IsName("CustomLoop")))
            {
                var animOverride = animatorBody.runtimeAnimatorController as AnimatorOverrideController;
                animOverride["CustomLoop"] = anim;
                animatorBody.SetTrigger("CustomLoop");
            }
        }
        return DoBehavior();
    }
    protected virtual bool DoBehavior()
    {
        return true;
    }
}
