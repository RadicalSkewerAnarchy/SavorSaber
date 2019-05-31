using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EventTriggerCollision : EventTrigger
{
    private void Start()
    {
        InitializeBase();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggering Event: " + name);
        var colliders = GetComponents<Collider2D>();
        foreach (var c in colliders)
            c.enabled = false;
        Trigger();
    }
    protected override void FinishEvent()
    {
        if (repeatable)
        {
            var colliders = GetComponents<Collider2D>();
            foreach (var c in colliders)
                c.enabled = true;
        }
        base.FinishEvent();
    }
}
