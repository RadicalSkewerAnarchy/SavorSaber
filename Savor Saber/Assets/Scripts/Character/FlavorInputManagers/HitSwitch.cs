using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class HitSwitch : FlavorInputManager
{

    public PoweredObject[] TargetObjects;
    private bool active = false;
    private AudioSource burnSFXPlayer;
    // Start is called before the first frame update
    void Start()
    {
        burnSFXPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Feed(IngredientData ingredient, bool fedByPlayer, CharacterData feeder)
    {
        if (active)
        {
            foreach (PoweredObject target in TargetObjects)
            {
                //target.ShutOff();
            }
            active = false;
        }
        else
        {
            foreach (PoweredObject target in TargetObjects)
            {
                target.TurnOn();
            }
            active = true;
            burnSFXPlayer.Play();
        }
    }

}
