using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystemForceField))]
public class WindController : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField]
    private float _intensity = 0;
    public float Intensity { get => _intensity; set => SetIntensity(value); }
    public Direction direction = Direction.East;
    public MathUtils.FloatRange magnitude = new MathUtils.FloatRange(0, 40);

    private ParticleSystemForceField pf;
    // Start is called before the first frame update
    void Start()
    {
        pf = GetComponent<ParticleSystemForceField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            SetIntensity(_intensity);
    }

    public void SetIntensity(float value)
    {
        _intensity = value;
        pf.directionX = direction == Direction.East ? magnitude.Lerp(value) : -magnitude.Lerp(value);
    }
}
