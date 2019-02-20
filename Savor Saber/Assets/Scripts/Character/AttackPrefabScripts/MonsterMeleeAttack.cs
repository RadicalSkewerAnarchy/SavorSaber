using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMeleeAttack : BaseMeleeAttack
{
    public AudioClip damageSFX;
    private PlaySFXRandPitch sfxPlayer;
    public MonsterChecks monsterChecks;

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
        //Physics2D.IgnoreCollision(circleCollider, GetComponent<CapsuleCollider2D>());
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("THERE IS NO CHARACTERCOLLISION");
        //monsterChecks.Enemies.Clear();
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "prey" || collision.gameObject.tag == "predator")
        {            
            sfxPlayer.PlayRandPitch(damageSFX);
            CharacterData charData = collision.gameObject.GetComponent<CharacterData>();
            //Debug.Log("THERE IS CHARACTER COLLISION");
            if (charData != null)
            {
                charData.health -= (int)meleeDamage;
                Debug.Log("Character health: " + charData.health);
                if (charData.health <= 0)
                {
                    //collision.gameObject.SetActive(false);
                }
            }                       
        }
    }
}
