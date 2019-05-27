using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantKiller : MonoBehaviour
{
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
        if(collision.gameObject.tag == "LargePlant")
        {
            PlantFlavorInput input = collision.gameObject.GetComponent<PlantFlavorInput>();
            input.isFed = true;
            input.OpenPlant();
        }
    }
}
