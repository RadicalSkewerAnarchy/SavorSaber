using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData : CharacterData
{
    #region Moods
    public float fear;
    public float hunger;
    public float hostility;
    public float friendliness;
    #endregion
    /// <summary> A delegate that returns a float between 0 and 1 </summary>
    public delegate float GetNormalValue();
    /// <summary> A dictionary of normalized AI values to be used by Utility curves</summary>
    private Dictionary<string, GetNormalValue> _values;

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
