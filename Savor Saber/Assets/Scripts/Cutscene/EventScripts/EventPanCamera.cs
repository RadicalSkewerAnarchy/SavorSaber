using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPanCamera : EventScript
{
    public enum MoveType
    {
        Smoothed,
        Linear,
    }

    public MoveType movementType;
    public GameObject target;
    public float maxPullSpeed = 100;
    public float snapTime = 0.75f;

    public override IEnumerator PlayEvent(GameObject player)
    {
        
        var cam = player.GetComponent<CameraController>();
        cam.Detatched = true;
        switch (movementType)
        {
            case MoveType.Smoothed:
                yield return StartCoroutine(cam.MoveToPointSmoothCr(target.transform.position, maxPullSpeed, snapTime));
                break;
            case MoveType.Linear:
                yield return StartCoroutine(cam.MoveToPointLinearCr(target.transform.position, maxPullSpeed/100));
                break;
        }   
    }
}
