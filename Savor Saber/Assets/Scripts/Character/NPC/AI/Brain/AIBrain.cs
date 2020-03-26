using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIBrain : MonoBehaviour
{
    /// <summary>
    /// all possible states this brain can
    /// decide between
    /// </summary>
    List<AIState> States;
    AIState CurrentState;

    /// <summary>
    /// evaluates the transitions of the current state
    /// and chooses whether to transition or stay
    /// </summary>
    /// <returns>the "next" state</returns>
    public AIState ChooseNextState()
    {
        // check all transitions
        float highestPriority = Mathf.NegativeInfinity;
        AIState next = CurrentState;
        foreach (AITransition trans in CurrentState.Transitions)
        {
            bool complete = trans.IsComplete();
            if (complete)
            {
                if (trans.Priority > highestPriority)
                {
                    next = trans.NextState;
                }
            }
        }
        return next;
    }
}
