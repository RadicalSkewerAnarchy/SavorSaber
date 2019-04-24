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
    ParticleSystem teleportRings;

    private bool blocked = false;
    private int objectsBlocking = 0;

    // Start is called before the first frame update
    void Start()
    {
        droneArray = new GameObject[maxDrones];
        intervalDelay = new WaitForSeconds(spawnInterval);
        spawnAudio = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        teleportRings = GetComponent<ParticleSystem>();
        StartCoroutine(SpawnLoop());

    }

    private void Update()
    {
        if(objectsBlocking > 0)
        {
            sr.sprite = offSprite;
            blocked = true;
            teleportRings.Stop();
        }
        else
        {
            sr.sprite = onSprite;
            teleportRings.Play();
            blocked = false;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        objectsBlocking++;
        Debug.Log("New blocking object: " + collision.gameObject);
        Debug.Log("Teleporter blocked by " + objectsBlocking + " objects");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        objectsBlocking--;
        Debug.Log("Removed blocking object: " + collision.gameObject);
        Debug.Log("Teleporter blocked by " + objectsBlocking + " objects");
    }

}
