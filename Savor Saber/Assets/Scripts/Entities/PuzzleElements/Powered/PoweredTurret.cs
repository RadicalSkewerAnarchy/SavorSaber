using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredTurret : PoweredObject
{
    public Animator beamAnimator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TurnOn()
    {
        base.TurnOn();
        beamAnimator.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        beamAnimator.SetTrigger("Fire");
        ShutOff();

    }
}
