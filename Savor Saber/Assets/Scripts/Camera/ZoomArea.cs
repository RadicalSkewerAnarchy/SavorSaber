using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomArea : MonoBehaviour
{
    public int targetPPU;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Capturing camera");
            var cam = collision.gameObject.GetComponent<CameraController>();
            cam.SetZoom(targetPPU);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Releasing Camera");
            var cam = collision.gameObject.GetComponent<CameraController>();
            cam.SetZoom(CameraController.standardZoom);
        }
    }
}
