using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PoweredGenerator : PoweredObject
{
    [SerializeField]
    private PoweredObject[] targetObjects;
    private Animator animator;
    private Light generatorLight;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        generatorLight = GetComponentInChildren<Light>();
        if (active)
            TurnOn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TurnOn()
    {
        base.TurnOn();
        foreach(PoweredObject target in targetObjects)
        {
            target.TurnOn();
        }
        animator.Play("On");
        generatorLight.color = Color.green;
    }

    public override void ShutOff()
    {
        base.ShutOff();
        foreach (PoweredObject target in targetObjects)
        {
            target.ShutOff();
        }
        animator.Play("Off");
        generatorLight.color = Color.red;
    }
}
