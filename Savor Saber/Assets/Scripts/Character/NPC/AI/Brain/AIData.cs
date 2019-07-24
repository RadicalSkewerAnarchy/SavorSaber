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
            public Behave currentBehavior = Behave.Idle;
            [HideInInspector]
            public Behave previousBehavior = Behave.Idle;
            #endregion

            #region Protocols
            /// <summary> my current state </summary>
            public enum Protocols
            {
                Melee,
                Ranged,
                Lazy,
                Party,
                Feast,
                Console,
                Runaway,
                Conga,
                Chase,
                Wander,
                Ride,
                Scare,
                Dead,
                Ability,
                Overcharged
            }
            public Protocols currentProtocol = Protocols.Lazy;
            [HideInInspector]
            public Protocols previousProtocol = Protocols.Lazy;
            #endregion

            #region Life and Move States
            public enum LifeState
            {
                alive,
                overcharged,
                dead
            }
            public LifeState currentLifeState = LifeState.alive;
            [HideInInspector]
            public LifeState previousLifeState = LifeState.alive;

            public enum MoveState
            {
                idle,
                move,
                ride
            }
            public MoveState currentMoveState = MoveState.idle;
            [HideInInspector]
            public MoveState previousMoveState = MoveState.idle;
            #endregion

        #endregion

        #region Timers
    [SerializeField]
        public float DecisionTimer;
        [SerializeField]
        [Range(0.25f, 15f)]
        public float DecisionTimerReset = 1f;
        [SerializeField]
        [Range(0f, 4f)]
        public float DecisionTimerVariance = 0.2f;
        #endregion

        #region Components
        [HideInInspector]
        public MonsterBehavior Behavior;
        private MonsterProtocols Protocol;
        [HideInInspector]
        public MonsterChecks Checks;
        private UtilityCurves Curves;
        private SpriteRenderer Renderer;

        #endregion

        #region Unfinished
        [HideInInspector]
        public SignalApplication Awareness = null;
        [HideInInspector]
        public List<RecipeData.Flavors> FoodPreference;
        [HideInInspector]
        public Queue<IngredientData> Stomach = new Queue<IngredientData>();
        #endregion

        #region Misc Info
        [HideInInspector]
        public bool updateAI = false;
        [HideInInspector]
        public bool updateBehavior = true;
        public bool meleeHunter = true;
        [HideInInspector]
        public Vector3 rideVector;
        [HideInInspector]
        public List<TileNode> path;
    #endregion

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
        Renderer = GetComponent<SpriteRenderer>();
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
    /// Updates protocol
    /// </summary>
    private void Update()
    {
        if (updateAI && !EventTrigger.InCutscene)
        {
            Act();
        }
    }

    #region Decision Making
    /// <summary>
    /// If its time to make a new decision, choose protocol based on curves and call switch statement
    /// </summary>
    public void Act()
    {
        if (DecisionTimer <= 0)
        {
            // UPDATE AWARENESS: creatures, player, and drops
            Checks.AwareNearby();

            //  DECIDE STATE
            currentProtocol = DecideProtocol();

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
    /// Override: apply logic specific to the fruitant or drone
    /// to know what and how it should transition
    /// </summary>
    /// <returns>the new or old state based on surroundings and stats</returns>
    public virtual Protocols DecideProtocol()
    {
        Protocols myNewState = currentProtocol;
        return myNewState;
    }

    /// <summary>
    /// Case switch based on current protocol
    /// Dependant on fruitant: chain together different ifs
    /// </summary>
    public void ProtocolSwitch()
    {
        if (currentProtocol != previousProtocol)
        {
            // exit old...
            OnProtocolExit(previousProtocol);
            /// ... enter new
            OnProtocolEnter(currentProtocol);
        }
        else
        {
            if (currentLifeState != previousLifeState)
            {
                // exit old...
                OnStateExit(previousLifeState);
                /// ... enter new
                OnStateEnter(currentLifeState);
            }
            else
            {
                // act upon current protocol
                // based on current life state
                switch (currentLifeState)
                {
                    case LifeState.alive:
                        WhileAlive();
                        break;
                    case LifeState.dead:
                        WhileDead();
                        break;
                    case LifeState.overcharged:
                        WhileOvercharged();
                        break;
                    default:
                        Debug.Log("fruitant has no current life state");
                        break;
                }
            }
        }
    }

    #region Entrance and Exit
    /// <summary>
    /// CALL THIS BEFORE OnStateEnter()
    /// when enterring any state, call this to modify any
    /// unique occurences for entering that Protocol
    /// </summary>
    /// <param name="p">old protocol</param>
    public virtual void OnProtocolExit(Protocols p)
    {
        switch (p)
        {
            default:
                // nothing at all
                break;
        }

        // set previous to this one
        previousProtocol = currentProtocol;
    }

    /// <summary>
    /// CALL THIS AFTER OnStateExit()
    /// when enterring any state, call this to modify any
    /// unique occurences for entering that Protocol
    /// </summary>
    /// <param name="p">new protocol</param>
    public virtual void OnProtocolEnter(Protocols p)
    {
        switch (p)
        {
            default:
                // nothing at all
                break;
        }
    }

    /// <summary>
    /// CALL THIS BEFORE OnStateEnter()
    /// modifies fruitant when becoming overcharged, alive and dead
    /// </summary>
    /// <param name="p">old protocol</param>
    public virtual void OnStateExit(LifeState s)
    {
        switch (s)
        {
            case LifeState.dead:
                Renderer.color = Color.white;
                break;
            default:
                // nothing at all
                break;
        }

        // set previous to this one
        previousLifeState = currentLifeState;
    }

    /// <summary>
    /// CALL THIS AFTER OnStateExit()
    /// modifies fruitant when having just been overcharged, alive, or dead
    /// </summary>
    /// <param name="p">new protocol</param>
    public virtual void OnStateEnter(LifeState s)
    {
        switch (s)
        {
            case LifeState.dead:
                Renderer.color = Color.grey;
                break;
            default:
                // nothing at all
                break;
        }
    }
    #endregion

    #region Acting Upon Life
    public virtual void WhileAlive()
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
            // party
            case Protocols.Party:
                Protocol.Party();
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
    public virtual void WhileDead()
    {

    }
    public virtual void WhileOvercharged()
    {

    }
    #endregion

    /// <summary>
    /// Sets any decision timers to 0 to force fruitant to update state on next Update()
    /// </summary>
    public void ManualDecision()
    {
        DecisionTimer = 0;
    }

    #endregion

    #region Brain Enabling
    /// <summary>
    /// enable and disable fruitant thinking if on/off screen
    /// </summary>
    private void OnBecameVisible()
    {
        updateAI = true;
    }
    private void OnBecameInvisible()
    {
        if (PlayerController.instance == null)
            return;
        var plDat = PlayerController.instance?.GetComponent<PlayerData>()?.party;
        if (plDat == null || !plDat.Contains(gameObject))
            updateAI = false;
    }
    #endregion

    #region Monster Info Getters
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

    #endregion

    #region Normal Values and Calculations
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
            {"EnemyDistance", () => Normalize(Vector2.Distance(transform.position, Checks.Enemies[0].transform.position), Perception) },
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
    #endregion

}
