using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherUpdate : MonoBehaviour
{
    public bool flipflop = true;

    public void WeatherActivate(bool turnOn)
    {
        if (turnOn)
        {
            //Debug.Log(this.name + "- Weather turning " + (flipflop ? "ON" : "OFF"));
            if (flipflop)
                WeatherOn();
            else
                WeatherOff();
        }
        else
        {
            //Debug.Log(this.name + "- Weather turning " + (flipflop?"OFF":"ON"));
            if (flipflop)
                WeatherOff();
            else
                WeatherOn();
        }
    }

    /// <summary>
    /// Override this for what to do when weather activates this
    /// </summary>
    public virtual void WeatherOn()
    {
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// Override this for what to do when weather deactivates this
    /// </summary>
    public virtual void WeatherOff()
    {
        this.gameObject.SetActive(false);
    }
}
