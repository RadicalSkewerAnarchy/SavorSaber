using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneAwake : Cutscene
{
    public bool onStart = false;
    private void Start()
    {
        if(onStart)
        {
            events = GetComponents<EventScript>();
            player = GameObject.FindGameObjectWithTag("Player");
            Activate();
        }
    }
    private void Awake()
    {
        if (!onStart)
        {
            events = GetComponents<EventScript>();
            player = GameObject.FindGameObjectWithTag("Player");
            Activate();
        }
    }
    protected override IEnumerator PlayEvent()
    {
        yield return new WaitForSeconds(2);
        yield return base.PlayEvent();
    }
}
