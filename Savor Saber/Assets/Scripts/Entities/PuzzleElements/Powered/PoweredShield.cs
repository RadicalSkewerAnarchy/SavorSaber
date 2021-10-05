using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredShield : PoweredObject
{
    public GameObject shield;
    private AudioSource shieldSFX;

    private bool allowSound = false;

    // Start is called before the first frame update
    void Start()
    {
        shieldSFX = GetComponent<AudioSource>();
        InitialFlagSet();

        if (FlagManager.GetFlag(activeStateFlag) == "True")
        {
            TurnOn();
        }
        else if (FlagManager.GetFlag(activeStateFlag) == "False")
        {
            ShutOff();
        }
        else
        {
            //if no existing flag state, default to inspector settings
            if (active)
                TurnOn();
            else
                ShutOff();
        }

        allowSound = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TurnOn()
    {
        //Debug.Log("Turning on Powered Shield");
        base.TurnOn();
        shield.SetActive(true);
        if (allowSound)
        {
            shieldSFX.pitch = 3;
            shieldSFX.Play();
        }

    }

    public override void ShutOff()
    {
        //Debug.Log("Turning off Powered Shield");
        base.ShutOff();
        shield.SetActive(false);
        if (allowSound)
        {
            shieldSFX.pitch = 0.7f;
            shieldSFX.Play();
        }

    }
}
