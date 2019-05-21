using System.Collections;
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
    TileNode targetTile;

		    bool runningCoRoutine = false;
            bool creatureMoving = false;
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
    public void Melee(GameObject target)
    {
        #region Get Nearest + Null Check
        var weakest = Checks.WeakestCreature();
        Vector2 pos;
        if (target != null)
        {
            pos = target.transform.position;
        }
        else if (weakest != null)
        {
            pos = Checks.WeakestCreature().transform.position;
        }
        else
        {
            return;
        }
        #endregion

        if (!CheckThreshold(pos, AiData.EngageHostileThreshold))
        {
            if (Behaviour.MoveTo(pos, AiData.Speed, 1.0f))
            {
                Behaviour.MeleeAttack(pos, AiData.Speed);
            }
        }
        else
        {
            Behaviour.MeleeAttack(pos, AiData.Speed);
        }
    }
    public void NavMelee(GameObject target)
    {
        #region Get Nearest + Null Check
        var weakest = Checks.ClosestCreature();
        Vector2 pos;
        if (target != null)
        {
            pos = target.transform.position;
        }
        else
        {
            pos = Checks.WeakestCreature().transform.position;
            if (pos == null)
                pos = this.transform.position;
        }
        #endregion
        TileNode tile = Checks.GetNearestNode(pos);
        if (!CheckThreshold(pos, AiData.EngageHostileThreshold))
        {
            if (NavTo(tile))
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
        var nearestEnemy = AiData.Checks.WeakestCreature();
        Vector2 pos;
        if (nearestEnemy != null)
        {
            pos = nearestEnemy.gameObject.transform.position;
            //Debug.Log(this.gameObject + " found nearest enemy at " + pos);
            //Debug.Log("My position: " + transform.position);
        }
        else
        {
            pos = transform.position;
            return;
        }
        #endregion
        var rat = AiData.RangeAttackThreshold;
        var eht = AiData.EngageHostileThreshold;
        var engage = CheckRangedThreshold(pos, rat, eht);
        if (engage < 0)
        {
            if (Behaviour.MoveFrom(pos, AiData.Speed, rat - eht))
            {
                Behaviour.RangedAttack(pos, AiData.Speed);
            }
        }
        else if (engage > 0)
        {
            if (Behaviour.MoveTo(pos, AiData.Speed, rat + eht))
            {
                Behaviour.RangedAttack(pos, AiData.Speed);
            }
        }
        else Behaviour.RangedAttack(pos, AiData.Speed);
    }
    public void NavRanged()
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
        var targetPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        var newPos = Vector3.ClampMagnitude(pos - targetPos, AiData.RangeAttackThreshold);
        TileNode tile = Checks.GetNearestNode(pos);
        if (CheckThreshold(pos, AiData.EngageHostileThreshold))
        {
            if (NavTo(tile))
            {
                Behaviour.RangedAttack(pos, AiData.Speed);
            }
        }
    }

    //end of aggro region
    #endregion

    #region Neutral Protocols
    // Lazy()
    // just chill
    /// <summary>
    /// If idle, update awareness and reset timers
    /// </summary>
    public void Lazy()
    {
        #region Get Nearest + Null Checks
        // For now, fun away from your first enemy (SOMA most likely)
        // Vector2 pos = new Vector2(-9.5f, -3.5f);
        /*GameObject creature = Checks.ClosestCreature();
        Vector2 pos;
        if (creature != null)
            pos = creature.gameObject.transform.position;
        else
            return;
         */
        //pos = new Vector2(-2.5f, -2.5f);
        /*TileNode realPos = Checks.GetNearestNode(pos);
        Debug.Log("The Tile Node " + realPos.name + " -- found: " + realPos.transform + " (" + realPos.x + ", " + realPos.y + ")");
        NavTo(realPos);*/
        #endregion
        if (Behaviour.Idle())
        {
            Wander(2f, 2f);
        }
    }

    // Runaway()
    // move away from the nearest anything
    public void Runaway()
    {
        NavRunaway();
    }
    public void NavRunaway()
    {
        //Debug.Log("navrunningaway");
        float maxDist = 0;
        //Debug.Log("Navigating runaway");
        Checks.SetCurrentTile();
        
        if (Checks.currentTile != null)
        {

            //Debug.Log(Checks.currentTile.name);
            if (Checks.NearestEnemyPosition() == Vector2.zero) return;
            //Debug.Log("Nearestenemy exists");
            foreach (var neighbor in Checks.currentTile.neighbors)
            {
                //Debug.Log("Tiles have neighbors");
                var distance = Vector2.Distance(Checks.ClosestCreature().transform.position, neighbor.transform.position);
                if(distance > maxDist)
                {
                    maxDist = distance;
                    targetTile = neighbor;
                }
            }
            if (targetTile != null)
            {
                //Debug.Log("Targettile is not null, tile id: " +targetTile.name);
                if (Vector2.Distance(transform.position, Checks.NearestEnemyPosition()) <= AiData.EngageHostileThreshold)
                {
                    Behaviour.MoveTo(targetTile.transform.position, AiData.Speed, 1f);
                }
            }
        }

        #endregion
    }

    // Chase()
    // move towards the nearest anything
    public void Chase()
    {
        #region Get Nearest + Null Checks
        Vector2 pos;
        GameObject creature = Checks.ClosestCreature();
        if (creature != null)
            pos = creature.transform.position;
        else
            pos = transform.position;
        #endregion
        if (Behaviour.MoveTo(pos, AiData.Speed, 1.0f))
        {
            Wander(2f, 2f);
        }
    }
    public void Chase(GameObject ch)
    {
        #region Get Nearest + Null Checks
        Vector2 pos;
        GameObject creature = ch;
        if (creature != null)
            pos = creature.transform.position;
        else
            pos = transform.position;
        #endregion
        if (Behaviour.MoveTo(pos, AiData.Speed, 1.0f))
        {
            Wander(2f, 2f);
        }
    }
    public void NavChase()
    {
        #region Get Nearest + Null Checks
        // For now, fun away from your first enemy (SOMA most likely)
        Vector2 pos;
        GameObject creature = Checks.ClosestCreature();
        if (creature != null)
        {
            pos = creature.transform.position;
            TileNode tile = Checks.GetNearestNode(pos);
            NavTo(tile);
        }
        #endregion
    }



    // Wander()
    // go in random directions
    public void Wander(float rx, float ry)
    {
        // pick a location near me
        // move towards it
        #region Get Nearest + Null Checks
        Checks.SetRandomPosition(rx, ry);
        Vector2 pos = Checks.GetSpecialPosition();
        #endregion
        if (Behaviour.MoveTo(pos, AiData.Speed, 1.0f))
        {
            Checks.ResetSpecials();
            Behaviour.ResetActionTimer();
        }
    }
    public void NavWander(float rx, float ry)
    {
        // pick a location near me
        // move towards it
        #region Get Nearest + Null Checks
        // For now, fun away from your first enemy (SOMA most likely)
        Checks.SetRandomPosition(rx, ry);//Checks.GetRandomPositionType();
        Vector2 pos = Checks.GetSpecialPosition();
        #endregion
        TileNode tile = Checks.GetNearestNode(pos);
        if (NavTo(tile))
        {
            Checks.ResetSpecials();
            Behaviour.ResetActionTimer();
        }
    }

    // end of neutral region
    //#endregion

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
                Wander(10f, 10f);
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
        GameObject cHittable;
        #endregion
        if (cDrop != null)
        {
            // go to the nearest drop
            if (Behaviour.MoveTo(cDrop.transform.position, AiData.Speed, 1f))
            {
                Behaviour.Feed(cDrop);
            }
        }
        else
        {
            // go to nearest thing that drops drops
            if (this.tag == "Prey")
            {
                cHittable = Checks.ClosestPlant();
                //Debug.Log("closest plant is " + cHittable.name);
            }
            else
            {
                cHittable = Checks.ClosestCreature();
            }

            // go to the nearest drop
            if (cHittable != null)
                Melee(cHittable);
            else
                Wander(10f, 10f);
        }
    }
    public void NavFeast()
    {
        #region Surroundings
        GameObject cDrop = Checks.ClosestDrop();
        GameObject cHittable;
        #endregion
        if (cDrop != null)
        {
            // go to the nearest drop
            if (Behaviour.MoveTo(cDrop.transform.position, AiData.Speed, 1f))
            {
                Behaviour.Feed(cDrop);
            }
        }
        else
        {
            // go to nearest thing that drops drops
            if (this.tag == "Prey")
            {
                cHittable = Checks.ClosestPlant();
                //Debug.Log("closest plant is " + cHittable.name);
            }
            else
            {
                cHittable = Checks.ClosestCreature();
            }

            // go to the nearest drop
            if (cHittable != null)
                Melee(cHittable);
            else
                Wander(10f, 10f);
        }
    }


    // Conga()
    // become the leader if no leader
    // otherwise, follow the last person in line
    public void Conga()
    {
        if (Checks.AmLeader())
        {
            // YOURE THE LEADER!!
        	// GO GO GO
            Wander(2f, 2f);

        }
        else if (Checks.specialLeader != null)
        {
            // FOLLOW THE LEADER!!
            // LEFT RIGHT LEFT
            GameObject drone = Checks.ClosestDrone();
            if (drone == null)
            {
                GameObject near = Checks.FollowTheLeader();
                Vector2 pos = near.transform.position;
                Behaviour.MoveTo(pos, AiData.Speed + 3, 1.5f);
            }
            else
            {
                Melee(drone);
            }
        }
        else if (Checks.specialTarget == null)
        {
        	// still gotta conga!!
        	// even with no leader
            //GameObject near = Checks.ClosestLeader();
            //Chase(near);
        }
        else
        {
        	// decide leader if no one around
        	if (!runningCoRoutine) { StartCoroutine(DecideLeader()); }
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
        #region Get Nearest + Null Checks
        Vector2 pos = Checks.AverageGroupPosition();
        #endregion
        if (Behaviour.MoveTo(pos, AiData.Speed, 1.0f))
        {
            if (Behaviour.Console())
            {
                Wander(2f, 2f);
            }
        }
    }

    // end of pacifict region
    #endregion

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


    // checks if their are enemies, then attempts to attack
    // if attack cannot happen, becomes lazy
    public void Guard()
    {

    }
    // in order to work with vector2, we must take in a vector2/vector3 and determine what tile overlaps at that position(if it overlaps at all)
    // in order to work based on the tilemap, we can access the neighbor tiles and determine the one that is furthest away/closest to the target then select that tile
    // in order to work based on gameobjects, all gameobjects must track their currenttile or be able to calculate their current tile based on collision
    public bool NavTo(TileNode target)
    {

        if(Checks.currentTile == target)
        {
            Debug.Log("AT TARGET");
            return true;
        }
        if(Checks.currentTile == null || target == null)
        {
            Debug.Log("EITHER AGENT OR TARGET IS NOT ON TILEMAP");
            return false;
        }
        if(AiData.path == null || AiData.path.Count == 0)
        {
            Debug.Log("Path being drawn from target to destination");
            Checks.SetCurrentTile();
            AiData.path = Behaviour.pathfinder.AStar(target);
        }
        if (AiData.path == null)
        {
            return false;
        }
        for(int i = AiData.path.Count-1; i > 0; i--)
        {
            if (Behaviour.MoveTo(AiData.path[i].transform.position, AiData.Speed, 1f))
            {
                Checks.currentTile = AiData.path[i];
                AiData.path.Remove(AiData.path[i]);
            }
            else
            {
                Behaviour.MoveTo(AiData.path[i].transform.position, AiData.Speed, 1f);
            }
        }


            /*
        // bool moving = false;
        // if agent is on tilemap
        var curTile = Checks.GetNearestNode((Vector2)transform.position);
        if(curTile == null)
        {
            return false;
        }
        if(curTile != target)
        {
            // Set the path based on AStar algorithm of the currentTile
            if (AiData.path == null)
            {
                AiData.path = Behaviour.pathfinder.AStar(Checks.currentTile, target);
            }
            /// if path is empty, fill it based on destination
            if (AiData.path.Count >= 1)
            {
                if (Behaviour.MoveTo(AiData.path[AiData.path.Count - 1].transform.position, AiData.Speed, AiData.MeleeAttackThreshold))
                {
                    AiData.path.Remove(AiData.path[AiData.path.Count - 1]);
                    Debug.Log("Navigated to next tile");
                }
                return false;
            }
            if(AiData.path.Count < 1)
            {
                AiData.path = null;
                return true;
            }
        }*/
        return false;
    }
    #endregion
}
