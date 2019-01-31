using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// not sure if this should be a hard requirement or not but I'll leave it here for now
/// </summary>
[RequireComponent(typeof(AIData))]
public class MonsterProtocols : MonoBehaviour
{
    AIData AiData;
    MonsterBehavior Behaviour;

    private void Start()
    {
        AiData = GetComponent<AIData>();
        Behaviour = AiData.GetComponent<MonsterBehavior>();
    }

    /// <summary>
    /// Every Behavior that is part of the protocol
    /// chain returns a boolean, 
    /// thus, they may be chained to create an order of operations
    /// Each protocol is of the format:
    ///     void X()
    ///     {
    ///         if (Behavior()){
    ///             if (Behavior())
    ///             {
    ///                 ...
    ///             }
    ///         }
    ///     }
    /// </summary>

    /// NEEDS A LOT OF POLISH
    public void Melee()
    {
        // var distanceToPlayer = AiData.getNormalizedValue("PlayerDistance");
        // float distanceToPlayer = AiData.Aware("PlayerDistance");
        var nearestEnemy = AiData.Checks.ClosestEnemyCreature().gameObject.transform.position;
        // Aware is a function that uses the Perception of the agent
        //  to get a list of targets

        //The way MoveTo is set up is that it assumes you're only calling it when you need to move
        //Both of these behaviors require the player position as a Vector2 stored somewhere in AIData.cs
        if (Vector2.Distance(nearestEnemy, AiData.gameObject.transform.position) > AiData.MeleeAttackThreshold)
        {
            if (Behaviour.MoveTo(nearestEnemy, AiData.Speed))
            {
                Behaviour.Attack(nearestEnemy, AiData.Speed);

            }
        }

        // Need the actual coordinates of Player and findobject() is computationally expensive, need workaround in AiData to have this Vector2           
    }

    public void Ranged()
    {
        var distanceToPlayer = AiData.getNormalizedValue("PlayerDistance");

        //The way MoveTo is set up is that it assumes you're only calling it when you need to move
        //Both of these behaviors require the player position as a Vector2 stored somewhere in AIData.cs
        if (distanceToPlayer > AiData.RangeAttackThreshold)
        {
            //Behaviour.MoveTo();
        }
        else
        {
            //Behaviour.Attack();
        }
    }
    public void Lazy()
    {
        if (Behaviour.Idle())
        {
            Behaviour.ActionTimer = Behaviour.ActionTimerReset;
        }
    }
    public void Guard()
    {

    }
    public void Party()
    {

    }
    public void Swarm()
    {

    }
    public void Feast()
    {

    }
    public void Console()
    {

    }
    public void Runaway()
    {
        // returns a collider
        // why not just the game object?
        var nearestEnemy = AiData.Checks.ClosestEnemyCreature().gameObject.transform.position;
        Behaviour.MoveFrom(nearestEnemy, AiData.Speed);
    }
}
