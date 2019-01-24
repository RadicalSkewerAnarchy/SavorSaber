using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction : int
{   
    East,
    NorthEast,
    North,
    NorthWest,
    West,
    SouthWest,
    South,
    SouthEast,
}


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class UpdatedController : MonoBehaviour
{
    [System.NonSerialized]
    public Direction direction;
    //////
    [System.NonSerialized]
    [Range(0f, 1f)]
    public float speedMod = 1f;
    //////
    [System.NonSerialized]
    public bool freezeDirection = false;
    //////
    [SerializeField]
    bool DebugBool = false;
    //////
    [SerializeField]
    [Range(100f,500f)]
    float speed = 100f;
    //////
    [SerializeField]
    [Range(100f, 500f)]
    float runSpeed = 100f;
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
        bool running = Input.GetButton("Run");
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");
        var movementVector = new Vector2(moveHorizontal, moveVertical);
        var modSpeed = (running ? runSpeed : speed) * speedMod;

        if (movementVector.magnitude > 1)
        {
            rigidBody.velocity = (movementVector/movementVector.magnitude * modSpeed * Time.deltaTime);
        }
        else
        {
            rigidBody.velocity = (movementVector * modSpeed * Time.deltaTime);
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
        bool running = Input.GetButton("Run");
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");
        var movementVector = new Vector2(moveHorizontal, moveVertical);
  
        if(movementVector != Vector2.zero)
        {
            animatorBody.SetBool("Moving", true);
            animatorBody.SetBool("Running", running);
            //calculates angle based on standard offset from East (1,0)
            if (!freezeDirection)
            {
                var movementAngle = Vector2.SignedAngle(Vector2.right, movementVector);
                if (movementAngle < 0)
                    movementAngle += 360;
                direction = Direction.East.Offset((int)(movementAngle / 45));
                animatorBody.SetFloat("Direction", (float)direction);
            }
        }
        else
        {
            animatorBody.SetBool("Moving", false);
            animatorBody.SetBool("Running", false);
        }
        //////
        if (DebugBool) { Debug.Log("AnimateAgent finished."); }
        //////
    }

    public void Slow()
    {

    }
}

// used for basic movement implementaion, https://unity3d.com/learn/tutorials/projects/2d-ufo-tutorial/controlling-player