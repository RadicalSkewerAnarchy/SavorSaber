﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSun : Weather
{
    public override float Intensity { get; set; }
    public override AudioSource AmbientSound { get; }
}
