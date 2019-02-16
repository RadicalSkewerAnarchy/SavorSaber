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
    public float ResetTimer;
    public float ResetTimerReset;
    public float ResetTimerVariance;
    bool left = false;

    // biases
    private float biasAngle = 15f;
    private float biasAngleMod;
    private float biasMovementAngle;

    #region Attacking
    public enum Direction : int
    {
        East,
        NorthEast,
        North,
        NorthWest,
        West,
        SouthWest,
        South,
        SouthEast,
    }
    /// <summary>
    /// where the attack collider will be spawned
    /// </summary>
    protected Vector2 attackSpawnPoint;
    /// <summary>
    /// field for the attack prefab to be spawned when attacking
    /// </summary>
    public GameObject attack;
    /// <summary>
    /// field for the attack prefab to be spawned when attacking
    /// </summary>
    public GameObject projectile;
    /// <summary>
    /// The collider orientation for the melee attack.
    /// </summary>
    protected CapsuleDirection2D attackCapsuleDirection;
    /// <summary>
    /// how much to rotate the attack capsule
    /// Needed for diagonal attack
    /// </summary>
    protected float attackCapsuleRotation;
    public float attackDuration = .5f;
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
        ResetTimer = -1f;
        ResetTimerReset = 6f;
        ResetTimerVariance = 2f;

        ResetMovementBias();
    }

    private void Update()
    {
        if (ResetTimer < 0)
        {
            ResetMovementBias();
            ResetTimer = ResetTimerReset + Random.Range(-ResetTimerVariance, ResetTimerVariance);
        }
        else
        {
            ResetTimer -= Time.deltaTime;
        }
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

    public bool MoveTo(Vector2 target, float speed, float threshold)
    {
        //Debug.Log("I am Chase at " + speed + "mph");
        // Turn Greenish
        AiData.currentBehavior = AIData.Behave.Chase;

        //left = false;

        var current = new Vector2(transform.position.x, transform.position.y);

        // at target
        if (Vector2.Distance(current, target) <= threshold)
        {
            return true;
        }
        else
        {
            // move towards target
            AnimatorBody.Play("Move");
            // random rotation of target around current
            //     based on bias of movement
            target = RotatePoint(current, biasMovementAngle, target);
            // get direction towards new target
            target = (target - current);
            target = Vector2.ClampMagnitude(target, speed * Time.deltaTime);
            //left = (target.x > 0) ? true : false;
            transform.Translate(target);

            return false;
        }
    }

    public bool MoveFrom(Vector2 target, float speed, float threshold)
    {
        // Turn Greenish
        AiData.currentBehavior = AIData.Behave.Flee;     
        var current = new Vector2(transform.position.x, transform.position.y);
        if(Vector2.Distance(current, target) <= threshold - 1)
        {
            // move towards target
            AnimatorBody.Play("Move");
            // random rotation of target around current
            //     based on bias of movement
            //target = RotatePoint(current, biasMovementAngle, target);
            target = (target - current);
            target = Vector2.ClampMagnitude(target, speed * Time.deltaTime);
            //left = (target.x < 0) ? true : false;
            transform.Translate(-1*target);
            return false;
        }
        else
        {
            return true;
        }
    }

    // FEED
    public bool Feed(GameObject drop)
    {
        AnimatorBody.Play("Feed");
        AiData.currentBehavior = AIData.Behave.Feed;
        drop.SetActive(false);
        //AiData.Stomach.Enqueue(Instantiate(drop));
        Destroy(drop);

        return true;
    }

    // ATTACK
    public bool MeleeAttack(Vector2 target, float speed)
    {
        if (!isAttacking)
        {
            isAttacking = true;
            AiData.currentBehavior = AIData.Behave.Attack;
            AnimatorBody.Play("Melee");
            // var dir = CalculateDirection(target);
            GameObject newAttack = Instantiate(attack, transform.position, Quaternion.identity, transform);
            CapsuleCollider2D newAttackCollider = newAttack.GetComponent<CapsuleCollider2D>();
            newAttackCollider.size = new Vector2(AiData.MeleeAttackThreshold, AiData.MeleeAttackThreshold);            
            //Debug.Log("ATTACKING");
            //stops attacking for 1 second after attack
            StartCoroutine(EndAttackAfterSeconds(attackDuration, newAttack, true));
            return true;
        }
        return false;        
    }

    public bool RangedAttack(Vector2 target, float speed)
    {
        //Debug.Log("I am rangedAttack");
        if (!isAttacking)
        {
            AiData.currentBehavior = AIData.Behave.Attack;
            // var dir = CalculateDirection(target);
            // AnimatorBody.Play("Ranged");
            Vector2 normalizedVec = GetTargetVector(target);
            GameObject newAttack = Instantiate(projectile, transform.position + new Vector3(0,.25f,0), Quaternion.identity, transform);
            Physics2D.IgnoreCollision(newAttack.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            BaseProjectile projectileData = newAttack.GetComponent<BaseProjectile>();
            projectileData.directionVector = normalizedVec;
            isAttacking = true;
            //Debug.Log("isAttacking is: " + isAttacking);
            StartCoroutine(EndAttackAfterSeconds(attackDuration, newAttack, true));
            
        }

        return true;
    }

    public bool Socialize()
    {
        AnimatorBody.Play("Socialize");
        AiData.currentBehavior = AIData.Behave.Socialize;
        if (ActionTimer < 0)
        {
            // create signal 
            // change signal radius
            // change signal values (++friendliness)
            GameObject obtainSurroundings = Instantiate(Checks.signalPrefab, this.transform, false) as GameObject;
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

    static Direction CalculateDirection(Vector2 target)
    {
        var movementAngle = Vector2.SignedAngle(Vector2.right, target);
        if (movementAngle < 0)
            movementAngle += 360;
        return Direction.East.Offset(Mathf.RoundToInt(movementAngle / 90));
    }

    protected IEnumerator EndAttackAfterSeconds(float time, GameObject newAttack, bool destroy)
    {
        yield return new WaitForSeconds(time);
        isAttacking = false;
        if (destroy) Destroy(newAttack);
        yield return null;
    }

    // reset action timer
    public void ResetActionTimer()
    {
        ResetMovementBias();
        ActionTimer = ActionTimerReset + Random.Range(-ActionTimerVariance, ActionTimerVariance);
    }

    // resest movement bias
    public void ResetMovementBias()
    {
        biasAngleMod = Random.Range(-2f, 2f);
        float bR = Mathf.Pow(2f, biasAngleMod);
        float bL = Mathf.Pow(2f, -biasAngleMod);
        biasMovementAngle = Random.Range(-biasAngle * bL, biasAngle * bR);
    }
    protected Vector2 GetTargetVector(Vector2 targetVector)
    {
        return new Vector2(targetVector.x - transform.position.x, targetVector.y - transform.position.y).normalized;
    }
    #region POINTS AND ANGLES
    /// <summary>
    /// Rotate Point: given a pivot and an angle,
    ///     return the original point having been
    ///     rotated that many degrees
    /// </summary>
    /// <param name="pivotPoint"></param>
    /// <param name="angle"></param>
    /// <param name="changePoint"></param>
    /// <returns></returns>
    public Vector2 RotatePoint(Vector2 pivotPoint, float angle, Vector2 changePoint)
    {
        // sin and cos
        float sin = Mathf.Sin(Mathf.Deg2Rad*angle);
        float cos = Mathf.Cos(Mathf.Deg2Rad*angle);

        // translate point back to origin
        changePoint.x -= pivotPoint.x;
        changePoint.y -= pivotPoint.y;

        // rotate point
        float xnew = changePoint.x * cos - changePoint.y * sin;
        float ynew = changePoint.x * sin + changePoint.y * cos;

        // return new vector
        // after readjusting from pivot
        return new Vector2(xnew + pivotPoint.x, ynew + pivotPoint.y) ;
    }
    #endregion
}
