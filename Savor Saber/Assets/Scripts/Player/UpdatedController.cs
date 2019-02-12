using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

static class DirectionMethods
{
    public static bool IsCardinal(this Direction d) => (int)d % 2 == 0;
    public static Direction FromVec2(Vector2 vec)
    {
        var movementAngle = Vector2.SignedAngle(Vector2.right, vec);
        if (movementAngle < 0)
            movementAngle += 360;
        return Direction.East.Offset(Mathf.RoundToInt(movementAngle / 45));
    }
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
    [Range(100f, 500f)]
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
    DialogData dialogData;
    /// <summary> The Squared magnitude of the movement vector from last frame
    /// Used to determine if soma is slowing down </summary>
    private float lastSqrMagnitude = 0;

    public float dashTime;
    public AnimationCurve dashSpeed;
    public float dashScale;
    public int maxDashes = 3;
    private int currDashes;
    private bool dashing = false;
    private float dashCurrTime = 0;
    private Vector2 dashVector;
    public float doubleTapTime;
    private Dictionary<Control, float> doubleTapTrackers;
    private Control[] keys;
    private bool running = false;
    private Coroutine rechargeDashes;
    public float dashRechargeTime = 1f;
    public UnityEngine.UI.Text debugText;

    private Coroutine run;
    private float currRunSpeed;
    public float runTimeBuffer;
    public float accelerationTime;
    public float maxSpeed;
    public float accelrationAmount;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animatorBody = GetComponent<Animator>();
        doubleTapTrackers = new Dictionary<Control, float>()
        {
            { Control.Up, 0 },
            { Control.Down, 0 },
            { Control.Left, 0 },
            { Control.Right, 0 },
        };
        keys = new Control[doubleTapTrackers.Count];
        doubleTapTrackers.Keys.CopyTo(keys, 0);
        currDashes = maxDashes;
        currRunSpeed = runSpeed;
        dialogData = GetComponent<DialogData>();
    }

    // Detect non-movement input every fram so input isn't dropped
    private void Update()
    {
        if (InputManager.GetButtonDown(Control.Dash, InputAxis.Dash))
            StartDash();
        else
            CheckForDoubleTaps();
        if (running && InputManager.GetAxis(InputAxis.Horizontal) == 0 && InputManager.GetAxis(InputAxis.Vertical) == 0)
            StopRunning();
    }

    void FixedUpdate()
    {
        /*Two functions are used here in order to allow easy edition of
        movement and stopping behavior as well as immediate frame by frame updates*/
        MoveAgent();
        //StopAgent();
        AnimateAgent();
    }

    private void CheckForDoubleTaps()
    {
        foreach (var key in keys)
        {
            if (InputManager.GetButtonDown(key))
            {
                if (doubleTapTrackers[key] > 0)
                {
                    doubleTapTrackers[key] = 0;
                    if (dashing && dashCurrTime <= Time.fixedDeltaTime)
                    {
                        var currDashDir = DirectionMethods.FromVec2(dashVector);
                        var moveVec = GetMovementVector();
                        var newDashDir = DirectionMethods.FromVec2(moveVec);
                        if (currDashDir.IsCardinal() && !newDashDir.IsCardinal())
                        {
                            StartDash(false);
                            break;
                        }
                    }
                    StartDash();
                    break;
                }
                else
                    doubleTapTrackers[key] = doubleTapTime;
            }
            else if (doubleTapTrackers[key] > 0)
                doubleTapTrackers[key] -= Time.deltaTime;
        }
    }

    private void StartDash(bool decrement = true)
    {
        if (decrement && currDashes <= 0)
            return;
        dashVector = GetMovementVector();
        if (dashVector.SqrMagnitude() == 0)
            return;
        if (decrement)
            currDashes--;
        if(debugText != null)
            debugText.text = "Dashes: " + currDashes;
        dashing = true;
        dashCurrTime = 0;
        freezeDirection = true;
        rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        animatorBody.SetBool("Dashing", true);
        if(rechargeDashes != null)
            StopCoroutine(rechargeDashes);
    }

    private void Dash()
    {
        var dSpeed = dashSpeed.Evaluate(dashCurrTime / dashTime) * dashScale * speedMod;
        rigidBody.velocity = (dashVector / dashVector.magnitude * dSpeed * Time.fixedDeltaTime);
        dashCurrTime += Time.fixedDeltaTime;
        if (dashCurrTime >= dashTime)
            StopDash();
    }

    private void StopDash()
    {
        rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        freezeDirection = false;
        dashCurrTime = 0;
        dashing = false;
        animatorBody.SetBool("Dashing", false);
        run = StartCoroutine(runCR());
        rechargeDashes = StartCoroutine(rechargeDashesCR());
        dashVector = Vector2.zero;
    }

    private IEnumerator rechargeDashesCR()
    {
        while(currDashes < maxDashes)
        {
            yield return new WaitForSeconds(dashRechargeTime);
            currDashes++;
            if(debugText != null)
                debugText.text = "Dashes: " + currDashes;
        }
    }

    private IEnumerator runCR()
    {
        running = true;
        currRunSpeed = runSpeed;
        yield return new WaitForSeconds(runTimeBuffer);
        while(currRunSpeed < maxSpeed)
        {
            yield return new WaitForFixedUpdate();
            currRunSpeed += accelrationAmount;
        }
    }

    private void StopRunning()
    {
        running = false;
        if(run != null)
            StopCoroutine(run);
    }

    private Vector2 GetMovementVector()
    {
        var moveHorizontal = InputManager.GetAxis(InputAxis.Horizontal);
        var moveVertical = InputManager.GetAxis(InputAxis.Vertical);
        return new Vector2(moveHorizontal, moveVertical);
    }

    void MoveAgent()
    {
        if(dialogData.inConversation)
        {
            StopDash();
            StopRunning();
            rigidBody.velocity = Vector2.zero;
        }
        else if (dashing)
            Dash();
        else
        {
            var movementVector = GetMovementVector();
            var modSpeed = (running ? currRunSpeed : speed) * speedMod;
            if (movementVector.magnitude > 1)
            {
                rigidBody.velocity = (movementVector / movementVector.magnitude * modSpeed * Time.fixedDeltaTime);
            }
            else
            {
                rigidBody.velocity = (movementVector * modSpeed * Time.fixedDeltaTime);
                StopRunning();
            }
            if (running && movementVector.magnitude < 0.25)
                StopRunning();
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
        if(dialogData.inConversation)
        {
            animatorBody.SetBool("Moving", false);
            animatorBody.SetBool("Running", false);
            return;
        }
        var movementVector = GetMovementVector();
        float clampedMagnitude = Mathf.Clamp01(movementVector.sqrMagnitude);
        if(movementVector != Vector2.zero)
        {
            animatorBody.SetBool("Moving", true);
            animatorBody.SetBool("Running", running);
            //calculates angle based on standard offset from East (1,0)
            if (!freezeDirection && clampedMagnitude >= lastSqrMagnitude)
            {
                direction = DirectionMethods.FromVec2(movementVector);
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
