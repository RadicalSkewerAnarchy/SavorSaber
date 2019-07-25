﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PoweredObjectCharger : MonoBehaviour
{

    public bool active = true; 
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
        if (active)
        {
            PoweredObject targetObject = collision.gameObject.GetComponent<PoweredObject>();
            if (targetObject == null || !targetObject.canBeSourCharged)
                return;

            targetObject.TurnOn();
        }

    }
}
