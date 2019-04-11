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
        GameObject g = collision.gameObject;
        string t = g.tag;
        if (t == "Prey" || t == "Predator" || t == "Drone")
        {
            sfxPlayer.PlayRandPitch(damageSFX);
            CharacterData charData = collision.gameObject.GetComponent<CharacterData>();
            //Debug.Log("THERE IS CHARACTER COLLISION");
            if (charData != null)
            {
                charData.DoDamage((int)meleeDamage);
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
        else if (g.GetComponent<DestructableEnvironment>() != null)
        {
            g.GetComponent<DestructableEnvironment>().Destroy();
        }
        else if (t == "Reflectable")
        {
            var body = g.GetComponent<BaseProjectile>();
            if(body != null)
            {
                body.directionVector *= -1;
                Physics2D.IgnoreCollision(body.GetComponent<Collider2D>(), body.attacker.GetComponent<Collider2D>(), false);
            }              
        }
    }

    private void DoKnockBack(Rigidbody2D body, float forceScale)
    {
        if (body != null)
        {
            var ForceDir = body.transform.position - transform.parent.position;
            body.AddForce(ForceDir.normalized * forceScale, ForceMode2D.Impulse);
        }
        /*else if (g.GetComponent<DestructableEnvironment>() != null)
        {
            Destroy(g);
        }
        else
        {
            //Debug.Log("This is something i cant hit: " + g.name);
        }*/
    }
}
