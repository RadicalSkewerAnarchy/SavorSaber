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
        Debug.Log("Applying Knife Effect");
        if (collision.gameObject.tag == "Monster")
        {
            sfxPlayer.PlayRandPitch(damageSFX);
            //just instakill the monster for testing purposes
            Monster targetMonster = collision.gameObject.GetComponent<Monster>();
            targetMonster.Kill();

            CharacterData characterData = collision.gameObject.GetComponent<CharacterData>();
            if (characterData != null)
            {
                characterData.health -= (int)meleeDamage;
            }
        }
    }
}
