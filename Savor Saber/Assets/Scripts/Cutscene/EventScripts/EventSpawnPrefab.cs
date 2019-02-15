using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpawnPrefab : EventScript
{
    public GameObject prefab;
    public Transform position;
    public Vector2 manualPosition;

    public override IEnumerator PlayEvent(GameObject player)
    {
        if(position == null)
        {
            var obj = Instantiate(prefab);
            obj.transform.position = manualPosition;
        }
        else
            Instantiate(prefab, position);
        yield break;
    }
}
