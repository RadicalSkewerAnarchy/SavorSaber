using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AIData))]
[RequireComponent(typeof(Animator))]

public class MonsterBehavior : MonoBehaviour
{
    Rigidbody2D RigidBody;
    Animator AnimatorBody;
    AIData AiData;
    MonsterChecks Checks;

    // Some behaviors go for a certain amount of time.
    // timer is complete when < 0
    public float ActionTimer;
    public float ActionTimerReset;
    public float ActionTimerVariance;
    bool left = false;

    #region Attacking
    /// <summary>
    /// where the attack collider will be spawned
    /// </summary>
    protected Vector2 attackSpawnPoint;
    /// <summary>
    /// field for the attack prefab to be spawned when attacking
    /// </summary>
    public GameObject attack;
    /// <summary>
    /// The collider orientation for the melee attack.
    /// </summary>
    protected CapsuleDirection2D attackCapsuleDirection;
    /// <summary>
    /// how much to rotate the attack capsule
    /// Needed for diagonal attack
    /// </summary>
    protected float attackCapsuleRotation;
    public float attackDuration = 1f;
    public bool isAttacking = false;
    #endregion

    private void Start()
    {
        AiData = GetComponent<AIData>();
        Checks = AiData.GetComponent<MonsterChecks>();
        AnimatorBody = GetComponent<Animator>();
        RigidBody = GetComponent<Rigidbody2D>();

        ActionTimer = -1f;
        ActionTimerReset = 5f;
        ActionTimerVariance = 2f;        
    }

    /// <summary>
    /// These actions return a Boolean to verify their completion.
    /// These actions may modify the agents position.
    /// </summary>

    public bool Idle()
    {
        //Debug.Log("I am Idle");
        //Debug.Log(ActionTimer);
        AnimatorBody.Play("Idle");
        AiData.currentBehavior = AIData.Behave.Idle;
        // if done being idle, reset and return true
        // stay idle or dont if time allows
        if (ActionTimer < 0)
        {
            return true;
        }
        else
        {
            ActionTimer -= Time.deltaTime;
            return false;
        }
    }

    public bool MoveTo(Vector2 target, float speed)
    {
        //Debug.Log("I am Chase at " + speed + "mph");
        // Turn Greenish
        AiData.currentBehavior = AIData.Behave.Chase;

        //left = false;

        var current = new Vector2(transform.position.x, transform.position.y);
        // make movement more random
        /*if (Random.Range(0, 100) > 95)
        {
            current = RandomPointAround(current);
        }*/

        // at target
        if (Vector2.Distance(current, target) < 1)
        {
            return true;
        }
        else
        {
            // move towards target
            AnimatorBody.Play("Move");
            target = (target - current);
            target = Vector2.ClampMagnitude(target, speed * Time.deltaTime);
            left = (target.x > 0) ? true : false;
            transform.Translate(target);

            return false;
        }
    }

    public bool MoveFrom(Vector2 target, float speed)
    {
        // Turn Greenish
        AiData.currentBehavior = AIData.Behave.Flee;

        var left = false;

        var current = new Vector2(transform.position.x, transform.position.y);

        {
            // move towards target
            AnimatorBody.Play("Move");
            target = (target - current);
            target = Vector2.ClampMagnitude(target, speed * Time.deltaTime);
            left = (target.x < 0) ? true : false;
            transform.Translate(-1*target);
            return false;
        }
    }

    // FEED
    public bool Feed()
    {
        AnimatorBody.Play("Feed");
        AiData.currentBehavior = AIData.Behave.Feed;
        
        // for all my nearby

        return true;
    }

    // ATTACK
    public bool Attack(Vector2 target, float speed)
    {
        if (!isAttacking)
        {
            isAttacking = true;
            AiData.currentBehavior = AIData.Behave.Attack;
            AnimatorBody.Play("Melee");
            CalculateDirection();
            GameObject newAttack = Instantiate(attack, transform.position, Quaternion.identity, transform);
            CapsuleCollider2D newAttackCollider = newAttack.GetComponent<CapsuleCollider2D>();
            newAttackCollider.size = new Vector2(AiData.MeleeAttackThreshold, AiData.MeleeAttackThreshold);            
            //Debug.Log("ATTACKING");
            //stops attacking for 1 second after attack
            StartCoroutine(EndAttackAfterSeconds(attackDuration, newAttack));
            return true;
        }
        return false;        
    }

    public bool Ranged(Vector2 target, float speed)
    {
        //Debug.Log("I am Attack");
        AiData.currentBehavior = AIData.Behave.Attack;
        AnimatorBody.Play("Ranged");
        return true;
    }

    public bool Socialize(Vector2 target, float speed)
    {
        AnimatorBody.Play("Socialize");
        AiData.currentBehavior = AIData.Behave.Socialize;
        if (ActionTimer < 0)
        {
            // create signal 
            // change signal radius
            // change signal values (++friendliness)
            GameObject obtainSurroundings = Instantiate(Checks.signalPrefab, this.transform, true) as GameObject;
            SignalApplication signalModifier = obtainSurroundings.GetComponent<SignalApplication>();
            signalModifier.SetSignalParameters(null, (AiData.Perception / 2), new Dictionary<string, float>() { { "Friendliness", 0.25f } }, true, false);
            return true;
        }
        else
        {
            ActionTimer -= Time.deltaTime;
            return false;
        }
    }

    void CalculateDirection()
    {
        attackCapsuleDirection = CapsuleDirection2D.Horizontal;
        attackSpawnPoint = new Vector2(transform.position.x + (AiData.MeleeAttackThreshold), transform.position.y + .5f);
    }

    protected IEnumerator EndAttackAfterSeconds(float time, GameObject newAttack)
    {
        yield return new WaitForSeconds(time);
        isAttacking = false;
        Destroy(newAttack);
        yield return null;
    }

    private Vector2 RandomPointAround(Vector2 origin)
    {
        //return origin + new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        Vector2 around = new Vector2(transform.position.x, transform.position.y);
        around = origin - around;
        Vector2.ClampMagnitude(around, 5f); 
        return origin + new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
    }

    // reset action timer
    public void ResetActionTimer()
    {
        ActionTimer = ActionTimerReset + Random.Range(-ActionTimerVariance, ActionTimerVariance);
    }
}
