using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class TimeOfDayLight : MonoBehaviour
{
    public float speed;
    public float dayTimeIntensity;
    private Light l;
    private float initialIntensity;
    private bool transitioning = false;
    private LightFlicker lightFlicker;
    // Start is called before the first frame update
    void Start()
    {
        l = GetComponent<Light>();
        lightFlicker = GetComponent<LightFlicker>();
        initialIntensity = l.intensity;
        if(DayNightController.instance.IsDayTime)
        {
            l.intensity = dayTimeIntensity;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(DayNightController.instance.IsDayTime)
        {
            if(!transitioning && l.intensity > (lightFlicker == null ? dayTimeIntensity : dayTimeIntensity + lightFlicker.intensityGainMax))
            {
                transitioning = true;
                StartCoroutine(GoToDayMode());
            }
        }
        else if (DayNightController.instance.IsNightTime)
        {
            if (!transitioning && (l.intensity < initialIntensity))
            {
                transitioning = true;
                StartCoroutine(GoToNightMode());
            }
        }
    }

    IEnumerator GoToDayMode()
    { 
        while(l.intensity > dayTimeIntensity)
        {
            yield return new WaitForEndOfFrame();
            l.intensity -= Time.deltaTime * speed;
            if(lightFlicker != null)
                lightFlicker.initialIntensity = l.intensity;
        }
        l.intensity = dayTimeIntensity;
        if (lightFlicker != null)
            lightFlicker.initialIntensity = l.intensity;
        transitioning = false;
    }

    IEnumerator GoToNightMode()
    {
        while (l.intensity < initialIntensity)
        {
            yield return new WaitForEndOfFrame();
            l.intensity += Time.deltaTime * speed;
            if (lightFlicker != null)
                lightFlicker.initialIntensity = l.intensity;
        }
        l.intensity = initialIntensity;
        if (lightFlicker != null)
            lightFlicker.initialIntensity = l.intensity;
        transitioning = false;
    }
}
