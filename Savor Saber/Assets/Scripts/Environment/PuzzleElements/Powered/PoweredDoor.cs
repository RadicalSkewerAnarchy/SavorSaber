using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PoweredDoor : PoweredObject
{

    private SpriteRenderer sr;
    private Animator gateAnimator;
    private Collider2D blocker;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        gateAnimator = GetComponent<Animator>();
        blocker = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TurnOn()
    {
        base.TurnOn();
        //sr.color = new Color(0, 0, 0, 0);
        blocker.enabled = false;
        gateAnimator.Play("Open");
    }

    public override void ShutOff()
    {
        base.ShutOff();

        //sr.color = Color.white;
        blocker.enabled = true;
        gateAnimator.Play("Close");
    }
}

