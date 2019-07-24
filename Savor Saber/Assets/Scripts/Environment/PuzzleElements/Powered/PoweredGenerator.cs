using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PoweredGenerator : PoweredObject
{
    [SerializeField]
    private PoweredObject[] targetObjects;

    //components
    private Animator animator;
    private Light generatorLight;
    private PlaySFX sfxPlayer;
    private AudioSource constantOnSound;

    // sfx fields
    public AudioClip startSound;
    public AudioClip stopSound;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sfxPlayer = GetComponent<PlaySFX>();
        constantOnSound = GetComponent<AudioSource>();
        generatorLight = GetComponentInChildren<Light>();
        if (active)
            TurnOn();
        else
            ShutOff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TurnOn()
    {
        if (!active)
        {
            base.TurnOn();
            foreach (PoweredObject target in targetObjects)
            {
                target.TurnOn();
            }
            animator.Play("On");
            generatorLight.color = Color.green;
            sfxPlayer.Play(startSound);
            constantOnSound.Play();
        }

    }

    public override void ShutOff()
    {
        if (active)
        {
            base.ShutOff();
            foreach (PoweredObject target in targetObjects)
            {
                target.ShutOff();
            }
            animator.Play("Off");
            generatorLight.color = Color.red;
            sfxPlayer.Play(stopSound);
            constantOnSound.Stop();
        }


    }
}
