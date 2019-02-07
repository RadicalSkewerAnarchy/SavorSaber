using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherRain : Weather
{
    [Range(0, 1)]
    [SerializeField]
    private float _intensity = 0;
    public override float Intensity { get => _intensity; set => SetIntensity(value); }
    private AudioSource _ambient;
    public override AudioSource AmbientSound { get => _ambient; }
    public MathUtils.FloatRange emissionMinMax = new MathUtils.FloatRange(10, 20);
    public MathUtils.FloatRange gravityMinMax = new MathUtils.FloatRange(5, 9);
    public float sizeBase = 0.025f;
    public float sizeStep = 0.025f;
    public float trailWidthBase = 0.025f;
    public float trailWidthStep = 0.025f;
    public float trailTimeBase = 0.025f;
    public float trailTimeStep = 0.025f;
    public float splashSizeBase = 0.025f;
    public float splashSizeStep = 0.025f;
    private ParticleSystem[] emitters;
    private ParticleSystem[] splashers;

    // Start is called before the first frame update
    void Start()
    {
        _ambient = GetComponent<AudioSource>();
        var emitterPairs = GetComponentsInChildren<ParticleSystem>();
        emitters = new ParticleSystem[emitterPairs.Length / 2];
        splashers = new ParticleSystem[emitterPairs.Length / 2];
        for (int i = 0; i < emitterPairs.Length /2; ++i)
        {
            emitters[i] = emitterPairs[2 * i];
            splashers[i] = emitterPairs[(2 * i) + 1];
        }
        for (int i = 0; i < emitters.Length; ++i)
        {
            var ps = emitters[i];
            var m = ps.main;
            m.startSize = sizeBase + (i * sizeStep);
            var trails = ps.trails;
            trails.widthOverTrail = trailWidthBase + (i * trailWidthStep);
            trails.lifetime = trailTimeBase + (i * trailWidthStep);
            var sp = splashers[i].main;
            sp.startSize = splashSizeBase + (i * splashSizeStep);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            SetIntensity(_intensity);
    }

    private void SetIntensity(float intensity)
    {
        _intensity = intensity;
        for(int i = 0; i < emitters.Length; ++i)
        {
            var ps = emitters[i];
            var m = ps.main;
            m.gravityModifier = gravityMinMax.Lerp(intensity);
            var e = ps.emission;
            var rot = e.rateOverTime;
            rot.constantMin = emissionMinMax.Lerp(intensity);
            rot.constantMax = rot.constantMin + 5 * Intensity;
            e.rateOverTime = rot;
        }
    }
}
