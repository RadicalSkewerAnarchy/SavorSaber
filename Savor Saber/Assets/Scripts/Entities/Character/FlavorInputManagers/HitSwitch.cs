using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class HitSwitch : FlavorInputManager
{
    public bool isToggle = true;
    public int cooldownTime = 0; //if 0, effect is permanent
    public PoweredObject[] TargetObjects;
    private bool targetsActive = false;
    private AudioSource burnSFXPlayer;


    private WaitForSeconds CooldownTimer;

    // Start is called before the first frame update
    void Start()
    {
        burnSFXPlayer = GetComponent<AudioSource>();
        CooldownTimer = new WaitForSeconds(cooldownTime);
        if (cooldownTime > 0)
            isToggle = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Feed(IngredientData ingredient, bool fedByPlayer, CharacterData feeder)
    {
        if (targetsActive)
        {
            foreach (PoweredObject target in TargetObjects)
            {
                target.ShutOff();
            }

            if(isToggle)
                targetsActive = false;

            if (cooldownTime > 0)
                StartCoroutine(StartCooldown());
        }
        else
        {
            foreach (PoweredObject target in TargetObjects)
            {
                target.TurnOn();
            }

            if(isToggle)
                targetsActive = true;

            if (cooldownTime > 0)
                StartCoroutine(StartCooldown());

            burnSFXPlayer.Play();
        }
    }

    private IEnumerator StartCooldown()
    {
        yield return CooldownTimer;

        if (targetsActive)
        {
            foreach (PoweredObject target in TargetObjects)
            {
                target.ShutOff();
            }

            if (isToggle)
                targetsActive = false;
        }
        else
        {
            foreach (PoweredObject target in TargetObjects)
            {
                target.TurnOn();
            }

            if (isToggle)
                targetsActive = true;

            burnSFXPlayer.Play();
        }

        yield return null;
    }

    

}
