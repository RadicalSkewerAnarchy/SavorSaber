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
    private bool dayMode = false;
    // Start is called before the first frame update
    void Start()
    {
        l = GetComponent<Light>();
        initialIntensity = l.intensity;
        if(DayNightController.instance.IsDayTime)
        {
            l.intensity = dayTimeIntensity;
            dayMode = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(DayNightController.instance.IsDayTime)
        {
            if(!dayMode)
            {
                dayMode = true;
                StartCoroutine(GoToDayMode());
            }
        }
        else if (DayNightController.instance.IsNightTime)
        {
            if (dayMode)
            {
                dayMode = false;
                StartCoroutine(GoToNightMode());
            }
        }
    }

    IEnumerator GoToDayMode()
    {
        var lightFlicker = GetComponent<LightFlicker>();      
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
    }

    IEnumerator GoToNightMode()
    {
        var lightFlicker = GetComponent<LightFlicker>();
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
    }
}
