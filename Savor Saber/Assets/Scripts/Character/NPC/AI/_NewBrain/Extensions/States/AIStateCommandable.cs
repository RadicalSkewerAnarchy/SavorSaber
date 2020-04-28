using System.Collections.Generic;
using UnityEngine;

public class AIStateCommandable : AIState
{
    [SerializeField]
    private List<AIData.Protocols> Verbs;
    [SerializeField]
    private List<AIState> Performances;

    private int commandOfChoice = -1;


    public override void Perform()
    {
        if (commandOfChoice >= 0)
        {
            AIState state = Performances[commandOfChoice];
            if (state != null)
            {
                state.Perform();
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
