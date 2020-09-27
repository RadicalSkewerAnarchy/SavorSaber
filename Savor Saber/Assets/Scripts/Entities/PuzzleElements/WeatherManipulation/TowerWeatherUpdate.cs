using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerWeatherUpdate : WeatherUpdate
{
    public override void WeatherOn()
    {
        foreach(Transform t in this.transform)
        {
            t.gameObject.SetActive(true);
        }
    }

    public override void WeatherOff()
    {
        foreach (Transform t in this.transform)
        {
            t.gameObject.SetActive(false);
        }
    }
}
