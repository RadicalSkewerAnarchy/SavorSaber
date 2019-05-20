using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EventTriggerCheckPartyInteract : EventTrigger
{
    public int number = 1;
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
        var data = player.GetComponent<PlayerData>();
        return data.party.Count >= number;
    }
}
