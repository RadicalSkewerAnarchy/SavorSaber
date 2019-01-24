using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ONLY REQUIRE FOR DEBUGGING OR HARDCODED ABSTRACT BehaviorS
/// </summary>
//[RequireComponent(typeof(MonsterBehavior))]
[RequireComponent(typeof(MonsterBehavior))]

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
        Custom1,
        Custom2,
        Custom3,
    }

    #endregion

    public State currentState = State.Idle;
    public CustomProtocol[] customProtocols = new CustomProtocol[3];
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
        if (Input.GetKeyDown(KeyCode.U))
        {
            switch (currentState)
            {
                // idle
                case State.Idle:
                    Behavior.Idle();
                    break;
                // chase
                case State.Chase:
                    Behavior.MoveTo(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                    break;
                // attack
                case State.Attack:
                    Behavior.Attack(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                    break;
                // flee
                case State.Flee:
                    Behavior.MoveFrom(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                    break;
                case State.Custom1:
                    customProtocols[0].Invoke();
                    break;
                case State.Custom2:
                    customProtocols[1].Invoke();
                    break;
                case State.Custom3:
                    customProtocols[2].Invoke();
                    break;
                // default         
                default:
                    Debug.Log("YOU SHOULD NEVER BE HERE!");
                    break;
            }
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
