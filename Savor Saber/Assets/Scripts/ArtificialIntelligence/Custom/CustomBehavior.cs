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
        if (type == Type.Trigger)
        {
            var animOverride = new AnimatorOverrideController(animatorBody.runtimeAnimatorController);
            animOverride["Custom Trigger"] = anim;
            animatorBody.SetTrigger("Trigger");
            animatorBody.SetTrigger("Custom");
        }
        else if (type == Type.Loop)
        {
            var animOverride = new AnimatorOverrideController(animatorBody.runtimeAnimatorController);
            animOverride["Custom Loop"] = anim;
            animatorBody.SetTrigger("Loop");
            animatorBody.SetTrigger("Custom");
        }
    }
    protected virtual void DoBehavior()
    {

    }
}
