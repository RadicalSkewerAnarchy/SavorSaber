using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParticleSortingLayers : MonoBehaviour
{

    public enum sortingLayer{
        Default,
        BelowObjects1,
        BelowObjects2,
        BelowObjects3,
        GroundEffects,
        Objects,
        AboveObjects, 
        Weapons,
        Weather,
        UI

    }
    public sortingLayer dropdown = sortingLayer.Default;
    // Start is called before the first frame update
    void Update()
    {
        //sortingLayer dropdown = sortingLayer.Default;
        foreach(var renderer in transform.GetComponentsInChildren<ParticleSystemRenderer>()){
            if(renderer.gameObject.transform.GetComponent<ParticleSortingLayers>() == null){
                renderer.sortingLayerName = dropdown.ToString();
            }
        }
        GetComponent<ParticleSystemRenderer>().sortingLayerName = dropdown.ToString();
    }


}
