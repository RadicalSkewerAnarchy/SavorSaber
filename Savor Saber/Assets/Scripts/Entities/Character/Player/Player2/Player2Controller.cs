using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player2Controller : EntityController
{
    public AudioClip emoteSfx;
    private PlaySFX sfxPlayer;
    public GameObject emoteSignal;
    private GameObject player;

    [SerializeField]
    private Direction _direction;
    public override Direction Direction
    {
        get => _direction;
        set
        {
            _direction = value;
            animatorBody.SetFloat("Direction", (float)value);
        }
    }

    [System.NonSerialized]
    [Range(0f, 1f)]
    public float speedMod = 1f;
    [System.NonSerialized]
    public bool freezeDirection = false;
    [SerializeField]
    bool DebugBool = false;
    [SerializeField]
    [Range(100f, 500f)]
    float speed = 100f;
    [SerializeField]
    [Range(0f, 500f)]
    float runSpeed = 100f;

    Rigidbody2D rigidBody;
    Animator animatorBody;
    DialogData dialogData;
    /// <summary> The Squared magnitude of the movement vector from last frame
    /// Used to determine if soma is slowing down </summary>
    private float lastSqrMagnitude = 0;

    #region Running Fields
    private bool running = false;
    private Coroutine run;
    private float currRunSpeed;
    public float runTimeBuffer;
    public float accelerationTime;
    public float maxSpeed;
    public float accelrationAmount;
    #endregion

    private AudioSource sfxSource;

    void Awake()
    {      
        rigidBody = GetComponent<Rigidbody2D>();
        animatorBody = GetComponent<Animator>();
        Direction = _direction;
        currRunSpeed = runSpeed;
        dialogData = GetComponent<DialogData>();
        sfxSource = GetComponent<AudioSource>();
        sfxPlayer = GetComponent<PlaySFX>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Detect non-movement input every fram so input isn't dropped
    private void Update()
    {
        if (running && GetMovementVector().sqrMagnitude == 0)
            StopRunning();
        if (Input.GetKeyDown(KeyCode.Joystick2Button0))
        {
            var sig = Instantiate(emoteSignal);
            sig.transform.position = transform.position + new Vector3(0, 20, 0);
            var sigApp = sig.GetComponent<SignalApplication>();
            if(sigApp != null)
                sigApp.SignalAnimator(1, "Friendliness", 1, sig, gameObject, false);
            sfxPlayer.Play(emoteSfx);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick2Button2))
        {
            transform.position = player.transform.position;
        }
        else if (Input.GetKeyDown(KeyCode.Joystick2Button7))
        {
            Player2Input.instance.playerTwoActive = false;
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        MoveAgent();
        AnimateAgent();
    }

    public void Stop()
    {
        StopRunning();
        rigidBody.velocity = Vector2.zero;
        animatorBody.SetBool("Moving", false);
        animatorBody.SetBool("Running", false);
    }

    private IEnumerator runCR()
    {
        Debug.Log("starting run");
        running = true;
        currRunSpeed = runSpeed;
        yield return new WaitForSeconds(runTimeBuffer);
        while(currRunSpeed < maxSpeed)
        {
            yield return new WaitForFixedUpdate();
            currRunSpeed += accelrationAmount;
        }
        run = null;
    }

    private void StopRunning()
    {
        Debug.Log("stopping run");
        running = false;
        if(run != null)
        {
            StopCoroutine(run);
            run = null;           
        }
        rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
    }

    private Vector2 GetMovementVector()
    {
        var moveHorizontal = Input.GetAxis("Player2Horizontal");
        var moveVertical = Input.GetAxis("Player2Vertical");
        return new Vector2(moveHorizontal, moveVertical);
    }

    void MoveAgent()
    {
        if(dialogData.inConversation)
        {
            StopRunning();
            rigidBody.velocity = Vector2.zero;
        }
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
            }
            if (running && (movementVector.magnitude < 0.1 || speedMod < 0.5f))
                StopRunning();
        }
        //////
        if (DebugBool) { Debug.Log("MoveAgent finished."); }
        //////
        //https://forum.unity.com/threads/diagonal-movement-speed-to-fast.271703/, comment by cranky. Normalizing
        //the vector adds a weird feel to the player but dividing by the vector's magnitude if it's greater than 1
        //gives a tight control feel
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
                Direction = DirectionMethods.FromVec2(movementVector);
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
