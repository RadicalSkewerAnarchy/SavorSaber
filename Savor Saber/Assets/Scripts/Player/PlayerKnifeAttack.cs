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

        // layer 11 is Monster layer
        if (collision.gameObject.tag == "Prey" || collision.gameObject.tag == "Predator")
        {
            Debug.Log("Applying Knife Effect: " + collision);
            sfxPlayer.PlayRandPitch(damageSFX);
            CharacterData characterData = collision.gameObject.GetComponent<CharacterData>();
            if (characterData != null)
            {
                characterData.DoDamage((int)meleeDamage);

            }
        }
    }
}
