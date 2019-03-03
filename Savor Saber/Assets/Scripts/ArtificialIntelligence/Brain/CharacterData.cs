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
    public AudioClip damageSFX;
    public AudioClip deathSFX;
    public GameObject sfxPlayer;
    private Vector2 Spawn;
    #endregion
    #region Variance
    float VDown = 9 / 10;
    float VUp = 11 / 10;
    #endregion
    #region Moods
    [Range(0f, 1f)]
    [SerializeField] protected float initialFear;
    [Range(0f, 1f)]
    [SerializeField] protected float initialHunger;
    [Range(0f, 1f)]
    [SerializeField] protected float initialHostility;
    [Range(0f, 1f)]
    [SerializeField] protected float initialFriendliness;
    public Dictionary<string, float> moods = new Dictionary<string, float>();
    #endregion

    void Start()
    {
        InitializeCharacterData();
    }

    protected void InitializeCharacterData()
    {
        // set mood values into dictionary
        #region Mood Value Setup
        moods.Add("Fear", initialFear);
        moods.Add("Hunger", initialHunger);
        moods.Add("Hostility", initialHostility);
        moods.Add("Friendliness", initialFriendliness);
        #endregion
        Spawn = transform.position;
    }
    public virtual void DoDamage(int damage)
    {
        health -= damage;
        //only play damage SFX if it was not a killing blow so sounds don't overlap
        if (damageSFX != null && health > 0)
        {
            var deathSoundObj = Instantiate(sfxPlayer, transform.position, transform.rotation);
            deathSoundObj.GetComponent<PlayAndDestroy>().Play(damageSFX);
        }
        else if(damageSFX == null && health > 0)
        {
            //play generic sound from asset bundle
        }
        else if (health <= 0)
        {
            Kill();
        }
    }
    /// <summary> The death function for non-player characters </summary>
    public void Kill()
    {
        var deathSoundObj = Instantiate(sfxPlayer, transform.position, transform.rotation);
        deathSoundObj.GetComponent<PlayAndDestroy>().Play(deathSFX);
        GetComponent<DropOnDeath>().Drop();
        Destroy(gameObject);
    }
}
