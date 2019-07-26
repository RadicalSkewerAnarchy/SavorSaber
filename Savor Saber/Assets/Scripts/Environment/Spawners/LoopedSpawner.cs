using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
/// <summary>
/// A component that spawns objects within an area on a timer
/// </summary>
public class LoopedSpawner : AreaSpawner
{
    public bool spawnOnStart = true;
    public bool loop = true;
    public float loopTime = 15f;
    
    private float currTime = 0f;

    private void Start()
    {
        RemoveDeadObjects();
        collider = GetComponent<Collider2D>();
        if (spawnOnStart)
            Spawn();
    }

    private void FixedUpdate()
    {
        currTime += Time.fixedDeltaTime;
        if(currTime >= loopTime)
        {
            currTime = 0;
            Spawn();
        }
    }

    public override void Spawn()
    {
        base.Spawn();
        // If the spawner is not reusable, destroy
        if (!loop)
        {
            Destroy(this);
        }
    }
}
