using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSun : Weather
{
    public override float Intensity { get; set; }
    public override AudioSource AmbientSound { get => GetComponent<AudioSource>(); }
    public override MathUtils.FloatRange WindRange { get; }
    public override float DestroyTime => 0;
}
