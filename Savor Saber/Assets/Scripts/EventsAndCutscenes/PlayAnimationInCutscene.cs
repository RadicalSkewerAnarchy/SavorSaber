using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationInCutscene : MonoBehaviour
{

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimationTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }

    public void PlayAnimationName(string name)
    {
        anim.Play(name);
    }

    public void MoveCharacter(Transform target)
    {
        anim.gameObject.transform.position = target.position;
    }
}
