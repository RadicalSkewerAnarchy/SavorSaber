using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MaterialChanger : MonoBehaviour
{

    public Material targetMaterial;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpriteRenderer targetSR = collision.gameObject.GetComponent<SpriteRenderer>();

        if(targetSR != null)
        {
            targetSR.material = targetMaterial;
        }
    }

}
