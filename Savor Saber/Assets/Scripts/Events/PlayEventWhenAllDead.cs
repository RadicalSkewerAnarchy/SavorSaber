using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class PlayEventWhenAllDead : MonoBehaviour
{
    public GameObject[] trackedObjects;
    public UnityEvent toPlay;

    // Update is called once per frame
    void Update()
    {
        if(trackedObjects.All((obj) => obj == null))
        {
            toPlay.Invoke();
            enabled = false;
        }           
    }
}
