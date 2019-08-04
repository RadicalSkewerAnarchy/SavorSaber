﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkewerAttack : BaseMeleeAttack
{
    public Inventory inventory;
    public AudioClip pickUpSFX;
    public AudioClip cantPickUpSFX;
    private PlaySFX sfxPlayer;
    public float bunceForce = 3;
    private Transform parentTransform;

    void Start()
    {
        parentTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        sfxPlayer = GetComponent<PlaySFX>();
        Debug.Log("Skewered " + collision.gameObject);
        // skewer roots
        if (collision.gameObject.tag == "ThrowThrough")
        {
            var environment = collision.gameObject.GetComponent<DestructableEnvironment>();
            if (environment != null)
            {
                if (environment.skewerable)
                {
                    environment.Health -= 1;
                }
            }
        }

        // pickup  drops
        if (collision.gameObject.tag == "SkewerableObject" || collision.gameObject.tag == "Reflectable")
        {
            if (!inventory.ActiveSkewerFull() && !inventory.ActiveSkewerCooked())
            {
                //Debug.Log("Hit skewerable object");
                SkewerableObject targetObject = collision.gameObject.GetComponent<SkewerableObject>();

                sfxPlayer.Play(pickUpSFX);

                inventory.AddToSkewer(targetObject.data);
                MGSTextSpawner.instance?.SpawnText(targetObject.data, transform.position);
                Destroy(collision.gameObject);
            }
            else
            {
                sfxPlayer.Play(cantPickUpSFX);
            }
        }

        //do knockback effects
        if (collision.gameObject.tag == "Predator" || collision.gameObject.tag == "Prey")
        {
            DoKnockBack(collision.gameObject.GetComponent<Rigidbody2D>(), bunceForce);
        }
    }

    private void DoKnockBack(Rigidbody2D body, float forceScale)
    {
        if (body != null)
        {
            var ForceDir = body.transform.position - parentTransform.position;
            body.AddForce(ForceDir.normalized * forceScale, ForceMode2D.Impulse);
        }
    }
}   
