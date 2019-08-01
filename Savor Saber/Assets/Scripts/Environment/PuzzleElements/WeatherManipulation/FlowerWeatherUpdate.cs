using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerWeatherUpdate : WeatherUpdate
{
    MarshPlantFlavorInput plantManager;

    // Start is called before the first frame update
    void Start()
    {
        plantManager = GetComponent<MarshPlantFlavorInput>();
    }

    /// <summary>
    /// Override this for what to do when weather activates this
    /// </summary>
    public override void WeatherOn()
    {
        if (plantManager.isOpen)
            plantManager.ClosePlant();
    }

    /// <summary>
    /// Override this for what to do when weather deactivates this
    /// </summary>
    public override void WeatherOff()
    {
        if (!plantManager.isOpen)
            plantManager.OpenPlant();
    }
}
