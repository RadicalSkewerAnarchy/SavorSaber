using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public enum _states : int
    {
        idle = 0,
        chase = 1,
        attack = 2,
        flee = 3
    }
    public Dictionary<string, int> _translation;
    #endregion
    public int currentState = (int)_states.idle;
    /// <summary> lists that may be needed for certain target positions or objects </summary>
    List<GameObject> targets = new List<GameObject>();
    

    private void Start()
    {
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

        _translation = new Dictionary<string, int>();
        _translation.Add("Idle",   (int)_states.idle);
        _translation.Add("Chase",  (int)_states.chase);
        _translation.Add("Attack", (int)_states.attack);
        _translation.Add("Flee",   (int)_states.flee);
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
                case (int)_states.idle:
                    Debug.Log("I am Idle");
                    break;
                // chase
                case (int)_states.chase:
                    Debug.Log("I am Chase");
                    break;
                // attack
                case (int)_states.attack:
                    Debug.Log("I am Attack");
                    break;
                // flee
                case (int)_states.flee:
                    Debug.Log("I am Flee");
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
