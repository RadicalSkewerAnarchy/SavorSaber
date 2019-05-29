using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuitantMount : MonoBehaviour
{
    // the fruitant being mounted
    public GameObject thisFruitant;
    private AIData fruitantData;

    // sounds
    public AudioClip mountSound;
    public AudioClip demountSound;

    private AudioSource audioSource;

    // player refs
    private GameObject player;
    private Rigidbody2D playerRB;
    private bool mounted = false;
    private bool mountable = false;
    private bool fruitantEnabled = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();
        fruitantData = thisFruitant.GetComponent<AIData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mountable)
        {
            if (mounted)
            {
                // demount
                if (InputManager.GetButtonDown(Control.Dash))
                {
                    Demount();
                }

                // move player to here
                player.transform.position = this.transform.position;

                fruitantData.rideVector = playerRB.velocity.normalized;
            }
            else
            {
                if (InputManager.GetButtonDown(Control.Dash))
                {
                    Mount();
                }
            }
        }
        else
        {
            // ensure demount
            //if (!fruitantEnabled)
            //{
            //    Demount();
            //}
        }
    }

    void Mount()
    {
        Debug.Log("Mounting");
        // disable fruitant brain
        fruitantData.enabled = false;
        fruitantData.rideVector = new Vector2(0, 0);
        fruitantData.currentProtocol = AIData.Protocols.Ride;
        // enable riding
        player.GetComponent<PlayerController>().riding = true;

        // mounted
        mounted = true;
    }

    void Demount()
    {
        Debug.Log("Demounting");
        // enable fruitant brain
        fruitantData.enabled = true;
        // disable riding
        player.GetComponent<PlayerController>().riding = false;

        // mounted
        mounted = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            mountable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            mountable = false;
        }
    }
}
