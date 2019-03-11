using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// on interact with a sword
[RequireComponent(typeof(Collider2D))]
public class DestructableEnvironment : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Detonate()
    {
        Destroy(this.gameObject);
    }
}
