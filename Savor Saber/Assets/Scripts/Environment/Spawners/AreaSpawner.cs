using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component that spawns objects within an area.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class AreaSpawner : MonoBehaviour
{    
    [SerializeField] protected GameObject[] spawnObjects = new GameObject[1];
    [SerializeField] protected GameObject[] spawnSignals = new GameObject[0];
    [SerializeField] protected List<GameObject> trackedObjects = new List<GameObject>();

    public int maxSpawnedEntites= 8;
    public int maxTries = 100;
    public bool spawnAttatched = false;

    new protected Collider2D collider;
    
    private void Start()
    {
        RemoveDeadObjects();
        collider = GetComponent<Collider2D>();
    }

    public virtual void Spawn()
    {
        RemoveDeadObjects();
        bool objectSpawned = false;
        // Attempt to spawn each object in spawn objects
        foreach(var go in spawnObjects)
        {
            if (trackedObjects.Count >= maxSpawnedEntites)
                break;
            for (int i = 0; i < maxTries; ++i)
            {
                var bounds = collider.bounds;
                Vector2 spawnPos = new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
                if (collider.OverlapPoint(spawnPos) && Physics2D.OverlapPoint(spawnPos, 0) == null)
                {
                    var newObj = Instantiate(go, spawnPos, Quaternion.identity);
                    objectSpawned = true;
                    trackedObjects.Add(newObj);

                    // if it's a skewerable object,
                    // set momentum to 0
                    if (newObj.tag == "SkewerableObject" && spawnAttatched)
                    {
                        newObj.GetComponent<SkewerableObject>().Attached = true;
                    }
                    break;
                }
            }
        }
        // If at least one object was spawned, spawn all signals
        if(objectSpawned)
        {
            foreach(var go in spawnSignals)
            {
                var newObj = Instantiate(go, transform);
            }
        }
    }

    public void RemoveDeadObjects()
    {
        trackedObjects.RemoveAll((obj) => obj == null);
    }

    public GameObject[] GetMembers()
    {
        RemoveDeadObjects();
        return trackedObjects.ToArray();
    }
}
