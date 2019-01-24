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
    /// <summary>
    /// moved to AIData
    /// </summary>    
    //public Vector2 TargetPoint;
    //public float Speed = 0;
    private void Start()
    {
        AiData = GetComponent<AIData>();
        AnimatorBody = GetComponent<Animator>();
    }

    public void Idle()
    {
        Debug.Log("I am Idle");
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        AnimatorBody.SetTrigger("Reset");
    }

    public void MoveTo(Vector2 target, float speed)
    {
        Debug.Log("I am Chase at " + speed + "mph");
        // Turn Green
        GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        // Amount the monster moved (change definition if pathfinding etc is implemented)
        float movementAmount = speed * Time.deltaTime;
        AnimatorBody.SetBool("Moving", movementAmount != 0);
    }

    public void MoveFrom(Vector2 target, float speed)
    {
        Debug.Log("I am Flee");
        //  Turn Blue
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);
        transform.position = Vector2.MoveTowards(transform.position, target, -speed * Time.deltaTime);
        // Amount the monster moved (change definition if pathfinding etc is implemented)
        float movementAmount = -speed * Time.deltaTime;
        AnimatorBody.SetBool("Moving", movementAmount != 0);
    }

    public void Feed(Vector2 target, float speed)
    {
        TriggerAnimation("Feed");
    }

    public void Attack(Vector2 target, float speed)
    {
        Debug.Log("I am Attack");
        // Turn Red
        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        TriggerAnimation("Melee");
    }

    public void Socialize(Vector2 target, float speed)
    {
        LoopAnimation("Socialize");
    }

    public void TriggerAnimation(string name)
    {
        AnimatorBody.SetTrigger("Trigger");
        AnimatorBody.SetTrigger(name);
    }

    public void LoopAnimation(string name)
    {
        AnimatorBody.SetTrigger("Loop");
        AnimatorBody.SetTrigger(name);
    }
}
