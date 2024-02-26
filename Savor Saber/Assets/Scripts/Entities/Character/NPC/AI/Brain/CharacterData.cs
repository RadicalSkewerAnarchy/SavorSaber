using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

    public bool armored = false;

    #region Main Variables

    //a component that stores health threshholds at which to play unity events
    protected HealthGatedEvent events;

    #region DoT fields
    [HideInInspector]
    public bool affectedByDot = false;
    [HideInInspector]
    public GameObject currentDot;
    #endregion

    #region Values for Behaviors

    public float Speed;
    public float Perception;
    public float MeleeAttackThreshold = 1f;
    public float MeleeChaseThreshold = 2f;
    public float RangeAttackThreshold = 2f;
    public float EngageHostileThreshold = 5f;
    public int maxHealth = 10;
    public int health = 10;
    public int overchargeTime = 6;
    private Vector2 Spawn;
    public GameObject signalPrefab;
    [HideInInspector]
    public int PartySize = 3;
    [HideInInspector]
    public float damageDealt = 0;
    [HideInInspector]
    public float entitiesKilled = 0;
    
    #endregion

        #region Variance
        float VDown = 9 / 10;
        float VUp = 11 / 10;
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
    [Header("Event to play on death")]
    public UnityEvent deathEvent;

    #endregion


    #endregion

    void Start()
    {
        InitializeCharacterData();
        events = GetComponent<HealthGatedEvent>();
    }

    protected void InitializeCharacterData()
    {
        // set mood values into dictionary
        Spawn = transform.position;
    }

    #region Healing and Damaging
    /// <summary> A standard damage function. </summary>
    public virtual bool DoDamage(int damage, bool overcharged = false)
    {
        if(armored && !overcharged)
        {
            return false;
        }

        bool dead = false;
        if (damage > 0)
        {
            health -= damage;

            //check for health-gated events
            if(events != null)
            {
                events.CheckGateProgress(health);
            }

            //only play damage SFX if it was not a killing blow so sounds don't overlap
            if (health > 0)
            {
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

                /*var ai = this.GetComponent<AIData>();
                if (ai != null)
                {
                    // squeeze!
                    ai.Wiggle(damage);
                }*/
            }
            else // Health <= 0
            {
                dead = true;
                // fruitant specific
                var ai = this.GetComponent<AIData>();
                if (ai != null)
                {
                    ai.currentLifeState = AIData.LifeState.dead;
                    if (this.tag == "Predator")
                        Kill();
                }
                health = 0;
            }
            if (healthBar != null)
            {
                healthBar.gameObject.SetActive(true);
                //Debug.Log("Update health bar");
                healthBar.value = (float)health / maxHealth;
                if (barCr != null)
                    StopCoroutine(barCr);
                barCr = StartCoroutine(ShowHealthBar());
            }

            // create a fear signal
            InstantiateSignal(4 , "Fear",  0.25f, true, true);
        }
        return dead;
    }

    /// <summary> A standard damage function. </summary>
    public virtual bool DoHeal(int restore)
    {
        bool overcharged = false;
        if (restore > 0)
        {
            health += restore;
            overcharged = (health > maxHealth);
            health = Mathf.Min(health, maxHealth);
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
                if (healSFX != null)
                {
                    var deathSoundObj = Instantiate(sfxPlayer, transform.position, transform.rotation);
                    deathSoundObj.GetComponent<PlayAndDestroy>().Play(healSFX);
                }

                // fruitant specific
                // set alive or overcharged
                var ai = this.GetComponent<AIData>();
                if (ai != null)
                {
                    if (overcharged)
                    {
                        Debug.Log("Overcharging");
                        if (this.tag == "Prey")
                        {
                            if (ai.currentLifeState == AIData.LifeState.overcharged)
                            {
                                // first stop
                                StopCoroutine(ai.OverchargeTimer(0));
                                // then start timer
                                StartCoroutine(ai.OverchargeTimer(overchargeTime));
                            }
                            else
                            {
                                // set state
                                ai.currentLifeState = AIData.LifeState.overcharged;
                                // then start timer
                                StartCoroutine(ai.OverchargeTimer(overchargeTime));
                            }
                        }
                    }
                    else
                    {
                        //revived or still alive
                        ai.currentLifeState = AIData.LifeState.alive;
                    }
                }
            }

            // create a anti fear signal
            float hp = (health) / maxHealth;
            InstantiateSignal(2, "Fear", -0.25f, true, true);
        }
        return overcharged;
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
        deathEvent.Invoke();
        Destroy(gameObject);
    }

    public virtual void SetHealth(int newHP)
    {
        health = newHP;
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
            //Debug.Log("Update health bar");
            healthBar.value = (float)health / maxHealth;
            if (barCr != null)
                StopCoroutine(barCr);
            barCr = StartCoroutine(ShowHealthBar());
        }
    }

    #endregion

    #region Signalling
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
    #endregion
}
