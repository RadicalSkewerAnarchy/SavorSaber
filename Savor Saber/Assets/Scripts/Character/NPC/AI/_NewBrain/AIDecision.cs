using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIDecision
{
    /// <summary>
    /// used to check if state should transition
    /// </summary>
    /// <returns>evaluation complete</returns>
    abstract public bool Evaluate();

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
}
