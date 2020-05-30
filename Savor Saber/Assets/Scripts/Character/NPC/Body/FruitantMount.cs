using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitantMount : MonoBehaviour
{
    // the fruitant being mounted
    public GameObject thisFruitant;
    private AIData fruitantData;

    // sounds
    public AudioClip mountSound;
    public AudioClip demountSound;

    public AudioSource audioSource;

    // player refs
    public GameObject player;
    public PlayerController controller;
    public SpriteRenderer fruitantRenderer;
    public MonsterController fruitantController;
    public SpriteRenderer playerRenderer;
    public PlayerData playerData;
    public ParticleSystem dust;
    public bool mounted = false;
    public bool demounting = false;
    [SerializeField]
    private bool mountable = false;
    private bool fruitantEnabled = false;

    // lerping
    public Vector3 mountStart;
    public Vector3 mountEnd;
    private float leapLerp = 0;



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

        dust = player.GetComponentInChildren<ParticleSystem>();

        audioSource = this.GetComponent<AudioSource>();
    }

    private void GetPlayerRefs()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();
        playerRenderer = player.GetComponent<SpriteRenderer>();
        playerData = player.GetComponent<PlayerData>();
    }

    public void MountOnLoad()
    {
        Debug.Log("Entering MountOnLoad");

        // set fruitant data
        if(fruitantData == null)
        {
            fruitantData = GetComponentInParent<AIData>();
        }
        fruitantData.rideVector = new Vector2(0, 0);
        fruitantData.currentProtocol = AIData.Protocols.Ride;

        // enable riding
        GetPlayerRefs();
        if (player != null)
            controller = player.GetComponent<PlayerController>();
        controller.riding = true;
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), thisFruitant.GetComponent<Collider2D>(), true);

        // change player layering
        playerRenderer.sortingLayerName = "AboveObjects";

        // set lerps
        leapLerp = 0;
        mountStart = player.transform.position;
        mountEnd = this.transform.position;

        // mounted
        mountable = true;
        mounted = true;
        demounting = false;

        // dust
        dust.Play();
    }

    public void DemountOnLoad()
    {
        Debug.Log("Demounting");

        // set fruitant data
        fruitantData.currentProtocol = AIData.Protocols.Lazy;

        // change player layering
        playerRenderer.flipX = false;
        if (controller != null)
            controller = player.GetComponent<PlayerController>();
        controller.riding = false;

        // set lerps
        leapLerp = 0;
        mountEnd = this.transform.position - new Vector3(0, 1.25f);
        mountStart = player.transform.position;

        // mounted
        mounted = false;
        demounting = true;
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
        if (controller != null)
            controller = player.GetComponent<PlayerController>();
        controller.riding = true;
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), thisFruitant.GetComponent<Collider2D>(), true);

        // change player layering
        playerRenderer.sortingLayerName = "AboveObjects";

        // set lerps
        leapLerp = 0;
        mountStart = player.transform.position;
        mountEnd = this.transform.position;

        // mounted
        mounted = true;
        demounting = false;

        // dust
        dust.Play();
    }

    public void Demount()
    {
        Debug.Log("Demounting");
        audioSource.clip = demountSound;
        audioSource.Play();

        // set fruitant data
        fruitantData.currentProtocol = AIData.Protocols.Lazy;
        
        // change player layering
        playerRenderer.flipX = false;
        if (controller != null)
            controller = player.GetComponent<PlayerController>();
        controller.riding = false;

        // set lerps
        leapLerp = 0;
        mountEnd = this.transform.position - new Vector3(0, 1.25f);
        mountStart = player.transform.position;

        // mounted
        mounted = false;
        demounting = true;
    }

    void Update()
    {
        Debug.Log("Mounted: " + mounted);
        Debug.Log("Mountable: " + mountable);
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
                demounting = false;
                playerRenderer.sortingLayerName = "Objects";
                Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), thisFruitant.GetComponent<Collider2D>(), false);
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
            //Debug.Log("Mount rideVector: " + controller.GetMovementVector());
            //Debug.Log("Mount proxy controller currently Null? " + (controller == null));

            // move player to here
            mountEnd = this.transform.position;
            if (leapLerp >= 1.0)
            {
                player.transform.position = mountEnd;
            }
            else
            {
                leapLerp += Time.deltaTime * 4;
                player.transform.position = Vector3.Lerp(mountStart, mountEnd, leapLerp);
            }

            // flip player
            playerRenderer.flipX = (fruitantController.invert ? fruitantRenderer.flipX : !fruitantRenderer.flipX);

        }
        else
        {
            if (mountable && !EventTrigger.InCutscene && playerData.health > 0 && !controller.riding && InputManager.GetButtonDown(Control.Dash, InputAxis.Dash)) //InputManager.GetAxis(InputAxis.Dash) > 0.9)
            {
                Mount();
                return;
            }
        }

        // when the player hops off
        if (demounting && leapLerp < 2f)
        {
            leapLerp += Time.deltaTime * 4;
            player.transform.position = Vector3.Lerp(mountStart, mountEnd, leapLerp);
            if (leapLerp >= 1)
            {
                demounting = false;
                playerRenderer.sortingLayerName = "Objects";
                Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), thisFruitant.GetComponent<Collider2D>(), false);

                // dust
                dust.Play();
            }
        }
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
            //if (mounted)
                //Demount();
        }
    }
}
