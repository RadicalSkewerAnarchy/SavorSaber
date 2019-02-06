using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializableCollections;

public enum WeatherType
{
    Sun,
    Rain,
    Hail,
    Snow,
    RaininMen,
}
[RequireComponent(typeof(WindController))]
public class WeatherController : MonoBehaviour
{
    public WeatherType _weather = WeatherType.Sun;
    public WeatherType Weather { get => _weather; set => SetWeather(value); }
    public WeatherDict weatherData = new WeatherDict();
    private WindController windController;
    private Weather weatherObj;

    private void Start()
    {
        windController = GetComponent<WindController>();
        weatherObj = Instantiate(weatherData[_weather].effectCreator, transform).GetComponent<Weather>();
    }

    /// <summary> Change the current weather. will allow crossfading in the future </summary>
    private void SetWeather(WeatherType w)
    {
        if (w == _weather)
            return;
        Destroy(weatherObj.gameObject);
        weatherObj = Instantiate(weatherData[w].effectCreator, transform).GetComponent<Weather>();
        _weather = w;
    }
    [System.Serializable] public class WeatherDict : SDictionary<WeatherType, WeatherData> { }
}
