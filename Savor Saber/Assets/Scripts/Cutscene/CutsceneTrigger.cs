using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CutsceneTrigger : Cutscene
{
    private void Start()
    {
        events = GetComponents<EventScript>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggering Event: " + name);
        Activate();
    }
}
