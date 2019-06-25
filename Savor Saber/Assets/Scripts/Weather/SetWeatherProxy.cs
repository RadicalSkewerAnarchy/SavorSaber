 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class designed to allow unity events to access the weather controller by the instance reference
/// </summary>
public class SetWeatherProxy : MonoBehaviour
{
    public AreaChange areaWeather;
    public void SetWeather(string weather)
    {
        var weatherType = WeatherType.Sun;
        if (weather == "rain")
            weatherType = WeatherType.Rain;
        else if (weather == "snow")
            weatherType = WeatherType.Snow;
        WeatherController.instance.Weather = weatherType;
        if (areaWeather != null)
            areaWeather.weather = weatherType;
    }
}
