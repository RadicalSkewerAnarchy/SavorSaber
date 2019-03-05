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
    [SerializeField] protected float initialFear;
    [Range(0f, 1f)]
    [SerializeField] protected float initialHunger;
    [Range(0f, 1f)]
    [SerializeField] protected float initialHostility;
    [Range(0f, 1f)]
    [SerializeField] protected float initialFriendliness;
    public Dictionary<string, float> moods = new Dictionary<string, float>();
    #endregion
    #region Death and Damage effects
    public AudioClip damageSFX;
    public AudioClip deathSFX;
    public GameObject sfxPlayer;
    public ParticleSystem damageParticleBurst = null;
    #endregion

    void Start()
    {
        InitializeCharacterData();
    }

    protected void InitializeCharacterData()
    {
        // set mood values into dictionary
        #region Mood Value Setup
        if (!moods.ContainsKey("Fear"))
            moods.Add("Fear", initialFear);
        if (!moods.ContainsKey("Hunger"))
            moods.Add("Hunger", initialHunger);
        if (!moods.ContainsKey("Hostility"))
            moods.Add("Hostility", initialHostility);
        if (!moods.ContainsKey("Friendliness"))
            moods.Add("Friendliness", initialFriendliness);
        #endregion
        Spawn = transform.position;
    }
    /// <summary> A standard damage function. </summary>
    public virtual void DoDamage(int damage)
    {
        health -= damage;
        //only play damage SFX if it was not a killing blow so sounds don't overlap
        if (health > 0)
        {
            if(damageSFX != null)
            {
                var deathSoundObj = Instantiate(sfxPlayer, transform.position, transform.rotation);
                deathSoundObj.GetComponent<PlayAndDestroy>().Play(damageSFX);
            }
            else
            {
                //play generic sound from asset bundle
            }
            if (damageParticleBurst != null)
                damageParticleBurst.Play();
            StartCoroutine(DamageEffectCr());
        }
        else // Health <= 0
        {
            Kill();
        }
    }

    protected IEnumerator DamageEffectCr()
    {
        var spr = GetComponent<SpriteRenderer>();
        if(spr != null)
        {
            spr.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.75f);
            yield return new WaitForSeconds(0.1f);
            spr.color = Color.white;
        }
    }
    /// <summary> The default death function </summary>
    public void Kill()
    {
        var deathSoundObj = Instantiate(sfxPlayer, transform.position, transform.rotation);
        deathSoundObj.GetComponent<PlayAndDestroy>().Play(deathSFX);
        GetComponent<DropOnDeath>().Drop();
        Destroy(gameObject);
    }
}
