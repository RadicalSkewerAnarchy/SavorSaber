using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITransition
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
    /// all the decisions to evaluate
    /// </summary>
    List<AIDecision> Decisions;

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
}
