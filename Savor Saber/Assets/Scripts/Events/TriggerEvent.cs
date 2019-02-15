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
        if (!repeatable)
            GetComponent<Collider2D>().enabled = false;
        var plCon = player.GetComponent<UpdatedController>();
        plCon.enabled = false;
        plCon.Stop();      
        int count = 0;
        foreach (var e in events)
        {
            Debug.Log("Running event: " + ++count);
            yield return StartCoroutine(e.PlayEvent(player));
        }
        plCon.enabled = true;
        if (!repeatable)
            Destroy(this);
    }
}
