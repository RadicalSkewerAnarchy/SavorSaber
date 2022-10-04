using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLimitBreak : PoweredObject
{
    [SerializeField]
    protected string displayName;
    [SerializeField]
    protected AudioClip sfx;
    [SerializeField]
    protected ParticleSystem vfx;
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected float duration = 5;

    protected WaitForSeconds activeTimer;

    public bool selfTerminate = true;

    public PoweredObject[] effects;
    public GameObject[] objectsToEnable;
    
    // Start is called before the first frame update
    void Start()
    {
        activeTimer = new WaitForSeconds(duration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TurnOn()
    {
        base.TurnOn();
        animator.SetTrigger("LimitBreak");
        vfx.Play();

        foreach(PoweredObject target in effects)
            target.TurnOn();
        foreach(GameObject targetObject in objectsToEnable)
            targetObject.SetActive(true);

        if (selfTerminate)
            StartCoroutine(StartActiveTimer());
    }


    public override void ShutOff()
    {
        base.ShutOff();

        foreach (PoweredObject target in effects)
            target.ShutOff();
        foreach (GameObject targetObject in objectsToEnable)
            targetObject.SetActive(false);
    }

    protected IEnumerator StartActiveTimer()
    {
        yield return activeTimer;
        ShutOff();
        yield return null;
    }
}
