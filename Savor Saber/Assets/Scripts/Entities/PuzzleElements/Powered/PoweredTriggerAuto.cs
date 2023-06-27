using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredTriggerAuto : MonoBehaviour
{

    public PoweredObject[] targets;
    // Start is called before the first frame update

    void Awake()
    {
        foreach(PoweredObject target in targets)
        {
            target.TurnOn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
