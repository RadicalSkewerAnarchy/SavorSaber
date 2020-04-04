using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIState: MonoBehaviour
{
    /// <summary>
    /// all the possible transitions in this one state
    /// </summary>
    [SerializeField]
    public List<AITransition> Transitions;

    /// <summary>
    /// Called by the brain to do actions upon targets
    /// </summary>
    virtual public void Perform()
    {
        // do nothing
    }

    /// <summary>
    /// If no target is given, how does it obtain one?
    /// Does the action need one?
    /// </summary>
    virtual public void ChooseTarget()
    {
        // do nothing
    }

    // targettables
    private List<GameObject> TargetObjects;
    private List<string> TargetTags;

    public void SetTargets(List<GameObject> objs, List<string> strs)
    {
        TargetObjects = objs;
        TargetTags = strs;
    }

    /// <summary>
    /// called when the state is entered
    /// </summary>
    virtual public void OnEnter()
    {
        // do nothing
    }
    /// <summary>
    /// called when the state is exited
    /// </summary>
    virtual public void OnExit()
    {
        // do nothing
    }

    /// <summary>
    /// allow state to access brain's knowledge
    /// </summary>
    protected AIBrain myBrain;
    public void SetBrain(AIBrain brain)
    {
        myBrain = brain;
        foreach (var transition in Transitions)
        {
            transition.SetBrain(brain);
        }
    }
}
