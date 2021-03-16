using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnedObject;
    private ParticleSystem particle;
    [SerializeField]
    private EventOnDeath deathEventTracker; //leave blank if unneeded

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        GameObject spawn = Instantiate(spawnedObject, transform.position, Quaternion.identity);
        particle.Play();

        //if relevant, adds the spawned object to a tracker that will fire events when all targets are dead
        if(deathEventTracker != null)
        {
            deathEventTracker.AddTarget(spawn);
        }
    }
}
