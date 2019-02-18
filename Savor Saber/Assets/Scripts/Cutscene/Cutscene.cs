using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cutscene : MonoBehaviour
{
    public bool IsActive { get; protected set; }   
    public bool repeatable = false;
    public UnityEvent callOnStart;
    public UnityEvent callOnCompletion;
    protected EventScript[] events;
    protected GameObject player;
    protected UpdatedController plCon;

    protected void InitializeBase()
    {
        events = GetComponents<EventScript>();
        player = GameObject.FindGameObjectWithTag("Player");
        plCon = player.GetComponent<UpdatedController>();
    }
    private void Start()
    {
        InitializeBase();
    }
    public void Activate()
    {
        if (IsActive)
            return;
        Debug.Log("Activating Cutscene: " + name);
        BeforeCutsceneStart();
        StartCoroutine(PlayEvent());
    }
    protected virtual IEnumerator PlayEvent()
    {
        int count = 0;
        foreach (var e in events)
        {
            Debug.Log("Running cutscene: " + name + " Event: " + ++count);
            yield return StartCoroutine(e.PlayEvent(player));
        }
        FinishCutscene();
    }
    protected virtual void BeforeCutsceneStart()
    {
        IsActive = true;
        callOnStart.Invoke();
        plCon.enabled = false;
        plCon.Stop();
        if (!repeatable && GetComponent<Collider2D>() != null)
            GetComponent<Collider2D>().enabled = false;
    }
    protected virtual void FinishCutscene()
    {
        plCon.enabled = true;
        IsActive = false;
        callOnCompletion.Invoke();
        if (!repeatable)
        {
            Destroy(gameObject);
        }
    }
}
