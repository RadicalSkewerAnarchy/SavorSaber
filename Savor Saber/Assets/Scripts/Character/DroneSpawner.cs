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
    private GameObject[] droneArray;
    private WaitForSeconds intervalDelay;

    private AudioSource spawnAudio;
    private Pathfinder pf;


    // Start is called before the first frame update
    void Start()
    {
        droneArray = new GameObject[maxDrones];
        intervalDelay = new WaitForSeconds(spawnInterval);
        spawnAudio = GetComponent<AudioSource>();
        StartCoroutine(SpawnLoop());
    }

    void SpawnDrones()
    {
        for(int i = 0; i < maxDrones; i++)
        {
            if(droneArray[i] == null)
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



}
