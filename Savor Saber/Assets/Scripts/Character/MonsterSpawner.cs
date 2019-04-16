using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MonsterSpawner : MonoBehaviour
{
    public AIGroup group = null;
    public GameObject allNodes;
    public TileNode currentTile;
    public bool loop = true;
    public bool spawnOnStart = true;
    public float loopTime = 15f;

    [SerializeField] private GameObject[] spawnObjects = new GameObject[1];
    [SerializeField] private GameObject[] spawnSignals = new GameObject[0];
    [SerializeField] private List<GameObject> trackedObjects = new List<GameObject>();

    public int maxSpawnedEntites= 8;
    public int maxTries = 100;
    private Collider2D collider;
    private float currTime = 0f;
    public bool spawnAttatched = true;

    private void Start()
    {
        RemoveDeadObjects();
        collider = GetComponent<Collider2D>();
        if (spawnOnStart)
            Spawn();
        float minDistance = Mathf.Infinity;
        foreach(var node in allNodes.GetComponentsInChildren<TileNode>())
        {
            var distance = Vector2.Distance(node.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                currentTile = node;
            }
        }
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

    public void Spawn()
    {
        RemoveDeadObjects();
        bool spawned = false;
        foreach(var go in spawnObjects)
        {
            if (trackedObjects.Count >= maxSpawnedEntites)
                return;

            for (int i = 0; i < maxTries; ++i)
            {
                var bounds = collider.bounds;
                Vector2 spawnPos = new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
                if (collider.OverlapPoint(spawnPos) && Physics2D.OverlapPoint(spawnPos, 0) == null)
                {
                    var newObj = Instantiate(go);
                    if(newObj.GetComponent<Pathfinder>() != null)
                    {
                        newObj.GetComponent<Pathfinder>().allNodes = allNodes;
                    }
                    if(newObj.GetComponent<MonsterChecks>() != null)
                    {
                        newObj.GetComponent<MonsterChecks>().currentTile = currentTile;
                    }
                    if(newObj.GetComponent<SkewerableObject>() != null)
                    {
                        newObj.GetComponent<SkewerableObject>().currentTile = currentTile;
                    }
                    newObj.transform.position = spawnPos;
                    spawned = true;
                    trackedObjects.Add(newObj);
                    group?.AddMember(newObj);

                    // if it's a skewerable object,
                    // set momentum to 0
                    if (newObj.tag == "SkewerableObject" && spawnAttatched)
                    {
                        //Debug.Log("skewerable object drift speed being set to 0");
                        newObj.GetComponent<SkewerableObject>().attached = true;
                    }

                    break;
                }
            }

            if (!loop)
            {
                Destroy(this);
            }
        }
        if(spawned)
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
