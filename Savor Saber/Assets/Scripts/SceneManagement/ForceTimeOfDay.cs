using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTimeOfDay : MonoBehaviour
{
    public TimeOfDay time;
    private TimeOfDay oldTime;
    private DayNightController instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = DayNightController.instance;
        oldTime = instance.CurrTimeOfDay;

        ForceTime(time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ForceTime(TimeOfDay t)
    {
        instance.SetTimeOfDay(t);
    }
    public void ResetToPrevious()
    {
        instance.SetTimeOfDay(oldTime);
        //instance.Paused = false;
    }
}
