using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AIData))]
[RequireComponent(typeof(Animator))]

public class MonsterBehavior : MonoBehaviour
{
    Rigidbody2D RigidBody;
    Animator AnimatorBody;
    AIData AiData;

    // Some behaviors go for a certain amount of time.
    // timer is complete when < 0
    public float ActionTimer;
    public float ActionTimerReset;
    public float ActionTimerVariance;

    /// <summary>
    /// moved to AIData
    /// </summary>
    //public Vector2 TargetPoint;
    //public float Speed = 0;
    private void Start()
    {
        AiData = GetComponent<AIData>();
        AnimatorBody = GetComponent<Animator>();
        RigidBody = GetComponent<Rigidbody2D>();

        ActionTimer = -1f;
        ActionTimerReset = 5f;
        ActionTimerVariance = 2f;
    }

    /// <summary>
    /// These actions return a Boolean to verify their completion.
    /// These actions may modify the agents position.
    /// </summary>

    public bool Idle()
    {
        //Debug.Log("I am Idle");
        //Debug.Log(ActionTimer);
        TriggerAnimation("Idle");
        // if done being idle, reset and return true
        if (ActionTimer < 0)
        {
            return true;
        }
        else
        {
            ActionTimer -= Time.deltaTime;
            return false;
        }
    }

    public bool MoveTo(Vector2 target, float speed)
    {
        //Debug.Log("I am Chase at " + speed + "mph");
        // Turn Green
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        LoopAnimation("Move");
        CancelTrigger();
        return true;
    }

    public bool MoveFrom(Vector2 target, float speed)
    {
        //Debug.Log("I am Flee");
        //  Turn Blue
        transform.position = Vector2.MoveTowards(transform.position, target, -1 *speed * Time.deltaTime);
        float movementAmount = -speed * Time.deltaTime;
        TriggerAnimation("Move");
        CancelTrigger();
        return true;
    }

    public bool Feed(Vector2 target, float speed)
    {
        TriggerAnimation("Feed");
        return true;
    }

    public bool Attack(Vector2 target, float speed)
    {
        //Debug.Log("I am Attack");
        TriggerAnimation("Melee");
        return true;
    }

    public bool Ranged(Vector2 target, float speed)
    {
        //Debug.Log("I am Attack");
        TriggerAnimation("Ranged");
        return true;
    }

    public bool Socialize(Vector2 target, float speed)
    {
        LoopAnimation("Socialize");
        return true;
    }

    public void TriggerAnimation(string name)
    {
        if (AnimatorBody.GetCurrentAnimatorStateInfo(1).IsName(name))
            return;
        AnimatorBody.Play(name);
        AnimatorBody.SetTrigger(name);
    }
    public void LoopAnimation(string name)
    {
        if (AnimatorBody.GetCurrentAnimatorStateInfo(0).IsName(name))
            return;
        AnimatorBody.SetTrigger(name);
    }
    public void CancelTrigger()
    {
        AnimatorBody.SetTrigger("Cancel");
    }
}
