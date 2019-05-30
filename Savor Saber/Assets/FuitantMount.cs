﻿using System.Collections;
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
    private PlayerController controller;
    private SpriteRenderer fruitantRenderer;
    private SpriteRenderer playerRenderer;
    private bool mounted = false;
    private bool mountable = false;
    private bool fruitantEnabled = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();
        playerRenderer = player.GetComponent<SpriteRenderer>();
        fruitantData = thisFruitant.GetComponent<AIData>();
        fruitantRenderer = thisFruitant.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (mountable)
        {
            if (mounted)
            {
                // demount
                if (InputManager.GetButtonDown(Control.Dash))
                {
                    Demount();
                    return;
                }

                // move the fruitant
                fruitantData.rideVector = controller.GetMovementVector();

                // move player to here
                player.transform.position = this.transform.position;
                playerRenderer.flipX = fruitantRenderer.flipX;

            }
            else
            {
                if (InputManager.GetButtonDown(Control.Dash))
                {
                    Mount();
                    return;
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
        // set fruitant data
        fruitantData.rideVector = new Vector2(0, 0);
        fruitantData.currentProtocol = AIData.Protocols.Ride;

        // enable riding
        controller.riding = true;
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), thisFruitant.GetComponent<Collider2D>(), true);

        // change player layering
        playerRenderer.sortingLayerName = "AboveObjects";

        // mounted
        mounted = true;
    }

    void Demount()
    {
        Debug.Log("Demounting");

        // set fruitant data
        fruitantData.currentProtocol = AIData.Protocols.Lazy;

        // disable riding
        controller.riding = false;
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), thisFruitant.GetComponent<Collider2D>(), false);

        // change player layering
        playerRenderer.sortingLayerName = "Objects";
        playerRenderer.flipX = false;

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
