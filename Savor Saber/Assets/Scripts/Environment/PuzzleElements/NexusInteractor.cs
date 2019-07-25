using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NexusInteractor : MonoBehaviour
{
    public AreaSpawner spawner;
    public float spawnCooldownTime = 2.5f;

    // Interact to spawn fields
    private Collider2D interactArea;
    private bool playerInInteractArea;
    private bool readyToSpawn;

    private void Awake()
    {
        interactArea = GetComponent<Collider2D>();
        readyToSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(readyToSpawn && playerInInteractArea && InputManager.GetButtonDown(Control.Interact))
        {
            spawner.Spawn();
            readyToSpawn = false;
            StartCoroutine(SpawnCooldownCr());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerInInteractArea = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInInteractArea = false;
    }

    private IEnumerator SpawnCooldownCr()
    {
        yield return new WaitForSeconds(spawnCooldownTime);
        readyToSpawn = true;
    }
}
