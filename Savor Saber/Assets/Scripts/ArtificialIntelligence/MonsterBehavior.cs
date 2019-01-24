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
    /// <summary>
    /// moved to AIData
    /// </summary>    
    //public Vector2 TargetPoint;
    //public float Speed = 0;
    private void Start()
    {
        AiData = GetComponent<AIData>();
        RigidBody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// These actions return a Boolean to verify their completion.
    /// These actions may modify the agents position.
    /// </summary>

    public bool Idle()
    {
        Debug.Log("I am Idle");
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        return true;
    }

    public bool MoveTo(Vector2 target, float speed)
    {
        Debug.Log("I am Chase at " + speed + "mph");
        // Turn Green
        GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        return true;
    }

    public bool MoveFrom(Vector2 target, float speed)
    {
        Debug.Log("I am Flee");
        //  Turn Blue
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);
        transform.position = Vector2.MoveTowards(transform.position, target, -1 *speed * Time.deltaTime);
        return true;
    }

    public bool Feed(Vector2 target, float speed)
    {
        return true;
    }

    public bool Attack(Vector2 target, float speed)
    {
        Debug.Log("I am Attack");
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
