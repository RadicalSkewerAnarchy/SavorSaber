using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EventTriggerInteract : EventTrigger
{
    public GameObject chatIcon;
    private bool playerInRange;

    private void Start()
    {
        chatIcon.SetActive(true);
    }

    private void Update()
    {
        if (!playerInRange)
            return;
        if (InputManager.GetButtonDown(Control.Interact))
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

    protected override void FinishEvent()
    {
        if(!repeatable)
            chatIcon.SetActive(false);
        base.FinishEvent();       
    }

}
