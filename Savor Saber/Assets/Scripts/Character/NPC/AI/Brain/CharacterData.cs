﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public float MeleeAttackThreshold = 1f;
    public float RangeAttackThreshold = 2f;
    public float EngageHostileThreshold = 5f;
    public int maxHealth = 10;
    public int health = 10;
    public int PartySize = 3;
    private Vector2 Spawn;
    public GameObject signalPrefab;

    public float damageDealt = 0;
    public float entitiesKilled = 0;
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
    #region Effects
    public AudioClip damageSFX;
    public AudioClip healSFX;
    public AudioClip deathSFX;
    public AudioClip eatSFX;
    public GameObject sfxPlayer;
    public ParticleSystem damageParticleBurst = null;
    public ParticleSystem eatingParticleBurst = null;
    public Slider healthBar;
    protected Coroutine barCr = null;
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
    public virtual bool DoDamage(int damage)
    {
        bool dead = false;
        if (damage > 0)
        {
            health -= damage;
            //only play damage SFX if it was not a killing blow so sounds don't overlap
            if (health > 0)
            {
                if (healthBar != null)
                {
                    healthBar.gameObject.SetActive(true);
                    //Debug.Log("Update health bar");
                    healthBar.value = (float)health / maxHealth;
                    if (barCr != null)
                        StopCoroutine(barCr);
                    barCr = StartCoroutine(ShowHealthBar());
                }
                if (damageSFX != null)
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
                dead = true;
                Kill();
            }

            // create a fear signal
            float hp = (maxHealth - health) / maxHealth;
            InstantiateSignal(4 , "Fear",  0.5f, true, true);
        }
        if (damage < 0)
        {
            health = Mathf.Max(health - damage, maxHealth);
            //only play damage SFX if it was not a killing blow so sounds don't overlap
            if (health > 0)
            {
                if (healthBar != null)
                {
                    healthBar.gameObject.SetActive(true);
                    Debug.Log("Update health bar");
                    healthBar.value = (float)health / maxHealth;
                    if (barCr != null)
                        StopCoroutine(barCr);
                    barCr = StartCoroutine(ShowHealthBar());
                }
                if (healSFX != null)
                {
                    var deathSoundObj = Instantiate(sfxPlayer, transform.position, transform.rotation);
                    deathSoundObj.GetComponent<PlayAndDestroy>().Play(healSFX);
                }
            }

            // create a anti fear signal
            float hp = (health) / maxHealth;
            InstantiateSignal(2, "Fear", -0.2f, true, true);
        }
        return dead;
    }
    /// <summary> Show the health bar for a short amount of time </summary>
    protected IEnumerator ShowHealthBar()
    {
        yield return new WaitForSeconds(3);
        healthBar.gameObject.SetActive(false);
        barCr = null;
    }
    /// <summary> Play a short color flash when the character is damaged </summary>
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
        //if (gameObject.name == "GhostReaper")
        //    FlagManager.SetFlag("reaperdead", "true");
        Destroy(gameObject);
    }

    // InstantiateSignal()
    // create a signal that subtracts
    public GameObject InstantiateSignal(float size, string mod, float modifier, bool hitall, bool hitself)
    {
        GameObject obtainSurroundings = Instantiate(signalPrefab, transform.position, Quaternion.identity, transform) as GameObject;
        SignalApplication signalModifier = obtainSurroundings.GetComponent<SignalApplication>();
        signalModifier.SetSignalParameters(this.gameObject, size, new Dictionary<string, float>() { { mod, modifier } }, hitall, hitself);
        signalModifier.Activate();
        return obtainSurroundings;
    }
}
