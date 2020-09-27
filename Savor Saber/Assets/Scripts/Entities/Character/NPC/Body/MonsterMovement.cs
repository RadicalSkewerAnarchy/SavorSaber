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
    public Direction direction;
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
        {
            //get movement vector 
            float moveHorizontal = TargetPoint.x - transform.position.x;
            float moveVertical = TargetPoint.y - transform.position.y;
            Vector2 movementVector = new Vector2(moveHorizontal, moveVertical);
            float movementAngle = Vector2.SignedAngle(Vector2.right, movementVector);

            if (movementAngle < 0)
                movementAngle += 360;

            direction = Direction.East.Offset((int)(movementAngle / 45));

            //begin movement
            transform.position = Vector2.MoveTowards(transform.position, TargetPoint, Speed * Time.deltaTime);


        }


            


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
