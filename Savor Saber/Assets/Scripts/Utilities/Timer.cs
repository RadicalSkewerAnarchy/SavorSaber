using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer
{
    public float time;
    private float currTime = 0;
    public bool Finished => currTime >= time;
    public float PercentDone => currTime / time;

    public Timer(float time) => this.time = time;

    public void Restart() => currTime = 0;
    public float Increment() => currTime += Time.deltaTime;
    public float IncrementFixed() => currTime += Time.fixedDeltaTime;
    public bool Update() => (currTime += Time.deltaTime) >= time;
    public bool UpdateFixed() => (currTime += Time.fixedDeltaTime) >= time;
}
