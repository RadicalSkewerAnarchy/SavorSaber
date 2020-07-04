using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltShield : MonoBehaviour
{
    public GameObject owner;
    private int immortalHp;
    private CharacterData data;
    private AudioSource audioSource;

    public float lifetime;

    // Start is called before the first frame update
    void Start()
    {
        //immortalHp = data.health;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //data.health = immortalHp;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // destroy projectiles
        if (collision.tag == "Reflectable")
        {
            BaseProjectile b = collision.GetComponent<BaseProjectile>();
            if (b.attacker != null)
            {
                if (b.attacker.tag == "Predator")
                {
                    audioSource.Play();
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
        data = owner.GetComponent<CharacterData>();
    }
}
