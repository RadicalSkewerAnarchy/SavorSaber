using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    protected GameObject player;
    public bool repeatable = false;
    protected EventScript[] events;
    private void Start()
    {
        events = GetComponents<EventScript>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Activate()
    {
        Debug.Log("Activating Cutscene: " + name);
        StartCoroutine(PlayEvent());
    }
    protected virtual IEnumerator PlayEvent()
    {
        if (!repeatable && GetComponent<Collider2D>() != null)
            GetComponent<Collider2D>().enabled = false;
        var plCon = player.GetComponent<UpdatedController>();
        plCon.enabled = false;
        plCon.Stop();
        int count = 0;
        foreach (var e in events)
        {
            Debug.Log("Running cutscene: " + name + " Event: " + ++count);
            yield return StartCoroutine(e.PlayEvent(player));
        }
        plCon.enabled = true;
        if (!repeatable)
        {
            Destroy(gameObject);
        }
    }
}
