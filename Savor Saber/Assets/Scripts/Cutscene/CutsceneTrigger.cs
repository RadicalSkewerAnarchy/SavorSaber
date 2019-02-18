using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CutsceneTrigger : Cutscene
{
    private void Start()
    {
        InitializeBase();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggering Event: " + name);
        Activate();
    }
}
