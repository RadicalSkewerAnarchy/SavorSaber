using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PoweredLight : PoweredObject
{

    public Sprite onSprite;
    public Sprite offSprite;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TurnOn()
    {
        base.TurnOn();
        sr.sprite = onSprite;
    }

    public override void ShutOff()
    {
        base.ShutOff();
        sr.sprite = offSprite;
    }
}
