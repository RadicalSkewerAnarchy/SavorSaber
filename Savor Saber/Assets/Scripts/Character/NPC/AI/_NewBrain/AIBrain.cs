using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIBrain : MonoBehaviour
{
    /// <summary>
    /// all possible states this brain can decide between
    /// </summary>
    [SerializeField]
    public AIState CurrentState;
    [SerializeField]
    public List<AIState> States;

    /// <summary>
    /// should the ai be updated
    /// </summary>
    public bool UpdateAI { get; private set; } = true;
    public bool CommandCompleted { get; private set; } = true;
    private bool ShouldUpdateAI => !EventTrigger.InCutscene && (UpdateAI || !CommandCompleted);

    /// <summary>
    /// what the agent can see
    /// </summary>
    public AIAwareness Awareness { get; private set; } = AIAwareness.DefaultAwareness();
    public float Perception { get; private set; } = 5;
    public List<GameObject> ObjectsInPerception { get; private set; }

    [HideInInspector]
    public AICharacterData CharacterData;

    /// <summary>
    /// evaluates the transitions of the current state
    /// and chooses whether to transition or stay
    /// </summary>
    /// <returns>the "next" state</returns>
    public Tuple<AIState, AITransition> ChooseNextState()
    {
        float highestPriority = Mathf.NegativeInfinity;
        AIState nextState = CurrentState;
        AITransition transition = null;
        foreach (AITransition trans in CurrentState.Transitions)
        {
            bool complete = trans.IsComplete();
            if (complete)
            {
                if (trans.Priority > highestPriority)
                {
                    nextState = trans.NextState;
                    transition = trans;
                }
            }
        }

        return new Tuple<AIState, AITransition>(nextState , transition);
    }


    /// <summary>
    /// INITIALIZE THE BRAIN
    /// </summary>
    private void Awake()
    {
        // get character data
        CharacterData = GetComponentInParent<AICharacterData>();
        // initialize brain cascade
        foreach (var state in States)
        {
            state.SetBrain(this);
        }
        ObjectsInPerception = new List<GameObject>();
        CurrentState = States[0];
    }


    /// <summary>
    /// 1. decide whether or not to change state
    /// 2. check perception
    /// </summary>
    private void FixedUpdate()
    {
        if (ShouldUpdateAI)
        {
            // state changes
            Tuple<AIState, AITransition> change = ChooseNextState();
            AITransition transition = change.Item2;
            if (transition != null)
            {
                AIState nextState = change.Item1;

                if (this.States.Contains(nextState))
                {
                    CurrentState.OnExit();
                    nextState.OnEnter();

                    nextState.SetTargets(transition.CollectReturnableObjects(), transition.CollectReturnableTags());
                    transition.InitializeDecisions();

                    CurrentState = nextState;
                }
                else
                {
                    Debug.Log($"{this.name} DOES NOT HAVE THIS STATE AVAILABLE: {nextState}");
                }
            }

            // perception update
            ObjectsInPerception.Clear();
            ObjectsInPerception.AddRange(Awareness.Perceive(this.gameObject, this.Perception));
        }
    }

    /// <summary>
    /// perform current state
    /// </summary>
    private void Update()
    {
        // perform if able
        if (ShouldUpdateAI)
        {
            CurrentState.Perform();
        }
    }


    public bool IsAwareOf(GameObject find)
    {
        return ObjectsInPerception.Contains(find);
    }

    public List<GameObject> AllSeeableWithTag(string tag)
    {
        List<GameObject> tagged = new List<GameObject>();
        foreach (GameObject see in ObjectsInPerception)
        {
            if (see.CompareTag(tag))
            {
                tagged.Add(see);
            }
        }
        return tagged;
    }

    /// <summary>
    /// enable and disable fruitant thinking if on/off screen
    /// </summary>
    private void OnBecameVisible()
    {
        UpdateAI = true;
    }
    private void OnBecameInvisible()
    {
        if (PlayerController.instance == null)
            return;
        var plDat = PlayerController.instance?.GetComponent<PlayerData>()?.party;
        if (plDat == null || !plDat.Contains(gameObject))
            UpdateAI = false;
    }

}
