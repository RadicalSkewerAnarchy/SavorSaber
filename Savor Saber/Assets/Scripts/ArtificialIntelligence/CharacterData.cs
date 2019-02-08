using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MeleeAttack))]
public class CharacterData : MonoBehaviour
{   
    public Vector2 Position
    {
        get
        {
            return transform.position;
        }
    }
    #region Values for Behaviors
    public float Speed;
    public float Perception;
    public float MeleeAttackThreshold;
    public float RangeAttackThreshold;
    public int maxHealth;
    public int health;
    public int PartySize = 3;
    #endregion
    #region Variance
    float VDown = 9 / 10;
    float VUp = 11 / 10;
    #endregion
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

    void Start()
    {
        // set mood values into dictionary
        #region Mood Value Setup
        moods.Add("Fear", fear);
        moods.Add("Hunger", hunger);
        moods.Add("Hostility", hostility);
        moods.Add("Friendliness", friendliness);
        #endregion
        // Variable instantiated variance
        #region Variance Implementation
        Speed = Random.Range(Speed*VDown, Speed*VUp);
        Perception = Random.Range(Perception*VDown, Perception*VUp);
        MeleeAttackThreshold = Random.Range(MeleeAttackThreshold * VDown, MeleeAttackThreshold*VUp);
        RangeAttackThreshold = Random.Range(RangeAttackThreshold * VDown, RangeAttackThreshold*VUp);
        maxHealth = (int)Random.Range(maxHealth * VDown, maxHealth * VUp);
        #endregion
    }

    private void Update()
    {
        if(health <= 0)
        {
            if(this.tag == "Player")
            {
                Respawn();
            }            
        }
    }

    private void Respawn()
    {
        transform.position = new Vector3(0, 0, .5f);
        health = maxHealth;
    }
}
