using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIData))]
public class MonsterProtocols : MonoBehaviour
{
    #region Initialize
    AIData AiData;
    MonsterBehavior Behaviour;
    MonsterChecks Checks;
    bool runningCoRoutine = false;
    #endregion

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

    #region Aggro Protocols
    /// NEEDS A LOT OF POLISH
    // Melee()
    // move to and make a melee attack on
    // some creature or enemy or bush
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
        // Aware is a function that uses the Perception of the agent
        //to get a list of targets
        //The way MoveTo is set up is that it assumes you're only calling it when you need to move
        //Both of these behaviors require the player position as a Vector2 stored somewhere in AIData.cs
        if (Vector2.Distance(pos, AiData.gameObject.transform.position) > AiData.MeleeAttackThreshold)
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
        //Behaviour.RangedAttack(pos, AiData.Speed);

        if (Vector2.Distance(pos, AiData.gameObject.transform.position) <= AiData.RangeAttackThreshold)
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
    #endregion

    #region Neutral Protocols
    // Lazy()
    // just chill
    public void Lazy()
    {             
        // idle
        if (Behaviour.Idle())
        {
            // test signals
            Checks.AwareHowMany();
            // reset action timer
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
        Vector2 pos = Checks.ClosestCreature().transform.position;//Checks.GetRandomPositionType();
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

    // plants currently not implemented
    // Feast()
    // find plants, hit plants, eat plant drops
    public void Feast()
    {
        
        Checks.AwareNearby();

        GameObject cDrop = Checks.ClosestDrop();
        //GameObject cBody = Checks.ClosestCreature();
        //Debug.Log("cDrop instanceID: " + cDrop.GetInstanceID());
        // if there are drops
        if (this.tag == "Prey")
        {
            // move to and feed
            if(cDrop != null)
            {
                if (Behaviour.MoveTo(cDrop.transform.position, AiData.Speed, AiData.MeleeAttackThreshold))
                {
                    Behaviour.Feed(cDrop);
                }
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

    // Console()
    // go to a friend in need
    // increase their friendliness and
    // decrease their fear and hostility
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

    // Wander()
    // go in random directions
    public void Wander()
    {

    }
    #endregion

    #region Helper Functions
    // DecideLeader()
    // used to randomly start and end
    // the decision making process for who
    // is 1st leader
    protected IEnumerator DecideLeader()
    {
        runningCoRoutine = true;
        //Debug.Log("In coroutine and running...");
        yield return new WaitForSeconds(Random.Range(0.01f, 0.05f));
        //sDebug.Log("... in coroutine and Done");
        Checks.BecomeLeader();
        yield return null;
        runningCoRoutine = false;
    }
    #endregion
}

