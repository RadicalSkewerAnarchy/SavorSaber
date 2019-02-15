using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerEvent : MonoBehaviour
{
    private GameObject player;
    public bool repeatable = false;
    private EventScript[] events;
    private void Start()
    {
        events = GetComponents<EventScript>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggering Event: " + name);
        StartCoroutine(PlayEvent());
    }
    private IEnumerator PlayEvent()
    {
        player.GetComponent<DialogData>().inConversation = true;
        foreach (var e in events)
        {
            Debug.Log("Running: " + e.name);
            yield return StartCoroutine(e.PlayEvent(player));
        }
        player.GetComponent<DialogData>().inConversation = false;
        if (!repeatable)
            Destroy(this);
    }
}
