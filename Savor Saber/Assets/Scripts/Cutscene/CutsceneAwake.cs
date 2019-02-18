using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneAwake : Cutscene
{
    public bool onStart = false;
    private void Start()
    {
        if (onStart)
            Play();
    }
    private void Awake()
    {
        if (!onStart)
            Play();
    }
    private void Play()
    {
        InitializeBase();
        Activate();
    }
    protected override IEnumerator PlayEvent()
    {
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        yield return base.PlayEvent();
    }
}
