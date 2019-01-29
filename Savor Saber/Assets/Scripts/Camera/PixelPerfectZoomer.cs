using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.U2D.PixelPerfectCamera))]
public class PixelPerfectZoomer : MonoBehaviour
{
    private UnityEngine.U2D.PixelPerfectCamera z;
    // Start is called before the first frame update
    void Start()
    {
        z = GetComponent<UnityEngine.U2D.PixelPerfectCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            z.assetsPPU += 1;
        if (Input.GetKeyDown(KeyCode.O))
            z.assetsPPU -= 1;
    }
}
