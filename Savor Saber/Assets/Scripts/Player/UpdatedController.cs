using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction : int
{
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest,
}


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class UpdatedController : MonoBehaviour
{
    [System.NonSerialized]
    public Direction direction;
    //////
    [SerializeField]
    bool DebugBool = false;
    //////
    [SerializeField]
    [Range(100f,500f)]
    float speed = 100f;
    //////
    /*[SerializeField]
    [Range(5f, 1000f)]
    float deceleration;*/
    //////
    Rigidbody2D rigidBody;
    Animator animatorBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animatorBody = GetComponent<Animator>();
    }

    /*Update is called once per frame*/
    void FixedUpdate()
    {   
        /*Two functions are used here in order to allow easy edition of 
        movement and stopping behavior as well as immediate frame by frame updates*/
        MoveAgent();
        //StopAgent();
        AnimateAgent();
    }

    void MoveAgent()
    {   
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");
        var movementVector = new Vector2(moveHorizontal, moveVertical);
        
        if (movementVector.magnitude > 1)
        {
            rigidBody.velocity = (movementVector/movementVector.magnitude * speed * Time.deltaTime);
        }
        else
        {
            rigidBody.velocity = (movementVector * speed * Time.deltaTime);
        }
        //////
        if (DebugBool) { Debug.Log("MoveAgent finished."); }
        //////
        //https://forum.unity.com/threads/diagonal-movement-speed-to-fast.271703/, comment by cranky. Normalizing
        //the vector adds a weird feel to the player but dividing by the vector's magnitude if it's greater than 1 
        //gives a tight control feel
    }

    void StopAgent()
    {   
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");
        var movementVector = new Vector2(moveHorizontal, moveVertical);
        var negativeDirection = -rigidBody.velocity;
 
        if (movementVector == Vector2.zero && rigidBody.velocity != Vector2.zero)
        {
            rigidBody.velocity = Vector2.zero;
        }
        else
        {
            rigidBody.drag = 1;
        }
        //////
        if (DebugBool) { Debug.Log("StopAgent finished."); }
        //////
    }

    void AnimateAgent()
    {
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");
        var movementVector = new Vector2(moveHorizontal, moveVertical);
        var movementAngle = Vector2.SignedAngle(Vector2.right, movementVector);

        //animatorBody.SetFloat("SpeedX", moveHorizontal);
        //animatorBody.SetFloat("SpeedY", moveVertical);
        
        //calculates angle agent is moving based on right vector(1,0) and agent movementVector
        
        if (movementAngle < 0)
        {
            movementAngle += 360;
        }
        if(movementVector != Vector2.zero)
        {
            animatorBody.SetBool("Moving", true);
            if (movementAngle > 315 || movementAngle < 45)
            {
                direction = Direction.East;
            }
            else if (movementAngle > 135 && movementAngle < 225)
            {
                direction = Direction.West;
            }
            else if(movementAngle > 45 && movementAngle < 135)
            {
                direction = Direction.North;
            }
            else //(movementAngle > 225 && movementAngle < 315)
            {
                direction = Direction.South;
            }
            animatorBody.SetFloat("Direction", (float)direction);
        }
        else
        {
            animatorBody.SetBool("Moving", false);
        }
        //////
        if (DebugBool) { Debug.Log("AnimateAgent finished."); }
        //////
    }
}

// used for basic movement implementaion, https://unity3d.com/learn/tutorials/projects/2d-ufo-tutorial/controlling-player