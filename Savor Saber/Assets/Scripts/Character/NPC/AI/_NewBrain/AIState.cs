using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState : MonoBehaviour
{
    /// <summary>
    /// Called by the brain to do actions upon targets
    /// </summary>
    abstract public void Perform();

    /// <summary>
    /// If no target is given, how does it obtain one?
    /// Does the action need one?
    /// </summary>
    abstract public void ObtainTarget();

    /// <summary>
    /// all the possible transitions in this one state
    /// </summary>
    public List<AITransition> Transitions;

    // targettables
    public List<GameObject> TargetObjects;
    public List<string> TargetTags;
}
