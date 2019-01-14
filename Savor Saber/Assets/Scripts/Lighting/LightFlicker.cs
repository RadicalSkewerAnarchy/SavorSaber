using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour, IPausable {

    public Light l;
    public float intensityGainMin = 0.1f;
    public float intensityGainMax = 1f;
    public float flickerTimeMin = 0.1f;
    public float flickerTimeMax = 1f;
    private float initialIntensity;
    private float initialRange;

    public bool Paused { get; set; }

    // Use this for initialization
    void Start () {
        initialIntensity = l.intensity;
        initialRange = l.range;
        StartCoroutine(flickerCr(initialIntensity + Random.Range(intensityGainMin, intensityGainMax)));
    }
    private IEnumerator flickerCr(float goalIntensity)
    {
        float flickerTime = Random.Range(flickerTimeMin, flickerTimeMax);
        float halfTime = flickerTime / 2;
        float currTime = 0;
        float gain = goalIntensity - initialIntensity;
        while(currTime < halfTime)
        {
            yield return new WaitWhile(() => Paused);
            yield return new WaitForEndOfFrame();
            float increment = gain * (Time.deltaTime / halfTime);
            l.intensity += increment;
            l.range += increment * 0.5f;
            currTime += Time.deltaTime;
        }
        while (currTime < flickerTime)
        {
            yield return new WaitWhile(() => Paused);
            yield return new WaitForEndOfFrame();
            float increment = gain * (Time.deltaTime / halfTime);
            l.intensity -= increment;
            l.range -= increment * 0.5f;
            currTime += Time.deltaTime;
        }
        l.intensity = initialIntensity;
        l.range = initialRange;
        StartCoroutine(flickerCr(initialIntensity + Random.Range(intensityGainMin, intensityGainMax)));
    }
}
