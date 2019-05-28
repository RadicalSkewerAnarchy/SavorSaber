using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkewerAttack : BaseMeleeAttack
{
    public Inventory inventory;
    public AudioClip pickUpSFX;
    public AudioClip cantPickUpSFX;
    private PlaySFX sfxPlayer;

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        sfxPlayer = GetComponent<PlaySFX>();

        // skewer roots
        if (collision.gameObject.tag == "ThrowThrough")
        {
            var environment = collision.gameObject.GetComponent<DestructableEnvironment>();
            if (environment != null)
            {
                if (environment.skewerable)
                {
                    environment.health -= 1;
                    environment.Destroy();
                }
            }
        }

        // pickup  drops
        if (collision.gameObject.tag == "SkewerableObject")
        {
            if (!inventory.ActiveSkewerFull() && !inventory.ActiveSkewerCooked())
            {
                //Debug.Log("Hit skewerable object");
                SkewerableObject targetObject = collision.gameObject.GetComponent<SkewerableObject>();

                sfxPlayer.Play(pickUpSFX);

                inventory.AddToSkewer(targetObject.data);
                Destroy(collision.gameObject);
            }
            else
            {
                sfxPlayer.Play(cantPickUpSFX);
            }
        }
    }
}   
