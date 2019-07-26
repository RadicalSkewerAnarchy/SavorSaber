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

        
        if (Behaviour.MoveTo(pos, AiData.Speed, 1.0f))
        {
            if (CheckThreshold(pos, AiData.MeleeAttackThreshold))
            {
                Behaviour.MeleeAttack(pos);
            }
        }
    }

    public void NavMelee(GameObject target, float speed)
    {
        #region Get Nearest + Null Check
        GameObject weakest = target;
        Vector2 pos;
        if (weakest != null)
        {
            pos = weakest.transform.position;
        }
        else
        {
            weakest = Checks.ClosestCreature(new string[] { (this.tag == "Prey" ? "Prey" : "Predator") });
            if (weakest != null)
                pos = weakest.transform.position;
            else
                return;
        }
        #endregion
        if (NavChase(weakest, speed, AiData.MeleeAttackThreshold) || Vector2.Distance(weakest.transform.position, this.transform.position) <= AiData.EngageHostileThreshold)
        {
            Behaviour.MeleeAttack(pos);
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
        var nearestEnemy = (this.tag=="Prey" ? Checks.ClosestDrone() : Checks.WeakestCreature());
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

    public void Ranged(GameObject target)
    {
        #region Get Nearest + Null Check
        Vector2 pos;
        if (target != null)
        {
            pos = target.transform.position;
        }
        else
        {
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
            if (NavTo(tile, AiData.Speed))
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
    public void NavRunaway(GameObject target=null)
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
            if(!creatureMoving)
            {
                Checks.closestEnemy = (target==null? Checks.ClosestCreature() : target);
                StartCoroutine(CreatureFearDegrading());
            }
            foreach (var neighbor in Checks.currentTile.neighbors)
            {
                //Debug.Log("Tiles have neighbors");
                if (Checks.closestEnemy == null)
                    return;
                var distance = Vector2.Distance(Checks.closestEnemy.transform.position, neighbor.transform.position);
                if(distance > maxDist)
                {
                    maxDist = distance;
                    targetTile = neighbor;
                }
            }
            if (targetTile != null)
            {
                //Debug.Log("Targettile is not null, tile id: " +targetTile.name);
                //if (Vector2.Distance(transform.position, Checks.NearestEnemyPosition()) <= AiData.EngageHostileThreshold)
                if (Vector2.Distance(transform.position, Checks.closestEnemy.transform.position) <= AiData.Perception)
                {
                    Behaviour.MoveTo(targetTile.transform.position, AiData.Speed, 1.5f);
                }
            }
            //StartCoroutine(CreatureFearDegrading());
        }
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
        Behaviour.MoveTo(pos, AiData.Speed, 0.5f);
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
    // used for ride protocol
    public void Ride(Vector3 go)
    {
        float chargeSpeed = 5;
        if (InputManager.GetButton(Control.Knife))
        {
            GameObject drone = Checks.ClosestDrone();
            if (drone != null)
            {
                NavMelee(drone, chargeSpeed);
            }
            else
                Chase(go);
        }
        else if (InputManager.GetButton(Control.Skewer))
        {
            Feast(true);
        }
        else
            Chase(go);
    }
    public void Chase(Vector3 go)
    {
        Behaviour.MoveTo(this.transform.position + go * 10, 4 + AiData.Speed * AiData.Speed, 0.1f);
    }



    public bool NavChase()
    {
        #region Get Nearest + Null Checks
        // For now, fun away from your first enemy (SOMA most likely)
        Checks.SetCurrentTile();
        Vector2 pos;
        GameObject creature = Checks.ClosestCreature();
        if (creature != null)
        {
            pos = creature.transform.position;
            TileNode tile = Checks.GetNearestNode(pos);
            //Debug.Log(this.name + " naving to: " + tile.name);
            return NavTo(tile, AiData.Speed);
        }
        return false;
        #endregion
    }
    public bool NavChase(GameObject target, float speed=1, float thresh=1)
    {
        #region Get Nearest + Null Checks
        // For now, fun away from your first enemy (SOMA most likely)
        Checks.SetCurrentTile();
        Vector2 pos;
        if (target != null)
        {
            pos = target.transform.position;
            TileNode tile = Checks.GetNearestNode(pos);
            return NavTo(tile, speed, thresh);
        }
        return false;
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
        if (NavTo(tile, AiData.Speed))
        {
            Checks.ResetSpecials();
            Behaviour.ResetActionTimer();
        }
    }

    // end of neutral region
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
    public void Feast(bool melee)
    {
        #region Surroundings
        GameObject cDrop = Checks.ClosestDrop();
        GameObject cHittable;
        #endregion
        if (cDrop != null && this.tag != "Predator")
        {
            // go to the nearest drop
            if (Behaviour.MoveTo(cDrop.transform.position, AiData.Speed, 1f))
            {
                FuitantMount flavorResponse = this.GetComponentInChildren<FuitantMount>();
                bool response = (flavorResponse == null ? false : flavorResponse.mounted);
                Behaviour.Feed(cDrop, response);
            }
        }
        else
        {
            // go to nearest thing that drops drops
            if (this.tag == "Prey")
            {
                cHittable = Checks.ClosestPlant();
            }
            else
            {
                cHittable = Checks.ClosestCreature(new string[] { "Predator" });
                if (cHittable == null)
                    cHittable = Checks.ClosestPlant();
            }

            // go to the nearest drop
            if (cHittable != null)
            {
                if (melee)
                    Melee(cHittable);
                else
                    Ranged(cHittable);
            }
            else
                Wander(5f, 5f);
        }
    }


    // Conga()
    // become the leader if no leader
    // otherwise, follow the last person in line
    public void Conga()
    {
        float congaSpeed = 4;
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
                NavChase(near, congaSpeed, 2f);
            }
            else
            {
                //Behaviour.MoveTo(drone.transform.position, 3f, 1f);
                NavMelee(drone, congaSpeed);
            }
        }
        else if (Checks.specialTarget == null)
        {
        	// still gotta conga!!
        	// even with no leader
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
    public void Scare()
    {
        #region Get Nearest + Null Checks
        Vector2 pos = Checks.AverageGroupPosition();
        #endregion
        if (Behaviour.MoveTo(pos, AiData.Speed, 1.0f))
        {
            if (Behaviour.Scare())
            {
                Wander(2f, 2f);
            }
        }
    }

    // end of pacifict region
    #endregion

    #region Tile Node Navigation
    // in order to work with vector2, we must take in a vector2/vector3 and determine what tile overlaps at that position(if it overlaps at all)
    // in order to work based on the tilemap, we can access the neighbor tiles and determine the one that is furthest away/closest to the target then select that tile
    // in order to work based on gameobjects, all gameobjects must track their currenttile or be able to calculate their current tile based on collision
    public bool NavTo(TileNode target, float speed, float thresh = 1)
    {
        if (Checks.currentTile == target)
        {
            //Debug.Log("AT TARGET");
            return true;
        }
        else if (Checks.currentTile == null || target == null)
        {
            //Debug.Log("EITHER AGENT OR TARGET IS NOT ON TILEMAP");
            return false;
        }
        else if (AiData.path == null || AiData.path.Count == 0)
        {
            Checks.SetCurrentTile();
            //Debug.Log(this.name + " pathing from: " + Checks.currentTile.name + " to: " + target.name);
            AiData.path = Behaviour.pathfinder.AStar(target);
            return false;
        }

        // move there
        int i = AiData.path.Count - 1;
        if (Behaviour.MoveTo(AiData.path[i].transform.position, speed, thresh))
        {
            Checks.currentTile = AiData.path[i];
            AiData.path.Remove(AiData.path[i]);
        }
        else
        {
            Behaviour.MoveTo(AiData.path[i].transform.position, speed, thresh);
        }

        return false;
    }
    #endregion
}
