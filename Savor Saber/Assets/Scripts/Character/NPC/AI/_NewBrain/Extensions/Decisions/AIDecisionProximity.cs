using System.Collections.Generic;
using UnityEngine;

public class AIDecisionProximity : AIDecision
{
    [SerializeField]
    private GameObject Target;
    public List<string> TargetTags;
    [SerializeField]
    private bool Within = true;
    [SerializeField]
    private float Distance = 3;


    public override bool Evaluate()
    {
        if (Target != null)
        {
            if (Within)
            {
                return Vector2.Distance(myBrain.CharacterData.transform.position, Target.transform.position) < Distance;
            }
            else
            {
                return Vector2.Distance(myBrain.CharacterData.transform.position, Target.transform.position) > Distance;
            }
        }
        else
        {
            FindTarget();
            return false;
        }
    }

    public override void Initialize()
    {
        // testing
        FindTarget();
    }

    protected virtual void FindTarget()
    {
        List<GameObject> objs = new List<GameObject>();
        if (myBrain == null) return;
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
}
