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
    public GameObject target;
    public Vector2 targetPos = new Vector2( 0, 0 );
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
                _behave.MoveTo(targetPos, speed);
                break;
            case Behavior.MoveAwayFrom:
                _behave.MoveFrom(targetPos, speed);
                break;
            case Behavior.Feed:
                _behave.Feed(target);
                break;
            case Behavior.Attack:
                _behave.Attack(targetPos, speed);
                break;
            case Behavior.Ranged:
                _behave.Ranged(targetPos, speed);
                break;
            case Behavior.Socialize:
                _behave.Socialize();
                break;
            case Behavior.Custom:
                custom.InvokeBehavior();
                break;
        }

    }
}
