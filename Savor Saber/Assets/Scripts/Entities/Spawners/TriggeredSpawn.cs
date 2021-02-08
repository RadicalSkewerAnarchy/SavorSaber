using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnedObject;
    private ParticleSystem particle;

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
        Instantiate(spawnedObject, transform.position, Quaternion.identity);
        particle.Play();
    }
}
