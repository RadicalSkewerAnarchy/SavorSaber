using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class ExtendingBridge : PoweredObject
{
    private Animator animator;
    private BoxCollider2D blocker;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        blocker = GetComponent<BoxCollider2D>();

        InitialFlagSet();

        if (FlagManager.GetFlag(activeStateFlag) == "True")
        {
            animator.Play("StartExtended");
            blocker.enabled = false;
            active = true;
        }
        else if (FlagManager.GetFlag(activeStateFlag) == "False")
        {
            animator.Play("StartRetracted");
            blocker.enabled = true;
            active = false;
        }
        else
        {
            //if no existing flag state, default to inspector settings
            if (active)
            {
                TurnOn();
            }
            else
            {
                ShutOff();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TurnOn()
    {
        base.TurnOn();
        animator.Play("Extend");
        blocker.enabled = false;
        active = true;
    }

    public override void ShutOff()
    {
        Debug.Log("Extending Bridge: ShutOff");
        base.ShutOff();
        animator.Play("Retract");
        blocker.enabled = true;
        active = false;
    }
}
