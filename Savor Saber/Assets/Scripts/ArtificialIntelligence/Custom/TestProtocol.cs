using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterBehavior))]
public class TestProtocol : CustomProtocol
{
    /// <summary>
    /// Which test behavior to call
    /// </summary>
    public enum Behavior
    {
        Idle,
        MoveTo,
        MoveAwayFrom,
        Feed,
        Attack,
        Ranged,
        Socialize,
        Custom,
    }

    public float speed = 0;
    public Vector2 target = new Vector2( 0, 0 );
    public Behavior curr;
    public CustomBehavior custom;
    MonsterBehavior _behave;
    private void Start()
    {
        _behave = GetComponent<MonsterBehavior>();
    }

    public override void Invoke()
    {
        switch (curr)
        {
            case Behavior.Idle:
                _behave.Idle();
                break;
            case Behavior.MoveTo:
                _behave.MoveTo(target, speed);
                break;
            case Behavior.MoveAwayFrom:
                _behave.MoveFrom(target, speed);
                break;
            case Behavior.Feed:
                _behave.Feed();
                break;
            case Behavior.Attack:
                _behave.Attack(target, speed);
                break;
            case Behavior.Ranged:
                _behave.Ranged(target, speed);
                break;
            case Behavior.Socialize:
                _behave.Socialize(target, speed);
                break;
            case Behavior.Custom:
                custom.InvokeBehavior();
                break;
        }

    }
}
