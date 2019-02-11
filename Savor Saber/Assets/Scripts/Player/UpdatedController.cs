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
    [Range(0f, 500f)]
    float runSpeed = 100f;
    //////
    /*[SerializeField]
    [Range(5f, 1000f)]
    float deceleration;*/
    //////
    Rigidbody2D rigidBody;
    Animator animatorBody;
    /// <summary> The Squared magnitude of the movement vector from last frame
    /// Used to determine if soma is slowing down </summary>
    private float lastSqrMagnitude = 0;

    public float dashTime;
    public AnimationCurve dashSpeed;
    public float dashScale;
    private bool dashing = false;
    private float dashCurrTime = 0;
    private Vector2 dashVector;


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
        if(InputManager.GetButtonDown(Control.Cook))
        {
            if (!dashing || true)
            {
                var h = InputManager.GetAxis(InputAxis.Horizontal);
                var v = InputManager.GetAxis(InputAxis.Vertical);
                dashVector = new Vector2(h, v);
                if (dashVector.SqrMagnitude() == 0)
                    return;
                dashing = true;
                dashCurrTime = 0;
                freezeDirection = true;
                rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                animatorBody.SetBool("Dashing", true);
            }
        }

        if(dashing)
        {
            var dSpeed = dashSpeed.Evaluate(dashCurrTime / dashTime) * dashScale * speedMod;
            Debug.Log(dSpeed);
            rigidBody.velocity = (dashVector / dashVector.magnitude * dSpeed * Time.fixedDeltaTime);
            dashCurrTime += Time.fixedDeltaTime;
            if (dashCurrTime >= dashTime)
            {
                rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
                freezeDirection = false;
                dashCurrTime = 0;
                dashing = false;
                animatorBody.SetBool("Dashing", false);
            }
        }
        else
        {
            var moveHorizontal = InputManager.GetAxis(InputAxis.Horizontal);
            var moveVertical = InputManager.GetAxis(InputAxis.Vertical);
            var movementVector = new Vector2(moveHorizontal, moveVertical);
            var modSpeed = speed * speedMod;
            if (movementVector.magnitude > 1)
            {
                rigidBody.velocity = (movementVector / movementVector.magnitude * modSpeed * Time.fixedDeltaTime);
            }
            else
            {
                rigidBody.velocity = (movementVector * modSpeed * Time.fixedDeltaTime);
            }
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
        var moveHorizontal = InputManager.GetAxis(InputAxis.Horizontal);
        var moveVertical = InputManager.GetAxis(InputAxis.Vertical);
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
        bool running = InputManager.GetButton(Control.Dash);
        var moveHorizontal = InputManager.GetAxis(InputAxis.Horizontal);
        var moveVertical = InputManager.GetAxis(InputAxis.Vertical);
        var movementVector = new Vector2(moveHorizontal, moveVertical);
        float clampedMagnitude = Mathf.Clamp01(movementVector.sqrMagnitude);
        if(movementVector != Vector2.zero)
        {
            animatorBody.SetBool("Moving", true);
            animatorBody.SetBool("Running", running);
            //calculates angle based on standard offset from East (1,0)
            if (!freezeDirection && clampedMagnitude >= lastSqrMagnitude)
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
        lastSqrMagnitude = clampedMagnitude;
        //////
        if (DebugBool) { Debug.Log("AnimateAgent finished."); }
        //////
    }
}
// used for basic movement implementaion, https://unity3d.com/learn/tutorials/projects/2d-ufo-tutorial/controlling-player