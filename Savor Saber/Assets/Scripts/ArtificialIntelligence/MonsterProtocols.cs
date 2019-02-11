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
    MonsterChecks Checks;

    private void Start()
    {
        AiData = GetComponent<AIData>();
        Behaviour = AiData.GetComponent<MonsterBehavior>();
        Checks = AiData.GetComponent<MonsterChecks>();
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
        #region Get Nearest + Null Check
        var nearestEnemy = AiData.Checks.ClosestCreature();
        Vector2 pos;
        if (nearestEnemy != null)
        {
            pos = nearestEnemy.gameObject.transform.position;
        }
        else
        {
            pos = transform.position;
        }
        #endregion
        // Aware is a function that uses the Perception of the agent
        //to get a list of targets
        //The way MoveTo is set up is that it assumes you're only calling it when you need to move
        //Both of these behaviors require the player position as a Vector2 stored somewhere in AIData.cs
        if (Vector2.Distance(pos, AiData.gameObject.transform.position) > AiData.MeleeAttackThreshold)
        {
            if (Behaviour.MoveTo(pos, AiData.Speed))
            {
                Behaviour.Attack(pos, AiData.Speed);
            }
        }
        else
        {
            Behaviour.Attack(pos, AiData.Speed);
        }       
    }



    public void Ranged()
    {
        #region Get Nearest + Null Check
        var nearestEnemy = AiData.Checks.ClosestCreature();
        Vector2 pos;
        if (nearestEnemy != null)
        {
            pos = nearestEnemy.gameObject.transform.position;
        }
        else
        {
            pos = transform.position;
        }
        #endregion
        if (Vector2.Distance(pos, AiData.gameObject.transform.position) > AiData.RangeAttackThreshold)
        {
            if (Behaviour.MoveTo(pos, AiData.Speed))
            {
                Behaviour.Ranged(pos, AiData.Speed);
            }
        }
    }



    public void Lazy()
    {             
        // idle
        if (Behaviour.Idle())
        {
            // test signals
            Checks.AwareHowMany();
            // reset action timer
            Behaviour.ResetActionTimer();
        }
    }



    // checks if their are enemies, then attempts to attack
    // if attack cannot happen, becomes lazy
    public void Guard()
    {
        if (AiData.Checks.NumberOfEnemies() > 0)
        {
            if (!Behaviour.Attack(AiData.Checks.ClosestCreature().gameObject.transform.position, AiData.Speed))
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
        #region Get Nearest + Null Check
        var nearestEnemy = AiData.Checks.ClosestCreature();
        Vector2 pos;
        if (nearestEnemy != null)
        {
            pos = nearestEnemy.gameObject.transform.position;
        }
        else
        {
            pos = transform.position;
        }
        #endregion
        // move to
        if (Behaviour.MoveTo(pos, AiData.Speed))
        {
            // socialize
            if (Behaviour.Socialize(pos, AiData.Speed))
            {
                // reset action timer
                Behaviour.ResetActionTimer();
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
        #region Get Nearest + Null Check
        var nearestEnemy = AiData.Checks.ClosestCreature();
        Vector2 pos;
        if (nearestEnemy != null)
        {
            pos = nearestEnemy.gameObject.transform.position;
        }
        else
        {
            pos = transform.position;
        }
        #endregion


        if (numFriends >= AiData.PartySize)
        {
            if (numFriends / numEnemies >= 2 * numEnemies )
            {
                if(Behaviour.MoveTo(pos, AiData.Speed))
                {
                    //behavior.attack should return true if creature dies?
                    if(Behaviour.Attack(pos, AiData.Speed))
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
        GameObject cDrop = Checks.ClosestDrop();
        // if there are drops
        if (this.tag == "Prey")
        {
            // move to and feed
            if (Behaviour.MoveTo(cDrop.transform.position, AiData.Speed))
            {
                Behaviour.Feed(cDrop);
            }
     
        }
        // else if i am a predator
        else if (this.tag == "Predator")
        {
            // move to 
            // attack
            Melee();
        }
    }



    // fear signal check needed
    public void Console()
    {
        if(AiData.Checks.NumberOfFriends() > 0)
        {
            Behaviour.Socialize();            
        }
    }

    public void Runaway()
    {
        #region Get Nearest + Null Check
        var nearestEnemy = AiData.Checks.ClosestCreature();
        Vector2 pos;
        if (nearestEnemy != null)
        {
            pos = nearestEnemy.gameObject.transform.position;
        }
        else
        {
            pos = transform.position;
        }
        #endregion
        Behaviour.MoveFrom(pos, AiData.Speed);
    }
}
