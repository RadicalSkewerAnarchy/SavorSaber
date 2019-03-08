using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnifeAttack : BaseMeleeAttack
{
    public AudioClip damageSFX;
    private PlaySFXRandPitch sfxPlayer;
    [Range(1,10)]
    public float bunceForce = 3;
    public float dropBunceForce = 1;

    // Start is called before the first frame update
    void Start()
    {
        sfxPlayer = GetComponent<PlaySFXRandPitch>();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        // layer 11 is Monster layer
        if (collision.gameObject.tag == "Prey" || collision.gameObject.tag == "Predator")
        {
            //Debug.Log("Applying Knife Effect: " + collision);
            sfxPlayer.PlayRandPitch(damageSFX);
            CharacterData characterData = collision.gameObject.GetComponent<CharacterData>();
            if (characterData != null)
            {
                characterData.DoDamage((int)meleeDamage);
            }
            DoKnockBack(collision.gameObject.GetComponent<Rigidbody2D>(), bunceForce);
        }
        else if (collision.gameObject.tag == "SkewerableObject")
        {
            var objComp = collision.gameObject.GetComponent<SkewerableObject>();
            if (objComp.attached)
            {
                objComp.attached = false;
            }
            else
                DoKnockBack(collision.gameObject.GetComponent<Rigidbody2D>(), dropBunceForce);
        }
    }

    private void DoKnockBack(Rigidbody2D body, float forceScale)
    {
        if (body != null)
        {
            var ForceDir = body.transform.position - transform.parent.position;
            body.AddForce(ForceDir.normalized * forceScale, ForceMode2D.Impulse);
        }
    }
}
