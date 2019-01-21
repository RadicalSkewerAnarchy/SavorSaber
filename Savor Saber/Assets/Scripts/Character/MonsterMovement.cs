using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
//[RequireComponent(typeof(Animator))]

public class MonsterMovement : MonoBehaviour
{
    Rigidbody2D RigidBody;
    Animator AnimatorBody;
    public Vector2 TargetPoint;
    //GameObject Target;
    public float Speed = 0;

    enum Directions { East, NorthEast, North, NorthWest, West, SouthWest, South, SouthEast }

    // Start is called before the first frame update
    void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        //AnimatorBody = GetComponent<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //UpdateDirection();
        //MoveAgent();
        //AnimateAgent();
    }

    public void MoveAgent()
    {
        if(TargetPoint != null)
            transform.position = Vector2.MoveTowards(transform.position, TargetPoint, Speed * Time.deltaTime);
    }

    void AnimateAgent()
    {

    }

    public void UpdateSpeed(float newSpeed)
    {
        Speed = newSpeed;
    }

    public void UpdateDirection(Vector2 newTargetPoint)
    {
        TargetPoint = newTargetPoint;
    }
    public void UpdateDirection(GameObject target)
    {
        TargetPoint = target.transform.position;
    }
}
