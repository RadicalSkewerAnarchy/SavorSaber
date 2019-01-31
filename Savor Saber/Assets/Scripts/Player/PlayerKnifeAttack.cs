using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnifeAttack : BaseMeleeAttack
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Applying Knife Effect: " + collision);
        if (collision.gameObject.tag == "Monster")
        {
            // get char data to deal damage and kill appropriately
            //Debug.Log("This is a Monster");
            CharacterData characterData = collision.gameObject.GetComponent<CharacterData>();
            if (characterData != null)
            {
                characterData.health -= (int)meleeDamage;
                //Debug.Log("This much hp left: " + characterData.health);
                if (characterData.health <= 0)
                {
                    //Debug.Log("Killing Monster");
                    Monster targetMonster = collision.gameObject.GetComponent<Monster>();
                    targetMonster.Kill();
                }

            }
        }
    }
}
