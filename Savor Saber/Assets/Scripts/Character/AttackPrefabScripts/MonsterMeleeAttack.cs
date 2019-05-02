using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMeleeAttack : BaseMeleeAttack
{
    public AudioClip damageSFX;
    private PlaySFXRandPitch sfxPlayer;
    public MonsterChecks monsterChecks;
    public CharacterData myCharData;

    // Start is called before the first frame update
    void Start()
    {
        sfxPlayer = GetComponent<PlaySFXRandPitch>();
        monsterChecks = gameObject.GetComponentInParent<MonsterChecks>();
        if(monsterChecks == null)
        {
            Debug.Log("MonsterChecks is null");
        }
        var circleCollider = GetComponent<CircleCollider2D>();
        myCharData = GetComponent<CharacterData>();
        //Physics2D.IgnoreCollision(circleCollider, GetComponent<CapsuleCollider2D>());
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("THERE IS NO CHARACTERCOLLISION");
        //monsterChecks.Enemies.Clear();
        GameObject g = collision.gameObject;
        string t = g.tag;
        if (t == "Player" || t == "Prey" || t == "Predator")
        {            
            if(damageSFX != null)
                sfxPlayer.PlayRandPitch(damageSFX);
            CharacterData charData = collision.gameObject.GetComponent<CharacterData>();
            //Debug.Log("THERE IS CHARACTER COLLISION");
            if (charData != null)
            {
                if(myCharData != null)
                    myCharData.damageDealt += (int)meleeDamage;
                if (charData.DoDamage((int)meleeDamage))
                    myCharData.entitiesKilled += 1;
            }                       
        }
        else if (g.GetComponent<DestructableEnvironment>() != null)
        {
            DestructableEnvironment de  = g.GetComponent<DestructableEnvironment>();
            de.health -= ((int)meleeDamage);
            de.Destroy();
        }
    }
}
