using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathUtils;

public class WeatherSnow : Weather
{
    [Range(0, 1)]
    [SerializeField]
    private float _intensity = 0;
    public override float Intensity { get => _intensity; set => SetIntensity(value); }
    private AudioSource _ambient;
    public override AudioSource AmbientSound => _ambient;
    [SerializeField]
    private FloatRange _windRange = new FloatRange(0, 40);
    public override FloatRange WindRange => _windRange;
    public override float DestroyTime => 10;

    public MathUtils.FloatRange emissionOverTime = new MathUtils.FloatRange(0, 40);
    public MathUtils.FloatRange emissionOverDistance = new MathUtils.FloatRange(0, 6);
    public MathUtils.FloatRange subEmissionOverTime = new MathUtils.FloatRange(0, 10);
    public MathUtils.FloatRange gravityMinMax = new MathUtils.FloatRange(0, 0.4f);


    private ParticleSystem mainEmitter;
    private ParticleSystem bottomEmitter;
    private ParticleSystem[] sideEmitters = new ParticleSystem[2];

    private void Awake()
    {
        _ambient = GetComponent<AudioSource>();
        var emitters = GetComponentsInChildren<ParticleSystem>();
        mainEmitter = emitters[0];
        bottomEmitter = emitters[1];
        sideEmitters[0] = emitters[2];
        sideEmitters[1] = emitters[3];
    }

    private void SetIntensity(float intensity)
    {
        _intensity = intensity;
        SetEmitter(mainEmitter, intensity, emissionOverTime);
        SetEmitter(bottomEmitter, intensity, subEmissionOverTime);
        SetEmitter(sideEmitters[0], intensity, subEmissionOverTime);
        SetEmitter(sideEmitters[1], intensity, subEmissionOverTime);
    }

    private void SetEmitter(ParticleSystem ps, float intensity, FloatRange emissionOT)
    {
        var m = ps.main;
        m.gravityModifier = gravityMinMax.Lerp(intensity);
        var e = ps.emission;
        var rot = e.rateOverTime;
        rot.constantMin = emissionOT.Lerp(intensity);
        rot.constantMax = rot.constantMin + 5 * Intensity;
        e.rateOverTime = rot;
        var rod = e.rateOverDistance;
        rod.constant = emissionOverDistance.Lerp(intensity);
        e.rateOverDistance = rod;
    }
}
