using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIDecision : MonoBehaviour
{
    /// <summary>
    /// used to check if state should transition
    /// </summary>
    /// <returns>evaluation complete</returns>
    virtual public bool Evaluate()
    {
        return true;
    }
    virtual public void Initialize()
    {
        // do nothing
    }

    /// <summary>
    /// send any targets needed for the transition
    /// </summary>
    /// <returns>targets</returns>
    virtual public List<GameObject> ReturnableObjects()
    {
        return new List<GameObject>();
    }

    /// <summary>
    /// send any tags needed for the transition
    /// </summary>
    /// <returns>target tags</returns>
    virtual public List<string> ReturnableTags()
    {
        return new List<string>();
    }


    /// <summary>
    /// allow decision to access brain's knowledge
    /// </summary>
    protected AIBrain myBrain;
    public void SetBrain(AIBrain brain) => myBrain = brain;
}
