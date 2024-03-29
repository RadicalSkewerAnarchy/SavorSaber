﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Weather : MonoBehaviour
{
    public abstract float Intensity { get; set; }
    public abstract MathUtils.FloatRange WindRange { get; }
    public abstract AudioSource AmbientSound { get; }
    public abstract float DestroyTime { get; }
}
