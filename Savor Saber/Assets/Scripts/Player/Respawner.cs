using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    SpawnPoint currSpawn;
    private Animator anim;
    bool respawning = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Respawn()
    {
        if(respawning == false)
        {
            respawning = true;
            StartCoroutine(Die());
        }

    }

    private IEnumerator Die()
    {
        anim.Play("Wasted");
        yield return new WaitForSeconds(1);
        currSpawn.Respawn(gameObject);
        respawning = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Triggering Event: " + name);
        if(collision.tag == "Respawn")
        {
            currSpawn = collision.GetComponent<SpawnPoint>();
        }
    }
}
