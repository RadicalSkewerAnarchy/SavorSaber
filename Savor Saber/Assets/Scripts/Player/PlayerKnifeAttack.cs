using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnifeAttack : BaseMeleeAttack
{
    public AudioClip damageSFX;
    private PlaySFXRandPitch sfxPlayer;

    // Start is called before the first frame update
    void Start()
    {
        sfxPlayer = GetComponent<PlaySFXRandPitch>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject g = collision.gameObject;
        string t = g.tag;
        if (t == "Prey" || t == "Predator")
        {
            sfxPlayer.PlayRandPitch(damageSFX);
            CharacterData charData = collision.gameObject.GetComponent<CharacterData>();
            //Debug.Log("THERE IS CHARACTER COLLISION");
            if (charData != null)
            {
                charData.DoDamage((int)meleeDamage);
            }
        }
        else if (collision.gameObject.tag == "SkewerableObject")
        {
            collision.gameObject.GetComponent<SkewerableObject>().attached = false;
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
