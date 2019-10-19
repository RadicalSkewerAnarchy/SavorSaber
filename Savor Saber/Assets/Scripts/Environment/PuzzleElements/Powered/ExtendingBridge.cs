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
        if (active)
        {
            animator.Play("StartExtended");
            blocker.enabled = false;
        }
        else
        {
            animator.Play("StartRetracted");
            blocker.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TurnOn()
    {
        animator.Play("Extend");
        blocker.enabled = false;
        active = true;
    }

    public override void ShutOff()
    {
        animator.Play("Retract");
        blocker.enabled = true;
        active = false;
    }
}
