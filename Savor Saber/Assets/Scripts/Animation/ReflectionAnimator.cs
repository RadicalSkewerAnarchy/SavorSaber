using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class ReflectionAnimator : MonoBehaviour
{
    private Animator anim;

    private Animator reflectionAnim;

    // Start is called before the first frame update
    void Start()
    {
        anim = PlayerController.instance.GetComponent<Animator>();
        reflectionAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var currState = anim.GetCurrentAnimatorStateInfo(0);
        reflectionAnim.Play(currState.fullPathHash);
        reflectionAnim.SetFloat("Direction", anim.GetFloat("Direction"));
        reflectionAnim.SetBool("Moving", anim.GetBool("Moving"));
        reflectionAnim.SetBool("Running", anim.GetBool("Running"));
        reflectionAnim.SetBool("Dashing", anim.GetBool("Dashing"));
        reflectionAnim.SetBool("Riding", anim.GetBool("Riding"));

    }
}
