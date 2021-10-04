using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SaltShield : MonoBehaviour
{
    public GameObject owner;
    private int immortalHp;
    private CharacterData data;
    private AudioSource audioSource;
    private SpriteRenderer sr;
    private Collider2D collision;

    //public float lifetime;
    public float rechargeDelay = 4;
    public float health = 4;
    private float maxHealth;
    public bool immortal = false;

    private Vector3 color;
    private WaitForSeconds rechargeTic;

    // Start is called before the first frame update
    void Start()
    {
        //immortalHp = data.health;
        audioSource = GetComponent<AudioSource>();
        maxHealth = health;
        sr = GetComponent<SpriteRenderer>();
        collision = GetComponent<Collider2D>();
        rechargeTic = new WaitForSeconds(0.25f);

        color.x = sr.color.r;
        color.y = sr.color.g;
        color.z = sr.color.b;
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
                    if(!immortal)
                        TakeDamage(b.projectileDamage);

                    Destroy(collision.gameObject);
                }
            }
        }
    }

    private void TakeDamage(float damage)
    {
        audioSource.Play();
        SetHealth(health - damage);
        StopAllCoroutines();
        StartCoroutine(RechargeDelay());
    }

    private void SetHealth(float hp)
    {
        health = hp;
        if (health <= 0)
        {
            health = 0;
            collision.enabled = false;
        }
        else
        {
            collision.enabled = true;
        }
        sr.color = new Color(color.x, color.y, color.z, (health / maxHealth));
    }

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
        data = owner.GetComponent<CharacterData>();
    }

    private IEnumerator RechargeDelay()
    {
        yield return new WaitForSeconds(rechargeDelay);
        yield return Recharge();
    }

    private IEnumerator Recharge()
    {
        if(health >= maxHealth)
        {
            health = maxHealth;
            yield return null;
        }
        yield return rechargeTic;
        SetHealth(health += 0.25f);
        yield return Recharge();
    }
}
