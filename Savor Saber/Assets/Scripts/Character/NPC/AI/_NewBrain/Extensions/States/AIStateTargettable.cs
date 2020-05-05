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
        List<GameObject> objs = new List<GameObject>();
        // find tags
        foreach (var target in myBrain.ObjectsInPerception)
        {
            if (target == null)
            {
                continue;
            }

            if (TargetTags.Contains(target.tag))
            {
                objs.Add(target);
            }
        }

        if (Target == null || !objs.Contains(Target))
        {
            SetRandomTarget(objs);
        }
        else
        {
            Target = null;
        }
    }

    protected void SetRandomTarget(List<GameObject> objs)
    {
        if (objs.Count > 0)
        {
            Target = objs[Random.Range(0, objs.Count)];
        }
        else
        {
            Target = null;
        }
    }

    public void SetTarget(GameObject obj)
    {
        Target = obj;
    }
}
