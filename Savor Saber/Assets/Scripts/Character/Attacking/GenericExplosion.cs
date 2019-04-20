using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class GenericExplosion : MonoBehaviour
{
    public float explosionLifetime = 1;
    public float explosionForce = 1;


    void Awake()
    {
        StartCoroutine(Explode());
        AudioSource explodeAudio = GetComponent<AudioSource>();
        Animator explodeAnim = GetComponent<Animator>();

        explodeAudio.Play();
        explodeAnim.SetBool("Explode", true);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 forceVector = (collision.transform.position - transform.position).normalized * explosionForce * 1.5f;
            rb.AddForce(forceVector, ForceMode2D.Impulse);
        }

        
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionLifetime);
        Destroy(this.gameObject);
    }
}
