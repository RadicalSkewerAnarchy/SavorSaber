using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AIData))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MonsterController))]
[RequireComponent(typeof(FlavorInputManager))]
//[RequireComponent(typeof(PlaySFX))]
//[RequireComponent(typeof(Pathfinder))]

public class MonsterBehavior : MonoBehaviour
{
    #region GlobalVariables
    #region Components
    Rigidbody2D RigidBody;
    PlaySFX sfxPlayer;
    Animator AnimatorBody;
    AIData AiData;
    MonsterChecks Checks;
    MonsterController controller;
    FlavorInputManager flavor;
    public Pathfinder pathfinder;
    #endregion
    #region ActionTimer
    /// <summary>
    /// behavior timers
    /// </summary>
    [HideInInspector]
    public float ActionTimer;
    public float ActionTimerReset;
    public float ActionTimerVariance;
    [HideInInspector]
    public float ResetTimer;
    [HideInInspector]
    public float ResetTimerReset;
    [HideInInspector]
    public float ResetTimerVariance;
    bool actionAvailable = true;
    bool left = false;
    #endregion
    #region Bias
    /// <summary>
    /// biase
    /// </summary>
    private float biasAngle = 0f;
    private float biasAngleMod;
    private float biasMovementAngle;
    #endregion
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
    public float attackCooldown = .5f;
    public float meleeAttackDuration = 0.5f;
    public float meleeAttackDelay = 0.25f;
    public Vector2 meleeAttackDimensions = new Vector2(2,2);
    [HideInInspector]
    public bool isAttacking = false;
    public AudioClip meleeSFX;
    #endregion
    #endregion
    private void Start()
    {
        #region Initialize
        #region GetComponents
        AiData = GetComponent<AIData>();
        Checks = AiData.GetComponent<MonsterChecks>();
        AnimatorBody = GetComponent<Animator>();
        RigidBody = GetComponent<Rigidbody2D>();
        controller = GetComponent<MonsterController>();
        pathfinder = GetComponent<Pathfinder>();
        flavor = GetComponent<FlavorInputManager>();
        sfxPlayer = GetComponent<PlaySFX>();
        #endregion
        ActionTimer = -1f;
        ActionTimerReset = 5f;
        ActionTimerVariance = 2f;
        ResetTimer = -1f;
        ResetTimerReset = 6f;
        ResetTimerVariance = 2f;
        ResetMovementBias();
        #endregion
    }

    #region Movement
    /// <summary>
    /// Idle does nothing until next decision is made
    /// </summary>
    public bool Idle()
    {
        if (ActionTimer < 0)
        {
            TransitionBehavior(AIData.Behave.Idle, "Idle");
            /*if (actionAvailable)
            {
                AiData.InstantiateSignal(1f, "Fear", -0.2f, false, true);
                actionAvailable = false;
            }*/
            return true;
        }
        else
        {
            ActionTimer -= Time.deltaTime;
            return false;
        }
    }
    /// <summary>
    /// Moves the agent to threshhold distance from target at speed
    /// </summary>
    public bool MoveTo(Vector2 target, float speed, float threshold, bool attacking = false)
    {
        Vector2 current = transform.position;
        if (Vector2.Distance(current, target) <= threshold || current == target)
        {
            if (!attacking)
                TransitionBehavior(AIData.Behave.Idle, "Idle");
            else
                TransitionBehavior(AIData.Behave.Attack, "Melee");
            
            return true;
        }
        else
        {
            #region Move
            target = (target - current);
            target = Vector2.ClampMagnitude(target, speed);
            controller.Direction = DirectionMethods.FromVec2(target);
            RigidBody.velocity = Vector2.ClampMagnitude(RigidBody.velocity + target, speed);
            TransitionBehavior(AIData.Behave.Chase, "Move");
            #endregion
            return false;
        }
    }
    /// <summary>
    /// Moves the agent away from threshold distance - 1 from target at speed
    /// </summary>
    public bool MoveFrom(Vector2 target, float speed, float threshold)
    {
        Vector2 current = transform.position;
        if (target == current)
            return true;

        if (Vector2.Distance(current, target) < threshold)
        {
            #region Move
            target = (current - target);
            target = Vector2.ClampMagnitude(target, speed);
            controller.Direction = DirectionMethods.FromVec2(target);
            if(!AiData.updateBehavior) return false;
            RigidBody.velocity = target;
            TransitionBehavior(AIData.Behave.Flee, "Move");
            #endregion
            return false;
        }
        else
        {
            TransitionBehavior(AIData.Behave.Idle, "Idle");
            return true;
        }
    }
    #endregion

