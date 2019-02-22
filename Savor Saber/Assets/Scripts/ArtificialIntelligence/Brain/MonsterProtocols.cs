using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIData))]
public partial class MonsterProtocols : MonoBehaviour
{
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
        if (CheckThreshold(pos, AiData.MeleeAttackThreshold))
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
    /// <summary>
    /// if no enemies to attack, stand still
    /// </summary>
    public void Guard()
    {
        if (EnemiesExist())
        {
            if (!Behaviour.MeleeAttack(AiData.Checks.ClosestCreature().gameObject.transform.position, AiData.Speed))
            {
                Lazy();
            }
        }
    }
    /// <summary>
    /// Checks if there are enough friends to party then socializes
    /// </summary>
    public void Party()
    {
        #region Get Nearest + Null Check
        Vector2 pos = Checks.GetRandomPositionType();
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
    /// <summary>
    /// Checks surroundings, if there is a drop move to it and eat it, if there aren't any drops attack nearest prey
    /// </summary>
    public void Feast()
    {
        #region Surroundings        
        Checks.AwareNearby();
        GameObject cDrop = Checks.ClosestDrop();
        #endregion
        if (this.tag == "Prey")
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
            Melee();
        }
    }
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
    /// <summary>
    /// if there are enemies, move away from them
    /// </summary>
    public void Runaway()
    {
        #region Get Nearest + Null Checks
        // For now, fun away from your first enemy (SOMA most likely)
        Vector2 pos = AiData.Enemies[0].transform.position;//Checks.GetRandomPositionType();
        #endregion
        if(Behaviour.MoveFrom(pos, AiData.Speed, 10f))
        {
            Checks.ResetSpecials();
        }
    }

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
    public void Swarm()
    {
        var numFriends = AiData.Checks.NumberOfFriends();
        var numEnemies = AiData.Checks.NumberOfEnemies();
        #region Get Nearest + Null Check
        Vector2 pos = Checks.GetRandomPositionType();
        #endregion


        if (numFriends >= AiData.PartySize)
        {
            if (numFriends / numEnemies >= 2 * numEnemies )
            {
                if(Behaviour.MoveTo(pos, AiData.Speed, AiData.MeleeAttackThreshold))
                {
                    //behavior.attack should return true if creature dies?
                    if(Behaviour.MeleeAttack(pos, AiData.Speed))
                    {
                        //need to look at behavior.feed
                        //Behaviour.Feed(closestEnemy, AiData.Speed);
                    }
                }
            }
        }
    }    
    public void Wander()
    {

    }
    #endregion
}

