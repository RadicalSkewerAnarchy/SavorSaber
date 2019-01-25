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
        var animOverride = animatorBody.runtimeAnimatorController as AnimatorOverrideController;
        if (animOverride["Custom"] != anim || !(animatorBody.GetCurrentAnimatorStateInfo(0).IsName("Custom")))
        {
            animOverride["Custom"] = anim;
            animatorBody.Play("Custom");
        }
        return DoBehavior();
    }
    protected virtual bool DoBehavior()
    {
        return true;
    }
}
