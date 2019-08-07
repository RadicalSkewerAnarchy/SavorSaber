using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiWeatherUpdate : WeatherUpdate
{
    public List<GameObject> OtherWeather = new List<GameObject>();

    public override void WeatherOn()
    {
        this.gameObject.SetActive(true);

        WeatherUpdate wu;
        foreach (GameObject go in OtherWeather)
        {
            Debug.Log(go.name + " performing a multiweather state");
            foreach (Transform t in go.transform)
            {

                Debug.Log(t.name + " activating component");
                wu = t.GetComponent<WeatherUpdate>();
                wu.WeatherActivate(wu.flipflop);
            }
        }
    }

    public override void WeatherOff()
    {
        this.gameObject.SetActive(false);
    }

}
