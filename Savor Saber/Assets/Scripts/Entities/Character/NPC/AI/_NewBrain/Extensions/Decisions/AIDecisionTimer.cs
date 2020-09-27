using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDecisionTimer : AIDecision
{
    [SerializeField]
    private float WaitTime = 3;
    [SerializeField]
    private float Timer = 0;

    public override bool Evaluate()
    {
        TimeTick();
        return Timer >= WaitTime;
    }

    public override void Initialize()
    {
        Timer = 0;
    }

    private void TimeTick()
    {
        if (Timer < WaitTime)
        {
            Timer += Time.deltaTime;
        }
    }
}
