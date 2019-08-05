using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherUpdate : MonoBehaviour
{
    public void WeatherActivate(bool turnOn)
    {
        if (turnOn)
        {
            Debug.Log(this.name + "- Weather turning ON");
            WeatherOn();
        }
        else
        {
            Debug.Log(this.name + "- Weather turning OFF");
            WeatherOff();
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
