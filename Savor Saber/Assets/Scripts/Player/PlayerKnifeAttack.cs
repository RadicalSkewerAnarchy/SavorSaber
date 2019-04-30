using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnifeAttack : BaseMeleeAttack
{
    public AudioClip damageSFX;
    public AudioClip fruitBunceSFX;
    private PlaySFXRandPitch sfxPlayer;
    PlayerData myCharData;
    [Range(1,10)]
    public float bunceForce = 3;
    public float dropBunceForce = 1;

    // Start is called before the first frame update
    void Start()
    {
        sfxPlayer = GetComponent<PlaySFXRandPitch>();
        myCharData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject g = collision.gameObject;
        string t = g.tag;
        if (t == "Predator")
        {
            sfxPlayer.PlayRandPitch(damageSFX);
            CharacterData charData = collision.gameObject.GetComponent<CharacterData>();
            //Debug.Log("THERE IS CHARACTER COLLISION");
            if (charData != null)
            {
                myCharData.damageDealt += (int)meleeDamage;
                if (charData.DoDamage((int)meleeDamage))
                    myCharData.entitiesKilled += 1;
            }
            DoKnockBack(collision.gameObject.GetComponent<Rigidbody2D>(), bunceForce);
        }
        else if (t == "Prey")
        {
            sfxPlayer.PlayRandPitch(fruitBunceSFX);
            DoKnockBack(collision.gameObject.GetComponent<Rigidbody2D>(), bunceForce);
        }
        else if (t == "SkewerableObject")
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
            var environment = g.GetComponent<DestructableEnvironment>();
            environment.health -= (int)meleeDamage;
            environment.Destroy();
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
    }
}
