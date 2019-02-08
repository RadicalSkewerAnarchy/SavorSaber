using System.Collections;
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

public class AIData : CharacterData
{
    #region NormalValues
    /// <summary> A delegate that returns a float between 0 and 1 </summary>
    public delegate float GetNormalValue();
    /// <summary> A dictionary of normalized AI values to be used by Utility curves</summary>
    private Dictionary<string, GetNormalValue> _values;
    private Dictionary<string, Vector2> _vectors;
    #endregion
    #region Behaviors
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
        Runaway
    }
    #endregion
    public Protocols currentProtocol = Protocols.Lazy;
    #endregion
    #region Timers

    [SerializeField]
    public float DecisionTimer;
    [SerializeField]
    [Range(5f, 15f)]
    public float DecisionTimerReset = 10f;
    [SerializeField]
    [Range(0f, 4f)]
    public float DecisionTimerVariance = 2f;
    #endregion
    #region NearbyCreatures and Food
    /// <summary> lists that may be needed for certain target positions or objects </summary>
    List<GameObject> TargetObjects = new List<GameObject>();
    GameObject AwarenessObject;
    Vector2 TargetPosition;
    /// <summary>
    /// empty array of nearby seen creatures
    /// </summary>
    public Collider2D[] NearbyCreatures = new Collider2D[10];
    Vector2 Target;
    #endregion
    #region MonstersBrain and inventory;
    /// <summary>
    /// Monster Behaviors, Monster Protocols, Monster Checks, Utility Curves
    /// </summary>
    private MonsterBehavior Behavior;
    private MonsterProtocols Protocol;
    public MonsterChecks Checks;
    private UtilityCurves Curves;

    /// Friends, Enemies, Food Pref, Stomach
    public SignalApplication Awareness = null;
    public List<GameObject> Friends;
    public List<GameObject> Enemies;
    public List<RecipeData.Flavors> FoodPreference;
    public Queue<GameObject> Stomach;
    #endregion
    

    private void Start()
    {
        Behavior = GetComponent<MonsterBehavior>();
        Protocol = GetComponent<MonsterProtocols>();
        Checks = GetComponent<MonsterChecks>();
        Curves = GetComponent<UtilityCurves>();
        //Behavior.UpdateSpeed(speed);
        // set mood values into dictionary
        moods.Add("Fear", fear);
        moods.Add("Hunger", hunger);
        moods.Add("Hostility", hostility);
        moods.Add("Friendliness", friendliness);

        _values = new Dictionary<string, GetNormalValue>()
        {
            {"Fear", () => {return moods["Fear"]; } },
            {"Hunger", () => {return moods["Hunger"]; } },
            {"Hostility", () => {return moods["Hostility"]; } },
            {"Friendliness", () => {return moods["Friendliness"]; } },
            {"PlayerDistance", () => { return 1; } }, // DEBUG
            {"FireDistance", () => {return 1; } }, //DEBUG
            {"Health", () => {return Normal(health, maxHealth); } }
        };

        _vectors = new Dictionary<string, Vector2> {
            {"Player", new Vector2(0f, 0f) }
        };

        // Naming for future creature tracking
        gameObject.name = gameObject.name + gameObject.GetInstanceID().ToString();
    }

    private void Update()
    {
        // check current state
        // acquire necessary data
        // act on current state

        // silly debug updates
        //fear = moods["Fear"];
        //hunger = moods["Hunger"];
        //hostility = moods["Hostility"];
        //friendliness = moods["Friendliness"];

        // UPDATE Decision
        if (DecisionTimer < 0)
        {
            // update state
            currentProtocol = Curves.DecideState();
            Debug.Log("Getting New Protocol: " + currentProtocol);
            // reset decision timer
            DecisionTimer = DecisionTimerReset + Random.Range(-DecisionTimerVariance, DecisionTimerVariance);
            // update awareness of creatures
            Checks.AwareNearby();
        }
        else
        {
            DecisionTimer -= Time.deltaTime;
        }

        // SWITCH protocol
        switch (currentProtocol)
        {
            // melee
            case Protocols.Melee:
                //Behavior.MoveTo(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                Protocol.Melee();
                break;
            // ranged
            case Protocols.Ranged:
                //Behavior.MoveTo(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                Protocol.Ranged();
                break;
            // lazy
            case Protocols.Lazy:
                Protocol.Lazy();
                break;
            // guard
            case Protocols.Guard:
                //Behavior.MoveFrom(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                Protocol.Guard();
                break;
            // party
            case Protocols.Party:
                //Behavior.MoveFrom(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                Protocol.Party();
                break;
            // swarm
            case Protocols.Swarm:
                //Behavior.MoveFrom(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                Protocol.Swarm();
                break;
            // feast
            case Protocols.Feast:
                //Behavior.MoveFrom(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                Protocol.Feast();
                break;
            // console
            case Protocols.Console:
                //Behavior.MoveFrom(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                Protocol.Console();
                break;
            // Runaway
            case Protocols.Runaway:
                //Behavior.MoveFrom(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), Speed);
                Protocol.Runaway();
                break;
            // default
            default:
                Debug.Log("YOU SHOULD NEVER BE HERE!");
                break;
        }
        
    }

    public float Normal(int now, int max)
    {
        return now / (float)max;
    }
    /// <summary> Get a normalized value from the value dictionary. if the value is not present, returns -1 </summary>
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

}
