 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class designed to allow unity events to access the weather controller by the instance reference
/// </summary>
public class SetWeatherProxy : MonoBehaviour
{
    public void SetWeather(string weather)
    {
        if (weather == "rain")
            WeatherController.instance.Weather = WeatherType.Rain;
        else if (weather == "snow")
            WeatherController.instance.Weather = WeatherType.Snow;
        else
            WeatherController.instance.Weather = WeatherType.Sun;
    }
}
