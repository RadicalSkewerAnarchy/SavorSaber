using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DroneSpawner : MonoBehaviour
{
    public GameObject droneType;
    public GameObject allNodes;

    [SerializeField]
    private int maxDrones = 3;
    [SerializeField]
    private int spawnInterval = 7;

    public Sprite onSprite;
    public Sprite offSprite;
    public Sprite deadSprite;

    private GameObject[] droneArray;
    private WaitForSeconds intervalDelay;

    private AudioSource spawnAudio;
    private Pathfinder pf;
    private SpriteRenderer sr;
    private Light light;
    ParticleSystem teleportRings;

    private bool blocked = false;
    private Collider2D[] overlappingObject = null;
    private bool overlapped = false;

    public bool active = true;

    //If this is not null, all spawned drones will be added to a list of minions
    //that can all be destroyed with a single event (e.g. boss minions)

    //DO NOT add a reference if you don't want this functionality
    public DestroyMinionsOnBossDeath minionList;

    // Start is called before the first frame update
    void Start()
    {
        droneArray = new GameObject[maxDrones];
        intervalDelay = new WaitForSeconds(spawnInterval);
        spawnAudio = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        teleportRings = GetComponent<ParticleSystem>();
        light = GetComponentInChildren<Light>();

        StartCoroutine(SpawnLoop());


    }

    private void Update()
    {
        //create an overlap test box that only checks default
        if (active)
        {
            overlapped = CheckValidCollisions();

            if (overlapped)
            {
                //Debug.Log("OFF");
                sr.sprite = offSprite;
                light.color = Color.red;
                blocked = true;
                teleportRings.Stop();
            }
            else
            {
                //Debug.Log("ON");
                sr.sprite = onSprite;
                light.color = Color.green;
                teleportRings.Play();
                blocked = false;
            }
        }

    }

    //deactivate the spawner entirely
    public void ShutOff()
    {
        sr.sprite = offSprite;
        light.color = Color.red;
        blocked = true;
        teleportRings.Stop();

        active = false;
    }

    //activate an inactive spawner
    public void TurnOn()
    {
        sr.sprite = onSprite;
        light.color = Color.green;
        teleportRings.Play();
        blocked = false;

        active = true;
    }

    private bool CheckValidCollisions()
    {
        overlappingObject = Physics2D.OverlapBoxAll(transform.position - new Vector3(0, 0.35f), new Vector2(1.5f, 0.75f), 0f);

        if (overlappingObject.Length == 0)
            return false;
        else
        {
            foreach(Collider2D collider in overlappingObject)
            {
                if (collider.gameObject.layer == 0 || collider.gameObject.layer == 8)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public void SpawnDrones()
    {
        for(int i = 0; i < maxDrones; i++)
        {
            if(!blocked && droneArray[i] == null)
            {
                //if you find an empty slot, spawn a new drone, then wait before checking again
                droneArray[i] = Instantiate(droneType, transform.position, Quaternion.identity);
                if(minionList != null)
                {
                    minionList.minions.Add(droneArray[i]);
                }
                pf = droneArray[i].GetComponent<Pathfinder>();
                if(allNodes != null)
                    pf.allNodes = allNodes;

                spawnAudio.Play();
                StopCoroutine(SpawnLoop());
                StartCoroutine(SpawnLoop());
                return;
            }
        }
        //if you found no empty slots, wait before checking again
        StopCoroutine(SpawnLoop());
        StartCoroutine(SpawnLoop());

    }

    private IEnumerator SpawnLoop()
    {
        yield return intervalDelay;
        SpawnDrones();
        yield return null;
    }

}
