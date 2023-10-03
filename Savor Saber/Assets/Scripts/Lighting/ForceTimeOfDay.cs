using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTimeOfDay : MonoBehaviour
{
    public TimeOfDay time;
    private TimeOfDay oldTime;
    [SerializeField]
    private DayNightController instance;
    private bool timeSet = false;
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
        if (Input.GetKeyDown(KeyCode.O))
        {
            ForceTime(time);
        }
    }


    public void ForceTime(TimeOfDay t)
    {
        Debug.Log("Forcing time of day: " + t); 
        instance.Paused = true;
        instance.SetTimeOfDayImmediate(t);   
    }
    public void ResetToPrevious()
    {
        instance.SetTimeOfDay(oldTime);
        instance.Paused = false;
    }
}
