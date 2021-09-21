using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneTurretParticle : MonoBehaviour
{

    public GameObject explosionTemplate;
    public int particleDamage = 1;

    public List<ParticleCollisionEvent> collisionEvents;

    // Start is called before the first frame update
    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        //Debug.Log("PARTICLE COLLISION");
        if (other.tag == "Player" || other.tag == "Prey")
        {
            CharacterData data = other.GetComponent<CharacterData>();
            data.DoDamage(particleDamage);
        }
    }
}
