using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantProximityTrigger : MonoBehaviour
{
    PlantFlavorInput plantManager;
    // Start is called before the first frame update
    void Start()
    {
        plantManager = GetComponentInParent<PlantFlavorInput>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(plantManager != null && !plantManager.isFed)
            {
                plantManager.ClosePlant();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !plantManager.isFed)
        {
            if (plantManager != null)
            {
                plantManager.OpenPlant();
            }
        }
    }
}
