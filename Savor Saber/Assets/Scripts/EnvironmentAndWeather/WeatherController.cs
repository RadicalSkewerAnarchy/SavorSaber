using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializableCollections;

public enum WeatherType
{
    None = -1,
    Sun,
    Rain,
    Hail,
    Snow,
    RaininMen,
}
[RequireComponent(typeof(WindController))]
public class WeatherController : MonoBehaviour
{
    private WeatherType _weather = WeatherType.Sun;
    public WeatherType Weather { get => _weather; set => SetWeather(value); }
    private bool fading = false;
    private float fadeTime = 12;
    private WeatherType buffer = WeatherType.None;
    public WeatherType Buffer => buffer;

    public WeatherDict weatherData = new WeatherDict();
    private WindController windController;
    private Weather weatherObj;

    private void Start()
    {
        windController = GetComponent<WindController>();
        weatherObj = Instantiate(weatherData[_weather].effectCreator, transform).GetComponent<Weather>();
    }

    /// <summary> Change the current weather. cross fades. </summary>
    private void SetWeather(WeatherType w, float intensity = 0.5f)
    {
        Debug.Log(w);
        if (w == _weather)
        {
            buffer = (WeatherType)(-1);
            return;
        }
        else if(fading)
        {
            buffer = w;
            return;
        }
        var next = Instantiate(weatherData[w].effectCreator, transform).GetComponent<Weather>();
        fading = true;
        _weather = w;
        StartCoroutine(crossFadeWeather(weatherObj, next, fadeTime, intensity));       
    }

    private IEnumerator crossFadeWeather(Weather curr, Weather next, float time, float goalIntensity)
    {
        float diffA = curr.Intensity;
        float diffB = goalIntensity;
        while (next.Intensity < goalIntensity)
        {
            yield return new WaitForEndOfFrame();
            float percent = (Time.deltaTime / time);
            next.Intensity += percent * diffB;
            curr.Intensity -= percent * diffB;
        }
        Destroy(curr.gameObject, curr.DestroyTime);
        next.Intensity = goalIntensity;
        weatherObj = next;
        if(buffer != WeatherType.None && buffer != _weather)
        {
            _weather = buffer;
            buffer = WeatherType.None;
            yield return crossFadeWeather(weatherObj, Instantiate(weatherData[_weather].effectCreator, transform).GetComponent<Weather>(), time, goalIntensity);
        }
        buffer = WeatherType.None;
        fading = false;
    }

    [System.Serializable] public class WeatherDict : SDictionary<WeatherType, WeatherData> { }
}
