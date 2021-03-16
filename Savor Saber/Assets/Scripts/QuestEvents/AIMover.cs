using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMover : PoweredObject
{
    //public bool useTrueAI = false; //whether the AI being moved has an actual brain or needs to be "puppeted"
    public float dumbAISpeed = 1; //how fast a "puppeted" AI should move
    public Direction dumbAIDirection;

    private bool playerNearby = false;
    [SerializeField]
    private Commander AICommander;
    [SerializeField]
    private GameObject escortTarget;
    [SerializeField]
    private GameObject escortBoundary;
    [SerializeField]
    private float boundaryRotationSpeed;
    private AIData escortData;
    private Rigidbody2D escortBody;
    private Animator escortAnimator;

    public GameObject waypoint;

    private BasicCharacterController panaceaController;

    // Start is called before the first frame update
    void Start()
    {
        if (AICommander == null)
            AICommander = GameObject.Find("Gaia").GetComponent<Commander>();

        escortData = escortTarget.GetComponent<AIData>();
        escortBody = escortTarget.GetComponent<Rigidbody2D>();
        escortAnimator = escortTarget.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active && playerNearby)
        {
            //Debug.Log("moving dumb ai");
            escortBody.transform.position += ((Vector3)Vector2.right * dumbAISpeed * Time.deltaTime);
            //Animate();
        }
    }

    public override void TurnOn()
    {
        base.TurnOn();
        if (playerNearby)
            Animate(true); //if the player is nearby when the mover is turned on, remember to restart the animation!
    }

    public override void ShutOff()
    {
        base.ShutOff();
        Animate(false); //when shutting off, be sure to stop the target's animation, even if the player is nearby
    }

    //when the player enters, set the flag that they're nearby. As long as we're active, movement will occur, so play the animation.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("AI Mover: Player nearby");
            playerNearby = true;
            if(active)
                Animate(true);

        }

    }

    //When the player leaves, set the flag to false so movement stops. Animation will need to be stopped too.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("AI Mover: Player leaving");
            playerNearby = false;
            Animate(false);
        }
    }
    
    /*
    public void StartMotion()
    {
        Debug.Log("Escort quest: Starting motion");
        AICommander.Command(escortTarget, AIData.Protocols.Chase, waypoint, waypoint.transform.position);
        escortData.Speed = 1.75f;
    }
    public void StopMotion()
    {
        Debug.Log("Escort quest: Stopping motion");
        AICommander.Command(escortTarget, AIData.Protocols.Lazy, waypoint, waypoint.transform.position);
        escortData.Speed = 0;

    }
    */

    private void ForceIdle()
    {
        escortAnimator.SetBool("Moving", false);
       // escortAnimator.Play("Idle");

    }

    private void Animate(bool moving)
    {
        escortAnimator.SetBool("Moving", moving);
        escortAnimator.SetFloat("Direction", (float)dumbAIDirection);
    }

}
