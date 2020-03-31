using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class AITransition: ScriptableObject
{
    public AIDecision[] Decisions;
    public List<AIDecision> DecisionsList => new List<AIDecision>(Decisions);

    public AITransition()
    {
        Decisions = new AIDecision[0];
    }

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
            foreach (AIDecision decide in DecisionsList)
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
            foreach (AIDecision decide in DecisionsList)
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

        foreach (AIDecision decision in DecisionsList)
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

        foreach (AIDecision decision in DecisionsList)
        {
            strs.AddRange(decision.ReturnableTags());
        }

        return strs;
    }

    /// <summary>
    /// ensure decisions start initialized
    /// </summary>
    public void InitializeDecisions()
    {
        foreach (var decide in DecisionsList)
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
        foreach (var decide in DecisionsList)
        {
            decide.SetBrain(brain);
        }
    }
}
