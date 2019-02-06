using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : BaseMeleeAttack
{
    public AudioClip damageSFX;
    private PlaySFXRandPitch sfxPlayer;
    private MonsterChecks monsterChecks;

    // Start is called before the first frame update
    void Start()
    {
        sfxPlayer = GetComponent<PlaySFXRandPitch>();
        monsterChecks = GetComponent<MonsterChecks>();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("THERE IS NO CHARACTERCOLLISION");
        if (collision.gameObject.tag == "Player")
        {
            sfxPlayer.PlayRandPitch(damageSFX);
            CharacterData charData = collision.gameObject.GetComponent<CharacterData>();
            Debug.Log("THERE IS CHARACTER COLLISION");
            if(charData != null)
            {
                charData.health -= (int)meleeDamage;
                Debug.Log("Character health: " + charData.health);
                if(charData.health <= 0)
                {
                    //monsterChecks.Enemies.Remove(collision.gameObject);
                    collision.gameObject.SetActive(false);                  
                    
                }
            }
        }
    }
}
