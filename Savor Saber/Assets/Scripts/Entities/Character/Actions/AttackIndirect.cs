using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackIndirect : PoweredObject
{
    public bool startActive = false;
    public string tagToTarget;
    public float cooldown = 3;
    public int reacquireTargetThreshhold = 3;
    public int damage = 1;
    private int attacksMade = 0;


    public GameObject targetMarkerVfx;
    private AttackIndirectDamageApplicator targetMarkerDamageApplicator;
    private Animator TMVAnimator;
    private AudioSource TMVSfx;
    private GameObject[] targets;
    private GameObject currentTarget;
    private CharacterData targetData;

    private WaitForSeconds Tic;


    // Start is called before the first frame update
    void Start()
    {
        Tic = new WaitForSeconds(cooldown);
        if (targetMarkerVfx != null)
        {
            TMVAnimator = targetMarkerVfx.GetComponent<Animator>();
            TMVAnimator.Play("Idle");
            TMVSfx = targetMarkerVfx.GetComponent<AudioSource>();
            targetMarkerDamageApplicator = targetMarkerVfx.GetComponent<AttackIndirectDamageApplicator>();
            targetMarkerDamageApplicator.damage = damage;
        }
        GenerateTargetList();
        if (startActive)
        {
            TurnOn();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TurnOn()
    {
        base.TurnOn();
        MarkTarget();
        Cooldown();
    }

    public override void ShutOff()
    {
        base.ShutOff();
        StopAllCoroutines();
    }

    private void GenerateTargetList()
    {
        targets = GameObject.FindGameObjectsWithTag(tagToTarget);
    }


    private void MarkTarget()
    {
        currentTarget = targets[Random.Range(0, targets.Length)];
        Debug.Log("Marked target as " + currentTarget);
        //failsafe for if we select a dead/missing target
        if(currentTarget == null)
        {
            for(int i = 0; i < targets.Length; i++)
            {
                if (targets[i] != null) currentTarget = targets[i];
            }
        }
        targetData = currentTarget.GetComponent<CharacterData>();
        targetMarkerVfx.transform.position = currentTarget.transform.position;
        targetMarkerDamageApplicator.targetData = targetData;
        
        //targetMarkerVfx.transform.parent = currentTarget.transform;

    }

    private void Cooldown()
    {
        if(currentTarget == null)
        {
            MarkTarget();
            if(currentTarget == null)
            {
                GenerateTargetList();
                MarkTarget();
                if(currentTarget == null)
                {
                    Debug.Log("Indirect unit ran out of targets...");
                    ShutOff();
                    return;
                }
            }
        }

        if (active)
        {
            targetMarkerVfx.transform.position = currentTarget.transform.position;
            //targetData.DoDamage(damage);
            Debug.Log("Animating indirect attack...");
            TMVAnimator.SetTrigger("Attack");
            TMVSfx.Play();
            attacksMade++;
            if(attacksMade >= reacquireTargetThreshhold)
            {
                MarkTarget(); //find a new random target
                attacksMade = 0;
            }
            StartCoroutine(ExecuteAfterSeconds());
        }
    }

    private IEnumerator ExecuteAfterSeconds()
    {
        yield return Tic;
        Cooldown();
    }

}
