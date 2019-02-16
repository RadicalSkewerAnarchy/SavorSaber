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
    private Vector2 Spawn;
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

        Spawn = transform.position;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Respawn")
        {
            var rand = Random.Range(.9f, 1.1f);
            Spawn.x = collision.gameObject.transform.position.x * rand;
            rand = Random.Range(.9f, 1.1f);
            Spawn.y = collision.gameObject.transform.position.y * rand;
            Debug.Log("Respawn Set");
        }
    }
    private void Respawn()
    {
        transform.position = Spawn;
        health = maxHealth;
    }
}
