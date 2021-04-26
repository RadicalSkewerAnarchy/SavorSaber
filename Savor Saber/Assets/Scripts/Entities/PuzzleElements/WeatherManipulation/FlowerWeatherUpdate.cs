using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerWeatherUpdate : WeatherUpdate
{
    PlantFlavorInput plantManager;
    public bool openWithWeather = true;

    // Start is called before the first frame update
    void Awake()
    {
        plantManager = this.GetComponent<PlantFlavorInput>();
    }

    /// <summary>
    /// Override this for what to do when weather activates this
    /// </summary>
    public override void WeatherOn()
    {
        if (openWithWeather)
        {
            if (!plantManager.isOpen)
                plantManager.OpenPlant();
        }
        else
        {
            if (plantManager != null && plantManager.isOpen)
                plantManager.ClosePlant();
        }
    }

    /// <summary>
    /// Override this for what to do when weather deactivates this
    /// </summary>
    public override void WeatherOff()
    {
        if (openWithWeather)
        {
            if (plantManager.isOpen)
                plantManager.ClosePlant();

        }
        else
        {
            if (!plantManager.isOpen)
                plantManager.OpenPlant();
        }
    }
}
