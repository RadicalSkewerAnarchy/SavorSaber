using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    public Dictionary<string, int> _translation;
    #endregion
    public State currentState = State.Idle;
    /// <summary> lists that may be needed for certain target positions or objects </summary>
    List<GameObject> targetObjects = new List<GameObject>();
    Vector2 targetPosition;
    /// <summary>
    /// Monster Behavior
    /// </summary>
    private MonsterBehavior Protocol;
    /// <summary>
    /// Variables to be used for calling MonsterBehaviors
    /// </summary>
    float Speed = 1;
    Vector2 Target;

    private void Start()
    {
        Protocol = GetComponent<MonsterBehavior>();
        //Protocol.UpdateSpeed(speed);

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
        if (Input.GetKeyDown(KeyCode.U))
        {
            switch (currentState)
            {
                // idle
                case State.Idle:
                    Protocol.Idle();
                    break;
                // chase
                case State.Chase:
                    Protocol.MoveTo(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                    break;
                // attack
                case State.Attack:
                    Protocol.Attack(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                    break;
                // flee
                case State.Flee:
                    Protocol.MoveFrom(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
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
