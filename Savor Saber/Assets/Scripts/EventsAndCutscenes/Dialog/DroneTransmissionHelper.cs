﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DroneTransmissionHelper : MonoBehaviour
{

    SpriteRenderer sr;
    bool alive = true;
    public GameObject parentDrone;
    public GameObject extraDebrisToEnable;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;

        if (extraDebrisToEnable != null)
        {
            extraDebrisToEnable.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(alive)
            transform.position = parentDrone.transform.position;
    }

    public void SpawnWreckage()
    {
        alive = false;
        sr.enabled = true;
        if(extraDebrisToEnable != null)
        {
            extraDebrisToEnable.SetActive(true);
        }
    }
}
