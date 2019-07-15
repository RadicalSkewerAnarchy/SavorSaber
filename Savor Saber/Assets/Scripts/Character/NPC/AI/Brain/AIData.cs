using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
        Feed,
        Console
    }
    #endregion
    public Behave currentBehavior = Behave.Idle;
    [HideInInspector]
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
        Conga,
        Chase,
        Wander,
        Ride,
        Scare
    }
    #endregion
    public Protocols currentProtocol = Protocols.Lazy;
    public List<TileNode> path;
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
    [HideInInspector]
    public MonsterBehavior Behavior;
    private MonsterProtocols Protocol;
    [HideInInspector]
    public MonsterChecks Checks;
    private UtilityCurves Curves;

    #endregion
    #region Unfinished
    [HideInInspector]
    public SignalApplication Awareness = null;
    [HideInInspector]
    public List<GameObject> Friends;
    [HideInInspector]
    public List<GameObject> Enemies;
    [HideInInspector]
    public List<RecipeData.Flavors> FoodPreference;
    [HideInInspector]
    public Queue<IngredientData> Stomach = new Queue<IngredientData>();
    #endregion
    [HideInInspector]
    public bool updateAI = false;
    [HideInInspector]
    public bool updateBehavior = true;
    public bool meleeHunter = true;
    [HideInInspector]
    public Vector3 rideVector;
    #endregion

    /// <summary>
    /// set necessary values and components
    /// </summary>
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
        path = new List<TileNode>();

        _vectors = new Dictionary<string, Vector2> {
            {"Player", new Vector2(0f, 0f) }
        };
    }

    /// <summary>
    /// set links to mood dictionary values
    /// </summary>
    protected void InitializeNormalValues()
    {
        _values = new Dictionary<string, GetNormalValue>()
        {
            {"Fear", () => moods["Fear"] },
            {"Hunger", () => moods["Hunger"] },
            {"Hostility", () => moods["Hostility"] },
            {"Friendliness", () => moods["Friendliness"] },
            {"EnemyDistance", () => Normalize(Vector2.Distance(transform.position, Enemies[0].transform.position), Perception) },
            {"Health", () => NormalizeInt(health, maxHealth) },
            {"PlayerDistance", () => Normalize(Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position), Perception) }
        };
    }
    /// <summary>
    /// normalize variables
    /// </summary>
    /// <param name="now">current value</param>
    /// <param name="max">maximum value</param>
    /// <returns></returns>
    public float Normalize(float now, float max)
    {
        return now / max;
    }
    public float NormalizeInt(int now, int max)
    {
        return now / (float)max;
    }

    /// <summary>
    /// Updates protocol
    /// </summary>
    private void Update()
    {
        if (updateAI && !EventTrigger.InCutscene)
        {
            Act();
        }
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

    /// <summary>
    /// Sets any decision timers to 0 to force fruitant to update state on next Update()
    /// </summary>
    public void ManualDecision()
    {
        DecisionTimer = -1f;
    }

    /// <summary>
    /// If its time to make a new decision, choose protocol based on curves and call switch statement
    /// </summary>
    public void Act()
    {
        if (DecisionTimer < 0)
        {
            bool decideState = true;
            // CONGA LOGIC
            // as long as fear is not 1, stay conga
            if (currentProtocol == Protocols.Conga)
            {
                if (moods["Hunger"] != 1)
                    decideState = false;
            }
            else if (currentProtocol == Protocols.Ride)
            {
                decideState = false;
            }
            else if (Checks.AwareHowManyEnemies() > 0)
            {
                decideState = false;
                currentProtocol = Protocols.Runaway;
            }


            // UPDATE AWARENESS: creatures, player, and drops
            Checks.AwareNearby();

            // DECIDE
            // CALCULATE AND ACQUIRE NEW STATE:
            if (decideState)
            {
                currentProtocol = Curves.DecideState();
            }

            // RESET DECISION TIMER
            DecisionTimer = DecisionTimerReset + Random.Range(-DecisionTimerVariance, DecisionTimerVariance);
        }
        else
        {
            DecisionTimer -= Time.deltaTime;
        }

        ProtocolSwitch();
    }
    /// <summary>
    /// Case switch based on current protocol
    /// Dependant on fruitant: chain together different ifs
    /// </summary>
    public virtual void ProtocolSwitch()
    {
        switch (currentProtocol)
        {
            case Protocols.Melee:
            // melee
                Protocol.Melee(null);
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
                Protocol.Feast(meleeHunter);
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
            // Chase
            case Protocols.Chase:
                Protocol.NavChase();
                break;
            // Wander
            case Protocols.Wander:
                Protocol.Wander(5f, 5f);
                break;
            // ride
            case Protocols.Ride:
                Protocol.Ride(rideVector);
                break;
            // scare
            case Protocols.Scare:
                Protocol.Scare();
                break;
            default:
                Debug.Log("YOU SHOULD NEVER BE HERE!");
                break;
        }
    }

    /// <summary>
    /// create a hunger signal... may not need because health and hunger are now the same...
    /// </summary>
    /// <param name="amount"> from -1 to 1</param>
    public void UpdateHunger(float amount)
    {
        // create a signal that subtracts from my hunger
        InstantiateSignal(0.1f, "Hunger", amount, false, true);
    }

    /// <summary>
    /// enable and disable fruitant thinking if on/off screen
    /// </summary>
    private void OnBecameVisible()
    {
        updateAI = true;
    }
    private void OnBecameInvisible()
    {
        var plDat = PlayerController.instance?.GetComponent<PlayerData>()?.party;
        if (plDat == null || !plDat.Contains(gameObject))
            updateAI = false;
    }

    // getters for monster info
    public MonsterBehavior getBehavior()
    {
        return this.Behavior;
    }
    public MonsterProtocols getProtocol()
    {
        return this.Protocol;
    }
    public MonsterChecks getChecks()
    {
        return this.Checks;
    }
}
