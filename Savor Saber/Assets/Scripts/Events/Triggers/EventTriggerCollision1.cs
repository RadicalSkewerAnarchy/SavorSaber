using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EventTriggerCollisionPatriarch : EventTrigger
{
    private void Start()
    {
        InitializeBase();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggering Event: " + name);
        GetComponent<Collider2D>().enabled = false;
        Trigger();
    }
    protected override void FinishEvent()
    {
        if (repeatable)
            GetComponent<Collider2D>().enabled = true;
        base.FinishEvent();
    }
}
