using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Area
{
    Plains,
    Marsh,
    Desert
}


[RequireComponent(typeof(Collider2D))]
public class AreaChange : MonoBehaviour
{
    public static Area CurrArea = Area.Plains;
    public Area area;
    public WeatherType weather;
    public WeatherData lightProfile;
    public AudioClip nightMusic;
    public AudioClip dayMusic;
    public AudioClip nightBgs;
    public AudioClip dayBgs;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Temp: disable area change during party
        if (CurrArea == area)
            return;
        BGMManager.instance.AreaBgmDay = dayMusic;
        BGMManager.instance.AreaBgmNight = nightMusic;
        BGMManager.instance.AreaBgsDay = dayBgs;
        BGMManager.instance.AreaBgsNight = nightBgs;

        if (FlagManager.GetFlag("party") != "true")
        {
            BGMManager.instance.FadeToAreaSounds();
            WeatherController.instance.Weather = weather;
        }
        DayNightController.instance.currWeather = lightProfile;
        CurrArea = area;
    }
}
