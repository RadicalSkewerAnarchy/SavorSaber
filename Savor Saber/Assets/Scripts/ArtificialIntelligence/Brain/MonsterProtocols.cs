﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIData))]
public partial class MonsterProtocols : MonoBehaviour
{
    #region Initialize
	    #region Global Variables
		    #region Components
		    AIData AiData;
		    MonsterBehavior Behaviour;
		    MonsterChecks Checks;
		    #endregion

		    #region Booleans
		    bool runningCoRoutine = false;
		    #endregion
	    #endregion
	#endregion

    private void Start()
    {
        #region Component Initialization
        AiData = GetComponent<AIData>();
        Behaviour = AiData.GetComponent<MonsterBehavior>();
        Checks = AiData.GetComponent<MonsterChecks>();
        #endregion
    }

    /// <summary>
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
    #region Aggro Protocols
    /// NEEDS A LOT OF POLISH
    // Melee()
    // move to and make a melee attack on
    // some creature or enemy or bush
    /// <summary>
    /// If the target is outside of our range, move to it and attack. Else, attack.
    /// </summary>
    public void Melee()
    {
        #region Get Nearest + Null Check
        var weakest = Checks.WeakestCreature();
        Vector2 pos;
        if (weakest != null)
        {
            pos = weakest.transform.position;
        }
        else
        {
            pos = transform.position;
        }
        #endregion
        if (!CheckThreshold(pos, AiData.MeleeAttackThreshold))
        {
            if (Behaviour.MoveTo(pos, AiData.Speed, AiData.MeleeAttackThreshold))
            {
                Behaviour.MeleeAttack(pos, AiData.Speed);
            }
        }
        else
        {
            Behaviour.MeleeAttack(pos, AiData.Speed);
        }       
    }
    // Ranged()
    // move from a target and launch a projectile
    /// <summary>
    /// If the target is outside of our range, move to it and attack. Else, attack.
    /// </summary>
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
        if (CheckThreshold(pos, AiData.RangeAttackThreshold))
        {
            if (Behaviour.MoveFrom(pos, AiData.Speed, AiData.RangeAttackThreshold))
            {
                Behaviour.RangedAttack(pos, AiData.Speed);
            }
        }
    }

    // checks if their are enemies, then attempts to attack
    // if attack cannot happen, becomes lazy
    public void Guard()
    {
        if (AiData.Checks.NumberOfEnemies() > 0)
        {
            if (!Behaviour.MeleeAttack(AiData.Checks.ClosestCreature().gameObject.transform.position, AiData.Speed))
            {
                Lazy();
            }
        }
    }

    #endregion

    #region Neutral Protocols
    // Lazy()
    // just chill
    /// <summary>
    /// If idle, update awareness and reset timers
    /// </summary>
    public void Lazy()
    {             
        if (Behaviour.Idle())
        {
            Checks.AwareHowMany();
            Behaviour.ResetActionTimer();
            Checks.ResetSpecials();
        }
    }

    // Runaway()
    // move away from the nearest anything
    public void Runaway()
    {
        #region Get Nearest + Null Checks
        // For now, fun away from your first enemy (SOMA most likely)
        Vector2 pos = Checks.GetRandomPositionType();
        #endregion
        if (Behaviour.MoveFrom(pos, AiData.Speed, 10f))
        {
            Checks.ResetSpecials();
        }
    }

    // Chase()
    // move away from the nearest anything
    public void Chase()
    {
        #region Get Nearest + Null Checks
        // For now, fun away from your first enemy (SOMA most likely)
        Vector2 pos = Checks.ClosestCreature().transform.position;//Checks.GetRandomPositionType();
        #endregion
        if (Behaviour.MoveTo(pos, AiData.Speed, 10f))
        {
            Checks.ResetSpecials();
        }
    }
    #endregion

    #region Pacifist Protocols
    // checks if there are enough friends to party
    // moves if necessary to the nearest friend
    // socializes
    /// <summary>
    /// Checks if there are enough friends to party then socializes
    /// </summary>
    public void Party()
    {
        #region Get Nearest + Null Check
        Vector2 pos = Checks.AverageGroupPosition();
        #endregion
        // move to
        if (Behaviour.MoveTo(pos, AiData.Speed, AiData.MeleeAttackThreshold))
        {
            // socialize
            if (Behaviour.Socialize())
            {
                // reset action timer
                Behaviour.ResetActionTimer();
                Checks.ResetSpecials();
            }
        }       
    }

    // plants currently not implemented
    // Feast()
    // find plants, hit plants, eat plant drops
    /// <summary>
    /// Checks surroundings, if there is a drop move to it and eat it, if there aren't any drops attack nearest prey
    /// </summary>
    public void Feast()
    {
        #region Surroundings        
        GameObject cDrop = Checks.ClosestDrop();
        #endregion
        if (cDrop != null)
        {
            if(cDrop != null)
            {
                if (Behaviour.MoveTo(cDrop.transform.position, AiData.Speed, AiData.MeleeAttackThreshold))
                {
                    Behaviour.Feed(cDrop);
                }
            }          
        }
        else if (this.tag == "Predator")
        {
            Debug.Log("I AM A HUNGRE PADDLE PREDATOR");
            Melee();
        }
    }

    // Console()
    // go to a friend in need
    // increase their friendliness and
    // decrease their fear and hostility
    /// <summary>
    /// If there are friends, socialize and update special position
    /// </summary>
    public void Console()
    {
        if(AiData.Checks.NumberOfFriends() > 0)
        {
            if(Behaviour.Socialize())
            {
                Checks.ResetSpecials();
            }
        }
    }

    // Conga()
    // become the leader if no leader
    // otherwise, follow the last person in line
    public void Conga()
    {
        if (Checks.AmLeader())
        {
            GameObject near = Checks.ClosestCreature();
            Vector2 pos = (near == null ? (Vector2)transform.position : (Vector2)near.transform.position);
            //Vector2 pos = Checks.AverageGroupPosition();
            Behaviour.MoveFrom(pos, AiData.Speed / 1.5f, 1f);
        }
        else if (Checks.specialTarget == null)
        {
            if (!runningCoRoutine) { StartCoroutine(DecideLeader()); }
        }
        else
        {
            GameObject near = Checks.ClosestLeader();
            Vector2 pos = near.transform.position;
            Behaviour.MoveTo(pos, AiData.Speed, 1f);
        }
    }


    #region Unimplemented Protocols    
    // Swarm()
    // given enough friends
    // and few enemies
    // swarm a target and kill them
    // eat their remains and feast
    public void Swarm()
    {
        var numFriends = AiData.Checks.NumberOfFriends();
        var numEnemies = AiData.Checks.NumberOfEnemies();
        #region Get Nearest + Null Check
        Vector2 pos = Checks.GetRandomPositionType();
        #endregion
        if (numFriends >= AiData.PartySize)
        {
            if (numFriends / numEnemies >= 2 * numEnemies)
            {
                if (Behaviour.MoveTo(pos, AiData.Speed, AiData.MeleeAttackThreshold))
                {
                    //behavior.attack should return true if creature dies?
                    if (Behaviour.MeleeAttack(pos, AiData.Speed))
                    {
                        //need to look at behavior.feed
                        //Behaviour.Feed(closestEnemy, AiData.Speed);
                    }
                }
            }
        }
    }
    // Wander()
    // go in random directions
    public void Wander()
    {
    	
    }
    #endregion
}

