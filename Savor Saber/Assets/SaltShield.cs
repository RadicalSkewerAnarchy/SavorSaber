using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltShield : MonoBehaviour
{
    public GameObject fruit;
    private int immortalHp;
    private AIData data;
    private AudioSource audioSource;

    public float lifetime;

    // Start is called before the first frame update
    void Start()
    {
        data = fruit.GetComponent<AIData>();
        immortalHp = data.health;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        data.health = immortalHp;

        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            Destroy(this.gameObject);
        }
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
}
