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
    public Color pathfinderColor;
    public bool showPathfindingColor;
    GameObject player;
		#endregion
	#endregion

    private void Awake()
    {
        #region Component Initialization
        AiData = GetComponent<AIData>();
        Behaviour = AiData.GetComponent<MonsterBehavior>();
        Checks = AiData.GetComponent<MonsterChecks>();
        player = GameObject.FindGameObjectWithTag("Player");
        #endregion
    }

    #region Aggro Protocols
    /// NEEDS A LOT OF POLISH
    // Melee()
    /// <summary>
    /// If the target is outside of our range, move to it and attack. Else, attack.
    /// </summary>
    public void Melee(GameObject target)
    {
        //Debug.Log("Entering Melee Protocol. Target Null: " + (target == null));
        #region Get Nearest + Null Check
        var nearestEnemy = (target);
        Vector2 pos;
        if (target != null)
        {
            //Debug.Log(this.name + "'s target is " + target.name);
            pos = nearestEnemy.transform.position;
        }
        else
        {
            nearestEnemy = (this.tag == "Prey" ? Checks.ClosestDrone() : Checks.WeakestCreature());
            if (nearestEnemy != null)
            { 
                //Debug.Log(this.name + "'s target is " + nearestEnemy.name);
                pos = nearestEnemy.transform.position;
            }
            else
            {
                //Debug.Log("for some reason " + this.name + "  dont see any targets");
                return;
            }
        }
        
        #endregion
        
        if (Behaviour.MoveTo(pos, AiData.Speed, AiData.MeleeAttackThreshold, true))
        {
            Behaviour.MeleeAttack(pos);
        }
    }

    public void NavMelee(GameObject target, float speed=0)
    {
        #region Get Nearest + Null Check
        var nearestEnemy = (this.tag == "Prey" ? Checks.ClosestDrone() : Checks.WeakestCreature());
        Vector2 pos;
        if (nearestEnemy != null)
        {
            pos = nearestEnemy.transform.position;
        }
        else
        {
            nearestEnemy = Checks.ClosestCreature(new string[] { (this.tag == "Prey" ? "Prey" : "Predator") });
            if (nearestEnemy != null)
                pos = nearestEnemy.transform.position;
            else
                return;
        }
        #endregion
        if (speed == 0)
            speed = AiData.Speed;
        if (NavChase(nearestEnemy, speed, AiData.MeleeAttackThreshold) || Vector2.Distance(nearestEnemy.transform.position, this.transform.position) <= AiData.EngageHostileThreshold)
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
        //Debug.Log("Entering Ranged Protocol");
        #region Null Check
        Vector2 pos;
        var nearestEnemy = (target);
        if (target != null)
        {
            pos = target.transform.position;
        }
        else
        {
            if (this.CompareTag("Prey"))
                nearestEnemy = Checks.ClosestCreature(new string[] {"Prey", "Player"});
            else
                nearestEnemy = Checks.ClosestCreature(new string[] {"Predator"});


            if (nearestEnemy != null)
                pos = nearestEnemy.transform.position;
            else
                return;
        }
        #endregion
        var rat = AiData.RangeAttackThreshold;
        var eht = AiData.EngageHostileThreshold;
        var engage = CheckRangedThreshold(pos, rat, eht);
        if (engage < 0)
        {
            //Debug.Log("Engage < 0");
            if (Behaviour.MoveFrom(pos, AiData.Speed, rat - eht))
            {
                Behaviour.RangedAttack(pos, AiData.Speed);
            }
        }
        else if (engage > 0)
        {
            //Debug.Log("Engage > 0");
            if (Behaviour.MoveTo(pos, AiData.Speed, rat + eht))
            {
                Behaviour.RangedAttack(pos, AiData.Speed);
            }
        }
        else
        {
            //Debug.Log("Engage == 0");
            Behaviour.RangedAttack(pos, AiData.Speed);
        }
    }
    public void NavRanged()
    {
        #region Get Nearest + Null Check
        //var nearestEnemy = AiData.Checks.ClosestCreature(new string[] { "Predator" }, false);
        Vector2 pos = Checks.specialPosition;
        GameObject targ = Checks.specialTarget;
        if (targ != null)
        {
            pos = targ.transform.position;
        }
        /*else if (nearestEnemy != null)
        {
            pos = nearestEnemy.transform.position;
        }*/
        #endregion
        TileNode tile = Checks.GetNearestNode(pos);
        if (CheckThreshold(pos, AiData.EngageHostileThreshold))
        {
            if (NavTo(tile, AiData.Speed))
            {
                //Behaviour.RangedAttack(pos, AiData.Speed);
                Ranged(targ!=null ? targ : null);
            }
        }
        else NavRunaway();
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
        if(Checks == null)
        {
            Debug.Log("Error: MonsterProtocols has no reference to Checks");
            return;
        }
        GameObject targ = Checks.specialTarget;
        Vector2 pos = Checks.specialPosition;
        if (targ != null)
        {
            pos = targ.transform.position;
            if (NavChase(targ, AiData.Speed, 0.5f))
            {
                Behaviour.Idle();
            }
        }
        else if (pos != Vector2.zero)
        {
            if (NavChase())
            {
                Behaviour.Idle();
                Checks.specialPosition = Vector2.zero;
            }
        }
        else
            Behaviour.Idle();
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
        GameObject creature = Checks.specialTarget;
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
            //Feast(true); TODO: Why was this needed, and did deleting it break anything?
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
        Checks.SetCurrentTile();
        Vector2 pos = Checks.specialPosition;
        //Debug.Log("NavChase Position: " + pos);
        GameObject targ = Checks.specialTarget;
        if (targ != null)
        {
            //Debug.Log("Special target: " + targ);
            pos = targ.transform.position;
            TileNode tile = Checks.GetNearestNode(pos);
            //Debug.Log("Special Target - Nearest tile node null: " + (tile == null));
            return (NavTo(tile, AiData.Speed, 3));
        }
        else if (pos != Vector2.zero)
        {
            TileNode tile = Checks.GetNearestNode(pos);
            //Debug.Log("Non Target - Nearest tile node null: " + (tile == null));
            return NavTo(tile, AiData.Speed, 3);
        }
        return true;
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
        pos += new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        #endregion

        // move to
        if (Behaviour.MoveTo(pos, AiData.Speed, AiData.MeleeAttackThreshold))
        {
            // socialize
            Behaviour.Socialize();
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
        //Debug.Log(this.gameObject + "IS ENTERING NAVTO");

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
            //Debug.Log(AiData.path);
            return false;
        }

        //set tile debug color
        if (AiData.path != null && showPathfindingColor)
        {
            //Debug.Log("setting color...");
            foreach (TileNode tile in AiData.path)
            {
                tile.GetComponent<SpriteRenderer>().color = pathfinderColor;
            }
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
