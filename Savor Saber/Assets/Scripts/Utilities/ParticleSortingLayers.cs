using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParticleSortingLayers : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Renderer>().sortingLayerName = "AboveObjects";
    }
}
