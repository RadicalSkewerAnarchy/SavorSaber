using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightControllerProxy : MonoBehaviour
{
    private DayNightController controller;
    // Start is called before the first frame update
    void Start()
    {
        //controller = GameObject.FindObjectOfType<DayNightController>();
        controller = DayNightController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LengthOfDay(float length)
    {
        controller.LengthOfDay = length;
    }

    public void Next()
    {
        controller.Next();
    }

    public void SetTimeScale(float scale)
    {
        
    }

    public void Paused(bool pause)
    {
        controller.Paused = pause;
    }

    public void GameHour(float hour)
    {
        controller.GameHour = hour;
    }

    public void GoToDay()
    {
        controller.GoToDay();
    }
}

