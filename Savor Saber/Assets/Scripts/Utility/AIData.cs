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
    public delegate float GetNormalValue();
    private Dictionary<string, GetNormalValue> _values;

    private void Start()
    {
        _values = new Dictionary<string, GetNormalValue>()
        {
            {"fear", () => {return fear; } },
            {"hunger", () => {return hunger; } },
            {"hostility", () => {return hostility; } },
            {"friendliness", () => {return friendliness; } },
            //{"playerDistance", GetPlayerDist },
            {"health", () => {return Normal(health, maxHealth); } }
        };
    }

    public float Normal(int now, int max)
    {
        return now / (float)max;
    }

    public float getNormalizedValue(string value)
    {
        return _values[value]();
    }
}
