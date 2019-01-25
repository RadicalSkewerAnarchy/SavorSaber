using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AIData))]

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
        Debug.Log(ActionTimer);
        GetComponent<SpriteRenderer>().color = new Color(255,255,255);
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
        //GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
        var left = false;
       
        var current = new Vector2(transform.position.x, transform.position.y);
        if (Vector2.Distance(current, target) < speed * Time.deltaTime)
        {
            return false;
        }                
        else
        {
            target = (target - current);
            target = Vector2.ClampMagnitude(target, speed * Time.deltaTime);
            left = (target.x < 0) ? true : false;
            transform.Translate(target);
        }
        

        return true;
    }

    public bool MoveFrom(Vector2 target, float speed)
    {

        var left = false;

        var current = new Vector2(transform.position.x, transform.position.y);
        if (Vector2.Distance(current, target) < speed * Time.deltaTime)
        {
            return false;
        }
        else
        {
            target = (target - current);
            target = Vector2.ClampMagnitude(target, speed * Time.deltaTime);
            left = (target.x < 0) ? true : false;
            transform.Translate(-1*target);
        }


        return true;       
    }

    public bool Feed(Vector2 target, float speed)
    {
        return true;
    }

    public bool Attack(Vector2 target, float speed)
    {
        //Debug.Log("I am Attack");
        // Turn Red
        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        return true;
    }

    public bool Socialize(Vector2 target, float speed)
    {
        return true;
    }

    
}
