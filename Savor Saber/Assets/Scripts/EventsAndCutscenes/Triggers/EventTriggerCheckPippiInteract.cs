﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
public class EventTriggerCheckPippiInteract : EventTrigger
{
    public GameObject PippiPear;
    public float threshold;
    public string successflagName;
    private bool playerInRange;

    private void Update()
    {
        if (!playerInRange)
            return;
        if (InputManager.GetButtonDown(Control.Interact))
        {
            FlagManager.SetFlag(successflagName, CheckParty().ToString());
            Trigger();
        }
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
    private void Start()
    {
        InitializeBase();
    }
    public bool CheckParty()
    {
        return Vector2.Distance(PippiPear.transform.position, player.transform.position) <= threshold;
    }
}
