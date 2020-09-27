using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireWeatherUpdate : WeatherUpdate
{
    public Sprite campUnlit;

    SpriteRenderer sr;
    Animator an;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();
    }

    /// <summary>
    /// Override this for what to do when weather activates this
    /// </summary>
    public override void WeatherOn()
    {
        an.enabled = false;
        sr.sprite = campUnlit;
    }

    /// <summary>
    /// Override this for what to do when weather deactivates this
    /// </summary>
    public override void WeatherOff()
    {
        an.enabled = true;
    }
}
