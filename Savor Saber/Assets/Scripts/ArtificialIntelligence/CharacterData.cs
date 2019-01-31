using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MeleeAttack))]
public class CharacterData : MonoBehaviour
{
    #region Moods
    [Range(0f, 1f)]
    public float fear;
    [Range(0f, 1f)]
    public float hunger;
    [Range(0f, 1f)]
    public float hostility;
    [Range(0f, 1f)]
    public float friendliness;

    public Dictionary<string, float> moods = new Dictionary<string, float>();
    #endregion
    public float distanceFrom;

    public Vector2 Position
    {
        get
        {
            return transform.position;
        }
    }

    public int maxHealth;
    public int health;
    public float speed;

    //public MeleeAttack attack;

    void Start()
    {
        // set mood values into dictionary
        moods.Add("Fear", fear);
        moods.Add("Hunger", hunger);
        moods.Add("Hostility", hostility);
        moods.Add("Friendliness", friendliness);
    }
}
