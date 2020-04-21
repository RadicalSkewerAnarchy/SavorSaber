using System.Collections.Generic;
using UnityEngine;

public class AIStateTargettable : AIState
{
    // targettables
    public List<string> TargetTags;
    protected List<GameObject> TargetObjects;
    protected GameObject Target;

    /// <summary>
    /// If no target is given, how does it obtain one?
    /// Does the action need one?
    /// </summary>
    virtual public void ChooseTarget()
    {
        // do nothing
    }
}
