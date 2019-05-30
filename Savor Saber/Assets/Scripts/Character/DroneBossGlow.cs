using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DroneBossGlow : MonoBehaviour
{

    Animator glowAnimator;
    // Start is called before the first frame update
    void Start()
    {
        glowAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Glow()
    {
        glowAnimator.Play("Socialize");
    }


}
