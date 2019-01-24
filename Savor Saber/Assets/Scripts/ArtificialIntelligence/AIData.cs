using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ONLY REQUIRE FOR DEBUGGING OR HARDCODED ABSTRACT BehaviorS
/// </summary>
//[RequireComponent(typeof(MonsterBehavior))]
[RequireComponent(typeof(MonsterBehavior))]
[RequireComponent(typeof(MonsterProtocols))]

public class AIData : CharacterData
{
    #region Moods
    [Range(0f,1f)]
    public float fear;
    [Range(0f, 1f)]
    public float hunger;
    [Range(0f, 1f)]
    public float hostility;
    [Range(0f, 1f)]
    public float friendliness;
    #endregion
    /// <summary> A delegate that returns a float between 0 and 1 </summary>
    public delegate float GetNormalValue();
    /// <summary> A dictionary of normalized AI values to be used by Utility curves</summary>
    private Dictionary<string, GetNormalValue> _values;

    #region States
    /// <summary> my current state </summary>
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Flee,
        Socialize,
        Feed
    }
    #endregion
    public State currentState = State.Idle;
    #region Protocols
    /// <summary> my current state </summary>
    public enum Protocols
    {
        Melee,
        Ranged,
        Lazy,
        Guard,
        Party,
        Swarm,
        Feast,
        Console
    }
    #endregion
    public Protocols currentProtocol = Protocols.Lazy;

    // Decision making
    float DecisionTimer;
    float DecisionTimerReset;
    float DecisionTimerVariance;

    /// <summary> lists that may be needed for certain target positions or objects </summary>
    List<GameObject> targetObjects = new List<GameObject>();
    Vector2 targetPosition;
    /// <summary>
    /// Monster Behavior, Monster Protocol
    /// </summary>
    private MonsterBehavior Behavior;
    private MonsterProtocols Protocol;
    /// <summary>
    /// Variables to be used for calling MonsterBehaviors
    /// </summary>
    float Speed = 1;
    Vector2 Target;

    private void Start()
    {
        Behavior = GetComponent<MonsterBehavior>();
        Protocol = GetComponent<MonsterProtocols>();
        //Behavior.UpdateSpeed(speed);

        _values = new Dictionary<string, GetNormalValue>()
        {
            {"Fear", () => {return fear; } },
            {"Hunger", () => {return hunger; } },
            {"Hostility", () => {return hostility; } },
            {"Friendliness", () => {return friendliness; } },
            {"PlayerDistance", () => { return 1; } }, // DEBUG
            {"FireDistance", () => {return 1; } }, //DEBUG
            {"Health", () => {return Normal(health, maxHealth); } }
        };

        // Decision making
        DecisionTimer = -1f;
        DecisionTimerReset = 10f;
        DecisionTimerVariance = 5f;
    }

    private void Update()
    {
        // check current state
        // acquire necessary data
        // act on current state
        /*** IMPORTANT: Currently states call behaviors directly for ease of debugging and setting them up,
        *               this should be changed so they only call protocols. EXCEPTION: Hard coded abstract
        *               protocol
        ***/
        // UPDATE Decision
        if (DecisionTimer < 0)
        {
            ///
            /// EVALUATE THE CURVES
            /// MAKE NEW DECISION
            ///
            DecisionTimer = DecisionTimerReset + Random.Range(-DecisionTimerVariance, DecisionTimerVariance);
        }

        // SWITCH protocol
        switch (currentProtocol)
        {
            // melee
            case Protocols.Melee:
                //Behavior.MoveTo(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                Protocol.Melee();
                break;
            // ranged
            case Protocols.Ranged:
                //Behavior.MoveTo(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                Protocol.Ranged();
                break;
            // lazy
            case Protocols.Lazy:
                Protocol.Lazy();
                break;
            // guard
            case Protocols.Guard:
                //Behavior.MoveFrom(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                Protocol.Guard();
                break;
            // party
            case Protocols.Party:
                //Behavior.MoveFrom(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                Protocol.Party();
                break;
            // swarm
            case Protocols.Swarm:
                //Behavior.MoveFrom(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                Protocol.Swarm();
                break;
            // feast
            case Protocols.Feast:
                //Behavior.MoveFrom(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                Protocol.Feast();
                break;
            // console
            case Protocols.Console:
                //Behavior.MoveFrom(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                Protocol.Console();
                break;
            // default
            default:
                Debug.Log("YOU SHOULD NEVER BE HERE!");
                break;
        }
    }

    public float Normal(int now, int max)
    {
        return now / (float)max;
    }
    /// <summary> Get a normalized value from the value dictionary. if the value is not present, returns -1 </summary>
    public float getNormalizedValue(string value)
    {
        if(!_values.ContainsKey(value))
        {
            Debug.LogError(value + " is not a valid AI value, returning -1");
            return -1f;
        }
        return _values[value]();
    }
}
