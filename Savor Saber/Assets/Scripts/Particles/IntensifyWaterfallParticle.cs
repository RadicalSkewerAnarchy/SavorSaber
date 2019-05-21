using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class IntensifyWaterfallParticle : MonoBehaviour
{
    ParticleSystem Particles;
    public float intensity;
    public float particleMinSize;
    public float particleMaxSize;
    public float startSpeed;
    public float steamOpacity;
    public Color steamColor;
    public Color waterColor;
    void Start()
    {
        Particles = GetComponent<ParticleSystem>();
        intensity = 0f;
        particleMinSize = Particles.startSize;
        startSpeed = Particles.startSpeed;
        steamOpacity = 4f;
        steamColor = new Color(255, 255, 255);
        waterColor = new Color(0, 253, 255);
        // GetComponent<Renderer>().sortingLayerName = ("AboveObjects");
        foreach(var render in GetComponentsInChildren<Renderer>())
        {
            // render.sortingLayerName = ("AboveObjects");
        }
    }

    
}
