using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EventTriggerInteractFlag : EventTrigger
{
    private bool playerInRange;
    public string flag;
    public string value;

    private void Update()
    {
        if (!playerInRange)
            return;
        if (InputManager.GetButtonDown(Control.Interact) && FlagManager.GetFlag(flag) == value)
            Trigger();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }
}
