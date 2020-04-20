﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[DisallowMultipleComponent]
[RequireComponent(typeof(AIDecision))]
public class AITransition: MonoBehaviour
{
    /// <summary>
    /// transition to this state
    /// </summary>
    public AIState NextState;

    /// <summary>
    /// if true, ALL decisions must evaluate to true,
    /// else ANY decision my be true
    /// </summary>
    public bool AllElseAny = true;

    /// <summary>
    /// higher priority transitions are chosen over lower
    /// if transitions are equal, choose lowest index
    /// </summary>
    public int Priority = 0;

    /// <summary>
    /// The decsions that are tested if a transition is needed
    /// </summary>
    public List<AIDecision> Decisions;


    /// <summary>
    /// check all decisions, decide if next state is ready
    /// </summary>
    /// <returns></returns>
    public bool IsComplete()
    {
        bool complete;

        if (AllElseAny)
        {
            // ALL must be TRUE
            complete = true;
            foreach (AIDecision decide in Decisions)
            {
                bool eval = decide.Evaluate();
                if (!eval)
                {
                    complete = false;
                    break;
                }
            }
        }
        else
        {
            // ANY may be true
            complete = false;
            foreach (AIDecision decide in Decisions)
            {
                bool eval = decide.Evaluate();
                if (eval)
                {
                    complete = true;
                    break;
                }
            }
        }

        return complete;
    }


    /// <summary>
    /// send any targets needed for the transition
    /// </summary>
    /// <returns>targets</returns>
    public List<GameObject> CollectReturnableObjects()
    {
        var objs = new List<GameObject>();

        foreach (AIDecision decision in Decisions)
        {
            objs.AddRange(decision.ReturnableObjects());
        }

        return objs;
    }

    /// <summary>
    /// send any tags needed for the transition
    /// </summary>
    /// <returns>target tags</returns>
    public List<string> CollectReturnableTags()
    {
        var strs = new List<string>();

        foreach (AIDecision decision in Decisions)
        {
            strs.AddRange(decision.ReturnableTags());
        }

        return strs;
    }

    private void Awake()
    {
        var decisions = GetComponents<AIDecision>();
        Decisions = new List<AIDecision>(decisions);


        if (Decisions.Count == 0)
        {
            Debug.Log($"{this.name} has no decisions to evaluate");
        }
        else
        {
            InitializeDecisions();
        }
    }

    /// <summary>
    /// ensure decisions start initialized
    /// </summary>
    public void InitializeDecisions()
    {
        foreach (var decide in Decisions)
        {
            decide.Initialize();
        }
    }


    /// <summary>
    /// allow transition to access brain's knowledge
    /// </summary>
    private AIBrain myBrain;
    public void SetBrain(AIBrain brain)
    {
        myBrain = brain;
        foreach (var decide in Decisions)
        {
            decide.SetBrain(brain);
        }
    }
}