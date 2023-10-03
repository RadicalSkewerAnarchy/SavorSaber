using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTimeOfDay : MonoBehaviour
{
    public TimeOfDay time;
    private TimeOfDay oldTime;
    // Start is called before the first frame update
    void Awake()
    {
        oldTime = DayNightController.instance.CurrTimeOfDay;

        ForceTime(time, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ForceTime(time, false);
        }
    }


    public void ForceTime(TimeOfDay t, bool skipCallbacks)
    {
        Debug.Log("Forcing time of day: " + t);
        DayNightController.instance.Paused = true;
        DayNightController.instance.SetTimeOfDayImmediate(t, skipCallbacks);   
    }
    public void ResetToPrevious()
    {
        DayNightController.instance.SetTimeOfDay(oldTime);
        DayNightController.instance.Paused = false;
    }
}
