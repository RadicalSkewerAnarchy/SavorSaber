using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class PlayEventWhenAllDead : MonoBehaviour
{
    public GameObject[] trackedObjects;
    public UnityEvent toPlay;
    private bool active = true;

    // Update is called once per frame
    void Update()
    {
        if(active && trackedObjects.All((obj) => obj == null))
        {
            toPlay.Invoke();
            enabled = false;
            active = false; // be double safe
        }           
    }
}
