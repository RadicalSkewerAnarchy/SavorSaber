﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnifeAttack : BaseMeleeAttack
{
    public AudioClip damageSFX;
    private PlaySFXRandPitch sfxPlayer;
    private const float bunceForce = 150;

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
            Rigidbody2D body = collision.GetComponent<Rigidbody2D>();
            if(body != null)
            {
                float angle = Vector2.Angle(transform.position, collision.transform.position);
                var vector = AngleToVector(angle, false) * bunceForce;
                if (transform.position.x > collision.transform.position.x)
                    vector.x *= -1;
                if (transform.position.y > collision.transform.position.y)
                    vector.y *= -1;
                body.AddForce(vector);
            }
        }
        else if (collision.gameObject.tag == "SkewerableObject")
        {
            collision.gameObject.GetComponent<SkewerableObject>().attached = false;
        }
    }

    private Vector2 AngleToVector(float angle, bool inDegrees)
    {
        if (inDegrees == true)
            angle = angle * (Mathf.PI / 180);
        Vector2 vec = new Vector2();
        //TRIGONOMETRY WOOOOOOOO
        vec.x = -1 * Mathf.Cos(angle);
        vec.y = Mathf.Sin(angle);
        return vec;
    }
}
