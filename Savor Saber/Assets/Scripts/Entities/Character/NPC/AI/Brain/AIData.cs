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
    [Header("AI Fields")]
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
                Overcharged,
                Pollinate,
                Attack
            }
            public Protocols currentProtocol = Protocols.Lazy;
            [HideInInspector]
            public Protocols previousProtocol = Protocols.Lazy;

            private Queue<Command> ActionQueue;
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

            public bool CommandCompleted = true;
            public bool CommandInProgress = false;
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
        //[HideInInspector]
        public MonsterBehavior Behavior;
        //[HideInInspector]
        public MonsterProtocols Protocol;
        //[HideInInspector]
        public MonsterChecks Checks;
        //[HideInInspector]
        public SpriteRenderer sRenderer;

        #endregion

        #region Unfinished
        [HideInInspector]
        public SignalApplication Awareness = null;
        [HideInInspector]
        public List<RecipeData.Flavors> FoodPreference;
        [HideInInspector]
        public Queue<IngredientData> Stomach = new Queue<IngredientData>();
        private Squeezer squeeze;
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

        //what flavor an AI is changes the trust buff it grants as a party member (only useful for fruitants, Drones are None
        public RecipeData.Flavors flavor;
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
        sRenderer = GetComponent<SpriteRenderer>();
        events = GetComponent<HealthGatedEvent>();
        #endregion
        #region Initialize Data
        InitializeCharacterData();
        #endregion

        ActionQueue = new Queue<Command>();
        path = new List<TileNode>();
        squeeze = GetComponent<Squeezer>();
        if (squeeze != null)
        {
            squeeze.horiRange = Random.Range(-0.2f, 0.2f);
            squeeze.horiSpeed = Random.Range(2f, 4f);
            squeeze.horiBase = Random.Range(0f, 0.5f);
            squeeze.vertRange = Random.Range(-0.2f, 0.2f);
            squeeze.vertSpeed = Random.Range(2f, 4f);
            squeeze.vertBase = Random.Range(0f, 0.5f);
            squeeze.SetGoals();
            squeeze.activate = false;
        }

        _vectors = new Dictionary<string, Vector2> {
            {"Player", new Vector2(0f, 0f) }
        };
    }

    /// <summary>
    /// Updates protocol
    /// </summary>
    private void Update()
    {
        if ((updateAI || !CommandCompleted) && !EventTrigger.InCutscene)
        {
            Act();
        }
    }

    #region Decision Making
    /// <summary>
    /// If its time to make a new decision, 
    ///     choose protocol  
    /// call switch statement
    /// </summary>
    public void Act()
    {
        if (DecisionTimer <= 0)
        {
            // UPDATE AWARENESS: creatures, player, and drops
            Checks.AwareNearby();

            //  DECIDE STATE
            if (CommandCompleted)
                currentProtocol = DecideProtocol();

            // RESET DECISION TIMER
            DecisionTimer = DecisionTimerReset 
                + Random.Range(-DecisionTimerVariance, DecisionTimerVariance);
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
    /// Case switch based on current protocol and life
    /// </summary>
    public void ProtocolSwitch()
    {
        if (currentProtocol != previousProtocol)
        {
            // exit old protocol...
            OnProtocolExit(previousProtocol);
            /// ... enter new protocol
            OnProtocolEnter(currentProtocol);
            // change previous
            previousProtocol = currentProtocol;
        }
        else
        {
            if (currentLifeState != previousLifeState)
            {
                // exit old life...
                AlwaysOnStateExit(previousLifeState);
                OnStateExit(previousLifeState);
                // ... enter new life
                AlwaysOnStateEnter(currentLifeState);
                OnStateEnter(currentLifeState);
                // change previous
                previousLifeState = currentLifeState;
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
                        Debug.Log("fruitant has no life?");
                        break;
                }
            }
        }
    }

    #region Entrance and Exit
    #region Enter/Exit Protos
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
    #endregion
    #region Enter/Exit Life
    /// <summary>
    /// CALL THIS BEFORE OnStateEnter()
    /// modifies fruitant when becoming overcharged, alive and dead
    /// </summary>
    /// <param name="p">old protocol</param>
    public virtual void OnStateExit(LifeState s)
    {
        switch (s)
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
    private void AlwaysOnStateExit(LifeState s)
    {
        switch (s)
        {
            case LifeState.overcharged:
                sRenderer.color = Color.white;
                break;
            case LifeState.dead:
                sRenderer.color = Color.white;
                this.GetComponent<Animator>().StopPlayback();
                break;
            default:
                // nothing at all
                break;
        }
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
            default:
                // nothing at all
                break;
        }
    }

    /// <summary>
    /// CALL THIS AFTER OnStateExit()
    /// modifies fruitant when having just been overcharged, alive, or dead
    /// </summary>
    /// <param name="p">new protocol</param>
    private void AlwaysOnStateEnter(LifeState s)
    {
        switch (s)
        {
            case LifeState.dead:
                sRenderer.color = Color.grey;
                this.GetComponent<Animator>().StartPlayback();
                break;
            default:
                // nothing at all
                break;
        }
    }
    #endregion

    public virtual IEnumerator OverchargeTimer(float time)
    {
        yield return new WaitForSeconds(time);
        if (this.currentLifeState != LifeState.dead)
            this.currentLifeState = LifeState.alive;
        yield return null;
    }

    #endregion

    #region Acting Upon Life
    public virtual void WhileAlive()
    {
        switch (currentProtocol)
        {
            case Protocols.Melee:
                // melee
                Protocol.NavMelee(null);
                break;
            // ranged
            case Protocols.Ranged:
                Protocol.NavRanged();
                break;
            // lazy
            case Protocols.Lazy:
                Protocol.Lazy();
                break;
            // party
            case Protocols.Party:
                Protocol.Party();
                break;
            // Runaway
            case Protocols.Runaway:
                Protocol.NavRunaway();
                break;
            // Chase
            case Protocols.Chase:
                Protocol.NavChase();
                break;
            // Wander
            case Protocols.Wander:
                Protocol.NavWander(5f, 5f);
                break;
            // ride
            case Protocols.Ride:
                Protocol.Ride(rideVector);
                break;
            // attack
            case Protocols.Attack:
                if (meleeHunter)
                    Protocol.Melee(Checks.specialTarget);
                else
                    Protocol.Ranged(Checks.specialTarget);
                break;
            default:
                Debug.Log(this.name + "is not behaving correctly: NO VALID PROTOCOL");
                break;
        }
    }
    public virtual void WhileDead()
    {
        currentProtocol = Protocols.Dead;
    }
    public virtual void WhileOvercharged()
    {
        WhileAlive();
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

    #region Monster Info Getters and Setters
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
    public bool GetOvercharged()
    {
        return (currentLifeState == LifeState.overcharged);
        //return (health > maxHealth && currentLifeState == LifeState.overcharged);
    }

    public void EnqueueAction(Command c)
    {
        this.ActionQueue.Enqueue(c);
    }
    public void ClearActionQueue()
    {
        this.ActionQueue.Clear();
    }

    public void Wiggle(int dmg=1)
    {
        if (squeeze != null)
        {
            if (squeeze.activate)
            {
                StopCoroutine(Wiggling());
                squeeze.horiSpeed = dmg*2;
                squeeze.vertSpeed = dmg*2;
                StartCoroutine(Wiggling(dmg*2));
            }
            else
            {
                squeeze.activate = true;
                squeeze.horiSpeed = dmg;
                squeeze.vertSpeed = dmg;
                StartCoroutine(Wiggling(dmg));
            }
        }
    }

    private IEnumerator Wiggling(float time=3)
    {
        yield return new WaitForSeconds(time);
        squeeze.activate = false;
    }
    #endregion


}
