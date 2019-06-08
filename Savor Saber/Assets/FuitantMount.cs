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
    private PlayerController controller;
    private SpriteRenderer fruitantRenderer;
    private MonsterController fruitantController;
    private SpriteRenderer playerRenderer;
    private PlayerData playerData;
    public bool mounted = false;
    private bool mountable = false;
    private bool fruitantEnabled = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();
        playerRenderer = player.GetComponent<SpriteRenderer>();
        playerData = player.GetComponent<PlayerData>();
        fruitantData = thisFruitant.GetComponent<AIData>();
        fruitantController = thisFruitant.GetComponent<MonsterController>();
        fruitantRenderer = thisFruitant.GetComponent<SpriteRenderer>();

        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (mounted)
        {
            if (fruitantData.health <= 0)
            {
                Demount();
            }
        }
    }

    void LateUpdate()
    {
        if (mountable)
        {
            if (mounted)
            {
                // if player dies, demount
                if (playerData.health <= 0)
                {
                    Demount();
                    return;
                }

                // if in cutscene, dmeount
                if (EventTrigger.InCutscene)
                {
                    Demount();
                    return;
                }

                // demount
                if (InputManager.GetButtonDown(Control.Dash, InputAxis.Dash))
                {
                    Demount();
                    return;
                }

                // move the fruitant
                fruitantData.rideVector = controller.GetMovementVector();

                // move player to here
                player.transform.position = this.transform.position;
                playerRenderer.flipX = (fruitantController.invert ? fruitantRenderer.flipX : !fruitantRenderer.flipX);

            }
            else
            {
                if (InputManager.GetButtonDown(Control.Dash, InputAxis.Dash) && playerData.health > 0)
                {
                    Mount();
                    return;
                }
            }
        }
    }

    void Mount()
    {
        Debug.Log("Mounting");
        audioSource.clip = mountSound;
        audioSource.Play();

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

    public void Demount()
    {
        Debug.Log("Demounting");
        audioSource.clip = demountSound;
        audioSource.Play();

        // set fruitant data
        fruitantData.currentProtocol = AIData.Protocols.Lazy;

        // disable riding
        controller.riding = false;
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), thisFruitant.GetComponent<Collider2D>(), false);

        // change player layering
        playerRenderer.sortingLayerName = "Objects";
        playerRenderer.flipX = false;

        // set player position
        player.transform.position = thisFruitant.transform.position;

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
            if (mounted)
                Demount();
        }
    }
}
