using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDecisionProximity : AIDecision
{
    [SerializeField]
    private GameObject Target;
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
                return Vector2.Distance(myBrain.transform.position, Target.transform.position) < Distance;
            }
            else
            {
                return Vector2.Distance(myBrain.transform.position, Target.transform.position) > Distance;
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

    protected void FindTarget()
    {
        Target = null;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (myBrain.IsAwareOf(player))
        {
            Target = player;
        }

    }
}
