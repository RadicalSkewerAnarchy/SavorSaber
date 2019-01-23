using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterMovement))]
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
        idle,
        chase,
        attack,
        flee
    }
    public Dictionary<string, int> _translation;
    #endregion
    public State currentState = State.idle;
    /// <summary> lists that may be needed for certain target positions or objects </summary>
    List<GameObject> targetObjects = new List<GameObject>();
    Vector2 targetPosition;
    /// <summary>
    /// Monster Movement
    /// </summary>
    private MonsterMovement moveMe;
    

    private void Start()
    {
        moveMe = GetComponent<MonsterMovement>();
        moveMe.UpdateSpeed(speed);

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
                case State.idle:
                    Debug.Log("I am Idle");
                    GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
                    break;
                // chase
                case State.chase:
                    Debug.Log("I am Chase");
                    // Turn Green
                    GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
                    break;
                // attack
                case State.attack:
                    Debug.Log("I am Attack");
                    // Turn Red
                    GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
                    break;
                // flee
                case State.flee:
                    Debug.Log("I am Flee");
                    //  Turn Blue
                    GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);
                    if(targetPosition == null)
                    {
                        targetPosition = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    }
                    else
                    {
                        moveMe.UpdateDirection(targetPosition);
                    }
                    
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
