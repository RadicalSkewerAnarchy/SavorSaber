using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PointOfInterest : MonoBehaviour
{
    public float maxPullDistance = 5;
    public float maxPullSpeed = 100;
    public float snapTime = 0.75f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("Capturing camera");
            var cam = collision.gameObject.GetComponent<CameraController>();
            cam.SetTarget(gameObject.gameObject, maxPullDistance, maxPullSpeed, snapTime);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("Releasing Camera");
            collision.gameObject.GetComponent<CameraController>().ReleaseTarget();
        }
    }
}