    #region Eating
    /// <summary>
    /// Deactivates detected drop and destroys it
    /// </summary>  
    public bool Feed(GameObject drop, bool fedByPlayer=false)
    {
        //Debug.Log("Feed Reached");
        if(drop != null)
        {
            #region Eat
            IngredientData ingredient = drop.GetComponent<SkewerableObject>().data;
            AiData.Stomach.Enqueue(ingredient);
            IngredientData[] ingredientArray = new IngredientData[1];
            ingredientArray[0] = ingredient;

            // activate flavor input manager
            flavor.Feed(ingredientArray, fedByPlayer);

            // deactivate drop
            drop.SetActive(false);
            Destroy(drop);

            #endregion
            AiData.InstantiateSignal(0.1f, "Hunger", -0.1f, false, true);
            if(AiData.eatingParticleBurst != null)
            {
                AiData.eatingParticleBurst.Play();
            }
            if(AiData.eatSFX != null)
            {
                Instantiate(AiData.sfxPlayer, transform.position, transform.rotation).GetComponent<PlayAndDestroy>().Play(AiData.eatSFX);
            }

            TransitionBehavior(AIData.Behave.Feed, "Feed");
            return true;
        }
        else
        {
            return false;
        }

    }
    #endregion

    #region Battle
    /// <summary>
    /// If you're not attacking, make attack collider and attack
    /// </summary>
    public bool MeleeAttack(Vector2 target)
    {
        if (!isAttacking)
        {
            #region Attack
            TransitionBehavior(AIData.Behave.Attack, "Melee");
            AnimatorBody.Play("Melee");
            isAttacking = true;
            if (meleeSFX != null)
                Instantiate(AiData.sfxPlayer, transform.position, transform.rotation).GetComponent<PlayAndDestroy>().Play(meleeSFX);
            StartCoroutine(MeleeDelay(target));
            #endregion
            return true;
        }
        TransitionBehavior(AIData.Behave.Idle, "Idle");
        return false;        
    }
    private IEnumerator MeleeDelay(Vector2 target)
    {
        yield return new WaitForSeconds(meleeAttackDelay);

        #region Melee
        GameObject newAttack = Instantiate(attack, transform.position, Quaternion.identity, transform);
        CapsuleCollider2D newAttackCollider = newAttack.GetComponent<CapsuleCollider2D>();
        newAttack.GetComponent<BaseMeleeAttack>().myAttacker = this.gameObject;
        newAttackCollider.size = meleeAttackDimensions;
        newAttackCollider.transform.position += new Vector3(this.GetComponent<Collider2D>().offset.x, this.GetComponent<Collider2D>().offset.y, 0);
        newAttack.transform.Rotate(target - (Vector2)this.transform.position);
        StartCoroutine(EndAttackAfterSeconds(meleeAttackDuration, newAttack, true));
        #endregion
    }

