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
    public float shakeTime = 0.2f;
    [Range(0, 2)]
    public float shakeIntensity = 0.1f;
    public bool invisible = false;
    public string tagToIgnore = "";

    void Awake()
    {
        StartCoroutine(Explode());
        if (!invisible)
        {
            AudioSource explodeAudio = GetComponent<AudioSource>();
            Animator explodeAnim = GetComponent<Animator>();
            CameraController.instance?.Shake(shakeTime, 0.1f, shakeIntensity);
            explodeAudio.Play();
            explodeAnim.SetBool("Explode", true);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null && collision.gameObject.tag != tagToIgnore)
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
