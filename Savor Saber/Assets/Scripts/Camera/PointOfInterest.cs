using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PointOfInterest : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Capturing camera");
            collision.gameObject.GetComponent<CameraController>().SetTarget(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Releasing Camera");
            collision.gameObject.GetComponent<CameraController>().ReleaseTarget();
        }
    }
}