    /// <summary>
    /// If you're not attacking, same boolean as MeleeAttack();
    /// </summary>
    public bool RangedAttack(Vector2 target, float speed)
    {
        //Debug.Log("Entering RangedAttack Behavior");
        if (!isAttacking)
        {
            //Debug.Log("Not currently executing attack");
            #region Attack
            isAttacking = true;
            TransitionBehavior(AIData.Behave.Attack, "Ranged");
            Vector2 normalizedVec = GetTargetVector(target);
            GameObject newAttack = Instantiate(projectile, transform.position + new Vector3(0,.25f,0), Quaternion.identity);
            Physics2D.IgnoreCollision(newAttack.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            BaseProjectile projectileData = newAttack.GetComponent<BaseProjectile>();
            projectileData.directionVector = normalizedVec;            
            projectileData.attacker = this.gameObject;            
            StartCoroutine(EndAttackAfterSeconds(attackDuration, newAttack, false));
            #endregion
        }
       
        return true;
    }

    /// <summary>
    /// Ends attack after time passes and destroys
    /// </summary>
    protected IEnumerator EndAttackAfterSeconds(float time, GameObject newAttack, bool destroy)
    {
        yield return new WaitForSeconds(time);
        if (destroy)
            Destroy(newAttack);
        StartCoroutine(EnableAttacking(attackCooldown));
        yield return null;
    }
    /// <summary>
    /// reenables attacking
    /// </summary>
    protected IEnumerator EnableAttacking(float time)
    {
        yield return new WaitForSeconds(time);
        isAttacking = false;
        yield return null;
    }

    #endregion

    #region Friends
    /// <summary>
    /// Spawns one friend signal per action
    /// </summary>
    public bool Socialize()
    {
        if (ActionTimer < 0)
        {
            // create signal 
            // change signal radius
            // change signal values (++friendliness)
            //Debug.Log("Instantiating Happiness Signal");
            TransitionBehavior(AIData.Behave.Socialize, "Socialize");
            AiData.InstantiateSignal(2f, "Friendliness", 0.1f, true, true);
            ResetActionTimer();
            return true;
        }
        else
        {
            //ActionTimer -= Time.deltaTime;
            ActionTimer = 0;
            return false;
        }
    } 
    /// <summary>
    /// Spawns one friend signal per action
    /// </summary>
    public bool Console()
    {
        if (ActionTimer < 0)
        {
            // create signal 
            // change signal radius
            // change signal values (--fear)
            //Debug.Log("Instantiating Calming Signal");
            TransitionBehavior(AIData.Behave.Socialize, "Socialize");
            AiData.InstantiateSignal(2f, "Fear", -0.25f, true, true);
            ResetActionTimer();
            return true;
        }
        else
        {
            ActionTimer -= Time.deltaTime;
            return false;
        }
    }
    /// <summary>
    /// Spawns one friend signal per action
    /// </summary>
    public bool Scare()
    {
        if (ActionTimer < 0)
        {
            // create signal 
            // change signal radius
            // change signal values (--fear)
            //Debug.Log("Instantiating Calming Signal");
            TransitionBehavior(AIData.Behave.Socialize, "Socialize");
            AiData.InstantiateSignal(3f, "Fear", 0.25f, true, false);
            ResetActionTimer();
            return true;
        }
        else
        {
            ActionTimer -= Time.deltaTime;
            return false;
        }
    }
    #endregion

    #region Resets

    /// <summary>
    /// Reset action timer
    /// </summary>
    public void ResetActionTimer()
    {
        ResetMovementBias();
        actionAvailable = true;
        ActionTimer = ActionTimerReset + Random.Range(-ActionTimerVariance, ActionTimerVariance);
    }
    /// <summary>
    /// Resets movement bias
    /// </summary>
    public void ResetMovementBias()
    {
        biasAngleMod = Random.Range(-2f, 2f);
        float bR = Mathf.Pow(2f, biasAngleMod);
        float bL = Mathf.Pow(2f, -biasAngleMod);
        biasMovementAngle = Random.Range(-biasAngle * bL, biasAngle * bR);
    }
    #endregion

    #region POINTS AND ANGLES

    /// <summary>
    /// Returns a normalized direction
    /// </summary>
    static Direction CalculateDirection(Vector2 target)
    {
        var movementAngle = Vector2.SignedAngle(Vector2.right, target);
        if (movementAngle < 0)
            movementAngle += 360;
        return Direction.East.Offset(Mathf.RoundToInt(movementAngle / 90));
    }


    protected Vector2 GetTargetVector(Vector2 targetVector)
    {
        return new Vector2(targetVector.x - transform.position.x, targetVector.y - transform.position.y).normalized;
    }

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

    #region Animations and Behaviors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="behave">behavior transitioning TO</param>
    private void TransitionBehavior(AIData.Behave behave, string anim)
    {
        if (AiData.previousBehavior != AiData.currentBehavior)
            BehaviorIn(behave, anim);
        else if (AiData.currentBehavior != behave)
            BehaviorOut(behave, anim);
    }

    private void BehaviorIn(AIData.Behave behave, string anim)
    {
        AiData.previousBehavior = AiData.currentBehavior;
        AnimatorBody.Play(anim);
    }

    private void BehaviorOut(AIData.Behave behave, string anim)
    {
        if (AiData.previousBehavior == AIData.Behave.Attack)
        {
            if (AnimatorBody.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                AiData.currentBehavior = behave;
                AnimatorBody.Play(anim);
            }
        }
        else
        {
            AiData.currentBehavior = behave;
            AnimatorBody.Play(anim);
        }
    }

    #endregion
}
