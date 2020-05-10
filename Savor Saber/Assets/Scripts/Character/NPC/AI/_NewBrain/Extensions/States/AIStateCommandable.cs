using System.Collections.Generic;
using UnityEngine;

public class AIStateCommandable : AIStateTargettable
{
    [SerializeField]
    private List<AIData.Protocols> Verbs;
    [SerializeField]
    private List<AIState> Performances;

    private int commandOfChoice = -1;

    private void Start()
    {
        if (Verbs.Count != Performances.Count)
        {
            Debug.Log($"NUMBER OF COMMANDS != NUMBER OF STATES IN {this.myBrain.name}");
            Destroy(this.gameObject);
        }
    }

    public override void Perform()
    {
        if (commandOfChoice >= 0)
        {
            AIState state = Performances[commandOfChoice];
            if (state != null)
            {
                //Debug.Log($"commandable member is performing {state}");
                state.Perform();
                if (state is AIStateTargettable)
                {
                    (state as AIStateTargettable).SetTarget(Target);
                }
            }
        }
    }

    public override void OnEnter()
    {
        foreach (var state in Performances)
        {
            state.SetBrain(this.myBrain);
        }
    }

    public void SetCommand(AIData.Protocols verb)
    {
        if (Verbs.Contains(verb))
        {
            commandOfChoice = Verbs.IndexOf(verb);
        }
        else
        {
            commandOfChoice = -1;
        }
    }
}
