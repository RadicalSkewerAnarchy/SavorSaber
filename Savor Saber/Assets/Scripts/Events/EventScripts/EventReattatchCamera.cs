using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventReattatchCamera : EventScript
{
    public override IEnumerator PlayEvent(GameObject player)
    {
        player.GetComponent<CameraController>().Detatched = false;
        yield break;
    }
}
