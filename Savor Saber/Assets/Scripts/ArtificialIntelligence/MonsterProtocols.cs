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
        var nearestEnemy = AiData.Checks.ClosestEnemyCreature().gameObject.transform.position;
        // Aware is a function that uses the Perception of the agent
        //  to get a list of targets

        //The way MoveTo is set up is that it assumes you're only calling it when you need to move
        //Both of these behaviors require the player position as a Vector2 stored somewhere in AIData.cs
        if (Vector2.Distance(nearestEnemy, AiData.gameObject.transform.position) > AiData.RangeAttackThreshold)
        {
            if (Behaviour.MoveTo(nearestEnemy, AiData.Speed))
            {
                Behaviour.Ranged(nearestEnemy, AiData.Speed);

            }
        }
    }



    public void Lazy()
    {
        if (Behaviour.Idle())
        {
            Behaviour.ActionTimer = Behaviour.ActionTimerReset;
        }
    }



    // checks if their are enemies, then attempts to attack
    // if attack cannot happen, becomes lazy
    public void Guard()
    {
        if (AiData.Checks.NumberOfEnemies() > 0)
        {
            if (!Behaviour.Attack(AiData.Checks.ClosestEnemyCreature().gameObject.transform.position, AiData.Speed))
            {
                Lazy();
            }
        }
    }


    // checks if there are enough friends to party
    // moves if necessary to the nearest friend
    // socializes
    public void Party()
    {
        var closestEnemy = AiData.Checks.ClosestEnemyCreature().gameObject.transform.position;


        if (AiData.Checks.NumberOfFriends() >= AiData.PartySize)
        {
            if (Behaviour.MoveTo(closestEnemy, AiData.Speed)) {
                Behaviour.Socialize(closestEnemy, AiData.Speed);
            }
        }
    }


    // calculates numFriends/numEnemies once for efficiency
    //
    // if there are a party of friends{
    //      if the ratio of friends to enemies is >= twice the number of enemies{
    //          if the agent is at closest enemy{
    //              if the creature is dead{
    //                  feed()
    public void Swarm()
    {
        var numFriends = AiData.Checks.NumberOfFriends();
        var numEnemies = AiData.Checks.NumberOfEnemies();
        var closestEnemy = AiData.Checks.ClosestEnemyCreature().gameObject.transform.position;


        if(numFriends >= AiData.PartySize)
        {
            if (numFriends / numEnemies >= 2 * numEnemies )
            {
                if(Behaviour.MoveTo(closestEnemy, AiData.Speed))
                {
                    //behavior.attack should return true if creature dies?
                    if(Behaviour.Attack(closestEnemy, AiData.Speed))
                    {
                        //need to look at behavior.feed
                        //Behaviour.Feed(closestEnemy, AiData.Speed);
                    }
                }
            }
        }
    }



    // plants currently not implemented
    public void Feast()
    {

    }



    // fear signal check needed
    public void Console()
    {
        if(AiData.Checks.NumberOfFriends() > 0)
        {
            //need a check for fear signals
            /*
            if (NearestFriend.isAfraid() ???)
            {
                Behaviour.Socialize(AiData.Checks.ClosestFriendlyCreature().gameObject.transform.position);
            }*/
            
        }
    }



    public void Runaway()
    {
        // returns a collider
        // why not just the game object?
        var nearestEnemy = AiData.Checks.ClosestEnemyCreature().gameObject.transform.position;
        Behaviour.MoveFrom(nearestEnemy, AiData.Speed);
        
        // for testing
        //Melee();
    }
}
