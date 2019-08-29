using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Respawner : MonoBehaviour
{
    SpawnPoint currSpawn;
    PlayerController controller;
    AttackBase[] attacks;
    CharacterData data;
    private Animator anim;
    public bool Respawning { get; private set; } = false;
    public AudioClip healSFX;
    private PlaySFX sfxPlayer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        attacks = GetComponents<AttackBase>();
        data = GetComponent<CharacterData>();
        sfxPlayer = GetComponent<PlaySFX>();
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
        BGMManager.instance.FadeBGMToSilence(1);
        yield return new WaitForSeconds(1);
        BGMManager.instance.FadeToAreaSounds();
        currSpawn.Respawn(gameObject);
        Respawning = false;
        controller.enabled = true;
        foreach (var component in attacks)
        {
            // TEMPORARY FIX, 
            // MAKE PREFAB WITH REMOVED COMPONENT: Attack Melee
            if (component is AttackMelee && !(component is AttackMeleeSkewer) )
                component.enabled = false;
            else
                component.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Triggering Event: " + name);
        if(collision.tag == "Respawn")
        {
            Debug.Log("Setting Spawn Point to: " + collision.name);
            currSpawn = collision.GetComponent<SpawnPoint>();

            //only heal if it's needed
            if(data.health < data.maxHealth)
            {
                data.health = data.maxHealth;
                sfxPlayer.Play(healSFX);
            }
            
            
        }
    }
}
