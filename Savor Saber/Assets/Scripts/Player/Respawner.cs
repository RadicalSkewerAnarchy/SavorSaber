using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UpdatedController))]
public class Respawner : MonoBehaviour
{
    SpawnPoint currSpawn;
    UpdatedController controller;
    AttackBase[] attacks;
    CharacterData data;
    private Animator anim;
    public bool Respawning { get; private set; } = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<UpdatedController>();
        attacks = GetComponents<AttackBase>();
        data = GetComponent<CharacterData>();
    }

    public void Respawn()
    {
        if(Respawning == false)
        {
            Respawning = true;
            StartCoroutine(Die());
        }

    }

    private IEnumerator Die()
    {
        anim.Play("Wasted");
        controller.Stop();
        controller.enabled = false;
        foreach (var component in attacks)
            component.enabled = false;
        yield return new WaitForSeconds(1);
        currSpawn.Respawn(gameObject);
        Respawning = false;
        controller.enabled = true;
        foreach (var component in attacks)
            component.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Triggering Event: " + name);
        if(collision.tag == "Respawn")
        {
            Debug.Log("Setting Spawn Point to: " + collision.name);
            currSpawn = collision.GetComponent<SpawnPoint>();
            data.health = data.maxHealth;
        }
    }
}
