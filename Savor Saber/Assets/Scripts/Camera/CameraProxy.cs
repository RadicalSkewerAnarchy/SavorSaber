using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Made so the main camera can be accessed from unity events with multi-scene loading
/// </summary>
public class CameraProxy : MonoBehaviour
{
    public float time;
    public float intensity;
    public void Shake()
    {
        CameraController.instance.Shake(time, intensity);
    }
}
