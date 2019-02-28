﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ONLY REQUIRE FOR DEBUGGING OR HARDCODED ABSTRACT BehaviorS
/// </summary>
//[RequireComponent(typeof(MonsterBehavior))]
[RequireComponent(typeof(MonsterBehavior))]
[RequireComponent(typeof(MonsterProtocols))]
[RequireComponent(typeof(MonsterChecks))]
[RequireComponent(typeof(UtilityCurves))]
[RequireComponent(typeof(SpriteRenderer))]

public class AIData : CharacterData
{
    #region Global Variables
    #region NormalValues
    /// <summary> A delegate that returns a float between 0 and 1 </summary>
    public delegate float GetNormalValue();
    /// <summary> A dictionary of normalized AI values to be used by Utility curves</summary>
    private Dictionary<string, GetNormalValue> _values;
    private Dictionary<string, Vector2> _vectors;
    #endregion
    #region Behavior/Protocol
    #region Behaviors
    /// <summary> my current state </summary>
    public enum Behave
    {
        Idle,
        Chase,
        Attack,
        Flee,
        Socialize,
        Feed
    }
    #endregion
    public Behave currentBehavior = Behave.Idle;
    public Behave previousBehavior = Behave.Idle;
    #region Protocols
    /// <summary> my current state </summary>
    public enum Protocols
    {
        Melee,
        Ranged,
        Lazy,
        Guard,
        Party,
        Swarm,
        Feast,
        Console,
        Runaway,
        Conga
    }
    #endregion
    public Protocols currentProtocol = Protocols.Lazy;
    #endregion
    #region Timers
    [SerializeField]
    public float DecisionTimer;
    [SerializeField]
    [Range(0.25f, 15f)]
    public float DecisionTimerReset = 10f;
    [SerializeField]
    [Range(0f, 4f)]
    public float DecisionTimerVariance = 2f;    
    #endregion
    #region Components
    private MonsterBehavior Behavior;
    private MonsterProtocols Protocol;
    public MonsterChecks Checks;
    private UtilityCurves Curves;
    #endregion
    #region Unfinished
    public SignalApplication Awareness = null;
    public List<GameObject> Friends;
    public List<GameObject> Enemies;
    public List<RecipeData.Flavors> FoodPreference;
    public Queue<IngredientData> Stomach = new Queue<IngredientData>();
    #endregion
    #endregion
    private void Start()
    {        
        #region Initialize Components
        Behavior = GetComponent<MonsterBehavior>();
        Protocol = GetComponent<MonsterProtocols>();
        Checks = GetComponent<MonsterChecks>();
        Curves = GetComponent<UtilityCurves>();
        #endregion
        #region Initialize Data
        InitializeCharacterData();
        InitializeNormalValues();
        #endregion

        _vectors = new Dictionary<string, Vector2> {
            {"Player", new Vector2(0f, 0f) }
        };
    }
    protected void InitializeNormalValues()
    {
        _values = new Dictionary<string, GetNormalValue>()
        {
            {"Fear", () => moods["Fear"] },
            {"Hunger", () => moods["Hunger"] },
            {"Hostility", () => moods["Hostility"] },
            {"Friendliness", () => moods["Friendliness"] },
            {"EnemyDistance", () => Normalize(Vector2.Distance(transform.position, Enemies[0].transform.position), Perception) },
            {"Health", () => NormalizeInt(health, maxHealth) }
        };
    }
    /// <summary>
    /// Updates protocol
    /// </summary>
    private void LateUpdate()
    {
        UpdateProtocol();
    }
    public float Normalize(float now, float max)
    {
        return now / max;
    }
    public float NormalizeInt(int now, int max)
    {
        return now / (float)max;
    }
    /// <summary> 
    ///  Get a normalized value from the value dictionary. if the value is not present, returns -1
    /// </summary>
    public float getNormalizedValue(string value)
    {
        if (!_values.ContainsKey(value))
        {
            Debug.LogError(value + " is not a valid AI value, returning -1");
            return -1f;
        }
        return _values[value]();
    }
    public void ManualDecision()
    {
        DecisionTimer = -1f;
    }

    /// <summary>
    /// If its time to make a new decision, choose protocol based on curves and call switch statement
    /// </summary>
    private void UpdateProtocol()
    {
        if (DecisionTimer < 0)
        {
            // CALCULATE AND ACQUIRE NEW STATE:
            currentProtocol = Curves.DecideState();

            //Debug.Log(this.gameObject.name + " is getting a New Protocol: " + currentProtocol);

            // RESET DECISION TIMER
            DecisionTimer = DecisionTimerReset + Random.Range(-DecisionTimerVariance, DecisionTimerVariance);

            // UPDATE AWARENESS: creatures, player, and drops
            Checks.AwareNearby();

            // UPDATE HUNGER??
            UpdateHunger();
        }
        else
        {
            DecisionTimer -= Time.deltaTime;
        }

        ProtocolSwitch();
    }
    /// <summary>
    /// Case switch based on current protocol
    /// </summary>
    private void ProtocolSwitch()
    {
        switch (currentProtocol)
        {
            case Protocols.Melee:
            // melee
                Protocol.Melee();
                break;
            // ranged
            case Protocols.Ranged:

                Protocol.Ranged();
                break;
            // lazy
            case Protocols.Lazy:
                Protocol.Lazy();
                break;
            // guard
            case Protocols.Guard:

                Protocol.Guard();
                break;
            // party
            case Protocols.Party:
                Protocol.Party();
                break;
            // swarm
            case Protocols.Swarm:
                Protocol.Swarm();
                break;
            // feast
            case Protocols.Feast:
                Protocol.Feast();
                break;
            // console
            case Protocols.Console:
                Protocol.Console();
                break;
            // Runaway
            case Protocols.Runaway:
                Protocol.Runaway();
                break;
            // Conga
            case Protocols.Conga:
                Protocol.Conga();
                break;

            default:
                Debug.Log("YOU SHOULD NEVER BE HERE!");
                break;
        }
    }
    // UpdateHunger()
    // damage me if im hungry
    private void UpdateHunger()
    {
        // have random chance to be hungry
        float rand = Random.Range(0f, 100f);
        if (rand < 50)
        {
            // create a signal that subtracts from my hunger
            InstantiateSignal(0.1f, "Hunger", 0.1f, false, true);

            // die or
            // change sprite color
            float hunger = moods["Hunger"];
            //Debug.Log("Im getting hungrier: " + this.gameObject.name + "'s hunger = " + hunger);
            if (hunger >= 1f)
            {
                // hurt me
                health -= 1;
                if (health <= 0)
                {
                    // ded
                    Monster me = this.gameObject.GetComponent<Monster>();
                    me.Kill();
                }
            }
            else if (hunger >= 0.75f)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(116f, 116f, 0f);
            }
        }
    }

    // InstantiateSignal()
    // create a signal that subtracts
    public GameObject InstantiateSignal(float size, string mod, float modifier, bool hitall, bool hitself)
    {
        GameObject obtainSurroundings = Instantiate(Checks.signalPrefab, transform.position, Quaternion.identity) as GameObject;
        SignalApplication signalModifier = obtainSurroundings.GetComponent<SignalApplication>();
        signalModifier.SetSignalParameters(this.gameObject, size, new Dictionary<string, float>() { { mod, modifier } }, hitall, hitself);

        return obtainSurroundings;
    }
}
