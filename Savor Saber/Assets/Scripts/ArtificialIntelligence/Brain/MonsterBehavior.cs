using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AIData))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MonsterController))]
[RequireComponent(typeof(FlavorInputManager))]
//[RequireComponent(typeof(Pathfinder))]

public class MonsterBehavior : MonoBehaviour
{
    #region GlobalVariables
    #region Components
    Rigidbody2D RigidBody;
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
    public float ActionTimer;
    public float ActionTimerReset;
    public float ActionTimerVariance;
    public float ResetTimer;
    public float ResetTimerReset;
    public float ResetTimerVariance;
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
    public bool isAttacking = false;
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
    /// <summary>
    /// Resets timer if it reaches 0, decrements if it's above 0
    /// </summary>
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
    
    /// <summary>
    /// Idle does nothing until next decision is made
    /// </summary>
    public bool Idle()
    {
        AnimatorBody.Play("Idle");
        AiData.currentBehavior = AIData.Behave.Idle;
        if (ActionTimer < 0)
        {
            AiData.InstantiateSignal(1f, "Fear", -0.1f, false, true);
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
    public bool MoveTo(Vector2 target, float speed, float threshold)
    {
        AiData.currentBehavior = AIData.Behave.Chase;
        var current = new Vector2(transform.position.x, transform.position.y+.25f);
        if (Vector2.Distance(current, target) <= threshold)
        {
            return true;
        }
        else
        {
            #region Move
            AnimatorBody.Play("Move");           
            target = RotatePoint(current, biasMovementAngle, target);
            target = (target - current);
            target = Vector2.ClampMagnitude(target, speed * Time.deltaTime);
            controller.Direction = DirectionMethods.FromVec2(target);
            RigidBody.velocity = target * speed;        
            #endregion
            return false;
        }
    }
    /// <summary>
    /// Moves the agent away from threshold distance - 1 from target at speed
    /// </summary>
    public bool MoveFrom(Vector2 target, float speed, float threshold)
    {
        AiData.currentBehavior = AIData.Behave.Flee;     
        var current = new Vector2(transform.position.x, transform.position.y);
        if(Vector2.Distance(current, target) <= threshold - 1)
        {
            #region Move
            AnimatorBody.Play("Move");
            target = (target - current);
            target = Vector2.ClampMagnitude(target, speed * Time.deltaTime);
            controller.Direction = DirectionMethods.FromVec2(-1 * target);
            transform.Translate(-1*target);
            #endregion
            return false;
        }
        else
        {
            return true;
        }
    }
    /// <summary>
    /// Deactivates detected drop and destroys it
    /// </summary>  
    public bool Feed(GameObject drop)
    {
        //Debug.Log("Feed Reached");
        if(drop != null)
        {
            #region Eat
            AnimatorBody.Play("Feed");
            AiData.currentBehavior = AIData.Behave.Feed;
            IngredientData ingredient = drop.GetComponent<SkewerableObject>().data;
            AiData.Stomach.Enqueue(ingredient);
            IngredientData[] ingredientArray = new IngredientData[1];
            ingredientArray[0] = ingredient;

            // activate flavor input manager
            flavor.Feed(ingredientArray);

            // deactivate drop
            drop.SetActive(false);
            Destroy(drop);

            #endregion
            AiData.InstantiateSignal(0.1f, "Hunger", -0.25f, false, true);
            if(AiData.eatingParticleBurst != null)
            {
                AiData.eatingParticleBurst.Play();
            }
            if(AiData.eatSFX != null)
            {
                Instantiate(AiData.sfxPlayer, transform.position, transform.rotation).GetComponent<PlayAndDestroy>().Play(AiData.eatSFX);
                //Debug.Log("===========================Hunger sound effect playing here");
            }
            return true;
        }
        else
        {
            return false;
        }

    }
    /// <summary>
    /// If you're not attacking, make attack collider and attack
    /// </summary>
    public bool MeleeAttack(Vector2 target, float speed)
    {
        if (!isAttacking)
        {
            #region Attack
            isAttacking = true;
            AiData.currentBehavior = AIData.Behave.Attack;
            AnimatorBody.Play("Melee");
            GameObject newAttack = Instantiate(attack, transform.position, Quaternion.identity, transform);
            CapsuleCollider2D newAttackCollider = newAttack.GetComponent<CapsuleCollider2D>();
            newAttackCollider.size = new Vector2(AiData.MeleeAttackThreshold, AiData.MeleeAttackThreshold);            
            newAttackCollider.transform.position += new Vector3(0, this.GetComponent<CapsuleCollider2D>().offset.y, 0);
            StartCoroutine(EndAttackAfterSeconds(attackDuration, newAttack, true));
            #endregion
            return true;
        }
        return false;        
    }
    /// <summary>
    /// If you're not attacking, same boolean as MeleeAttack();
    /// </summary>
    public bool RangedAttack(Vector2 target, float speed)
    {
        if (!isAttacking)
        {
            #region Attack
            isAttacking = true;
            AiData.currentBehavior = AIData.Behave.Attack;
            AnimatorBody.Play("Ranged");
            Vector2 normalizedVec = GetTargetVector(target);
            GameObject newAttack = Instantiate(projectile, transform.position + new Vector3(0,.25f,0), Quaternion.identity, transform);
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
    /// Spawns one friend signal per action
    /// </summary>
    public bool Socialize()
    {
        AnimatorBody.Play("Socialize");
        AiData.currentBehavior = AIData.Behave.Socialize;
        if (ActionTimer < 0)
        {
            // create signal 
            // change signal radius
            // change signal values (++friendliness)
            Debug.Log("Instantiating Happiness Signal");
            AiData.InstantiateSignal((AiData.Perception / 2), "Friendliness", 0.25f, true, false);
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
    public bool Console()
    {
        AnimatorBody.Play("Socialize");
        AiData.currentBehavior = AIData.Behave.Console;
        if (ActionTimer < 0)
        {
            // create signal 
            // change signal radius
            // change signal values (--fear)
            //Debug.Log("Instantiating Calming Signal");
            AiData.InstantiateSignal((AiData.Perception), "Fear", -0.2f, true, true);
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
    /// Returns a normalized direction
    /// </summary>
    static Direction CalculateDirection(Vector2 target)
    {
        var movementAngle = Vector2.SignedAngle(Vector2.right, target);
        if (movementAngle < 0)
            movementAngle += 360;
        return Direction.East.Offset(Mathf.RoundToInt(movementAngle / 90));
    }
    /// <summary>
    /// Ends attack after time passes and destroys
    /// </summary>
    protected IEnumerator EndAttackAfterSeconds(float time, GameObject newAttack, bool destroy)
    {
        yield return new WaitForSeconds(time);
        isAttacking = false;
        if (destroy) Destroy(newAttack);
        yield return null;
    }
    /// <summary>
    /// Reset action timer
    /// </summary>
    public void ResetActionTimer()
    {
        ResetMovementBias();
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
