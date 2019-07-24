using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredObject : MonoBehaviour
{

    public bool active = false;
    public bool canBeSourCharged = false;

    // Start is called before the first frame update
    void Start()
    {
        if (active) TurnOn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TurnOn()
    {
        active = true;
    }

    public virtual void ShutOff()
    {
        active = false;
    }
}
