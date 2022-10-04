using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltSpawner : PoweredObject
{
    public GameObject objectTemplate;
    private GameObject activeObject;

    public Transform spawnPoint;
    public Transform dropoffPoint;

    public float speed; 

    private bool moving;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            float step = speed * Time.deltaTime;
            activeObject.transform.position = Vector3.MoveTowards(activeObject.transform.position, dropoffPoint.position, step);
            if (Vector3.Distance(activeObject.transform.position, dropoffPoint.position) < 0.1f)
            {
                ShutOff();
            }
        }
    }

    public override void TurnOn()
    {
        if (!active) //one at a time
        {
            activeObject = Instantiate(objectTemplate, spawnPoint.position, Quaternion.identity);
            base.TurnOn();
        }

    }

    public override void ShutOff()
    {
        base.ShutOff();
    }
}
