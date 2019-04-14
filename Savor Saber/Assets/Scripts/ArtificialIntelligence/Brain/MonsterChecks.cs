﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIData))]

public class MonsterChecks : MonoBehaviour
{
    #region Initialize
    /// <summary>
    /// brings AIData for easy reference;
    /// </summary>
    public AIData AiData;
    public GameObject signalPrefab;
    public BoxCollider2D boxCollider;

    public List<GameObject> Friends;
    public List<GameObject> Enemies;
    public List<GameObject> AllCreatures;
    public List<GameObject> AllPlants;
    public List<GameObject> AllDrops;

    /// <summary>
    /// Closest Friend and Closest Enemy for quick access
    /// </summary>
    GameObject ClosestFriendly;
    GameObject ClosestEnemy;
    float closestDistance;

    /// <summary>
    /// Special Targets and Locations
    /// </summary>
    public GameObject specialTarget = null;
    public GameObject specialLeader = null;
    public Vector2 specialPosition;
    bool amLeader = false;
    bool positionAcquired = false;
    public TileNode currentTile = null;
    #endregion

    private void Start()
    {
        closestDistance = Mathf.Infinity;
        AiData = GetComponent<AIData>();
        GameObject soma = GameObject.FindGameObjectWithTag("Player");

        // Know who is friend and foe
        Friends = AiData.Friends;
        Enemies = AiData.Enemies;
        Enemies.Add(soma);
        Friends.Add(soma);

        // clear often
        AllCreatures = new List<GameObject>();
        AllPlants = new List<GameObject>();
        AllDrops = new List<GameObject>();

        // specials
        specialPosition = transform.position;

        currentTile = new TileNode();
    }

    #region AWARENESS
    /// <summary>
    /// checks for all creatures, Max of 10 (Change this in AIData in Start() for NearbyCreatures[]
    /// </summary>
    public int AwareHowMany()
    {
        return AllCreatures.Count;
    }

    /// <summary>
    /// for every creature in the nearbycreatures list, inserts them into a dictionary with (Collider2D : character data)
    /// </summary>
    public void AwareNearby()
    {
        // clear all creatues
        AllCreatures.Clear();
        // update all creatures
        GameObject obtainSurroundings = Instantiate(signalPrefab, this.transform.position, Quaternion.identity) as GameObject;
        SignalApplication signalModifier = obtainSurroundings.GetComponent<SignalApplication>();
        signalModifier.SetSignalParameters(this.gameObject, AiData.Perception, new Dictionary<string, float>() { }, true, false);
        AiData.Awareness = signalModifier;
        // the signal will notify the signal creator of this data once it is dead
    }
    #endregion

    #region LOCATIONAL CHECKS
    /// <summary>
    /// Checks creatures and returns closest
    /// </summary>
    /// <returns> Collider2D of closest enemy or friend </returns>
    public GameObject ClosestCreature()
    {
        #region Initialize closest vars
        float close = closestDistance;
        GameObject closestCreature = null;
        #endregion
        foreach (GameObject Creature in AllCreatures)
        {
           // Debug.Log("Checking creatures");
            #region Check if Creature Deleted
            if (Creature == null)
            {
                continue;
            }
            #endregion
            float dist = Vector2.Distance(transform.position, Creature.transform.position);
            if (dist < close)
            {
                //Debug.Log("Closest is found");
                close = dist;
                closestCreature = Creature;
            }
        }
        //if (closestCreature != null) { Debug.Log("Closest Creature Name: " + closestCreature.name); }
        return closestCreature;
    }


    /// <summary>
    /// Checks creatures and returns weakest
    /// </summary>
    /// <returns> Collider2D of closest enemy or friend </returns>
    public GameObject WeakestCreature()
    {
        #region Initialize closest vars
        float weak = 10f;
        GameObject weakest = null;
        #endregion
        foreach (GameObject Creature in AllCreatures)
        {
            #region Check if Creature Deleted
            if (Creature == null)
            {
                continue;
            }
            #endregion
            float dist = Creature.GetComponent<CharacterData>().health;
            if (dist < weak)
            {
                weak = dist;
                weakest = Creature;
            }
        }

        return ( weakest==null ? this.gameObject : weakest );
    }

    /// <summary>
    /// Checks creatures and returns the average group position
    /// </summary>
    /// <returns> Collider2D of closest enemy or friend </returns>
    public Vector2 AverageGroupPosition()
    {
        float avgx = 0f, avgy = 0f;
        float count = AllCreatures.Count;
        foreach (GameObject Creature in AllCreatures)
        {
            #region Check if Creature Deleted
            if (Creature == null)
            {
                count--;
                continue;
            }
            #endregion
            avgx += Creature.transform.position.x;
            avgy += Creature.transform.position.y;
        }

        if (count == 0) { return transform.position; }
        // average out positions
        avgx /= count;
        avgy /= count;
        // send new vector
        return new Vector2(avgx, avgy);
    }

    /// <summary>
    /// Return Closest Drop
    /// </summary>
    /// <returns></returns>
    public GameObject ClosestPlant()
    {
        #region Initialize Friend and Enemy
        float close = closestDistance;
        GameObject closestPlant = null;
        #endregion
        foreach (GameObject Plant in AllPlants)
        {
            #region Check if Creature Deleted
            if (Plant == null)
                continue;
            if (Plant.GetComponent<DestructableEnvironment>().destroyed)
                continue;
            #endregion
            //Debug.Log("Potential Plant: " + Creature.GetInstanceID());
            //Debug.Log(Plant.GetInstanceID() + " is a drop");
            float dist = Vector2.Distance(transform.position, Plant.transform.position);
            if (dist < close)
            {
                close = dist;
                closestPlant = Plant;
            }

        }
        //Debug.Log("Closest drop is reached = " + (closestPlant == null ? "and it is null" : closestPlant.name + closestPlant.GetInstanceID()));
        return closestPlant;
    }
    
    /// <summary>
    /// Return Closest Drop
    /// </summary>
    /// <returns></returns>
    public GameObject ClosestDrop()
    {
        #region Initialize Friend and Enemy
        float close = closestDistance;
        GameObject closestDrop = null;
        #endregion
        foreach (GameObject Drop in AllDrops)
        {
            #region Check if Creature Deleted
            if (Drop == null)
            {
                continue;
            }
            #endregion
            //Debug.Log("Potential Drop: " + Creature.GetInstanceID());
            //Debug.Log(Drop.GetInstanceID() + " is a drop");
            float dist = Vector2.Distance(transform.position, Drop.transform.position);
            if (dist < close)
            {
                close = dist;
                closestDrop = Drop;
            }
        }
        //Debug.Log("Closest drop is reached = " + (closestDrop == null ? "and it is null" : closestDrop.name + closestDrop.GetInstanceID()));
        return closestDrop;
    }

    /// <returns> Vector2 of Closest Enemy or Friend </returns>
    public Vector2 NearestEnemyPosition()
    {
        return ClosestCreature().gameObject.transform.position;
    }
    public Vector2 NearestFriendPosition()
    {
        return ClosestCreature().gameObject.transform.position;
    }

    #endregion

    #region CONGA LINE CHECKS
    ///Conga Line Functions
    public void BecomeLeader()
    {
        bool become = true;
        amLeader = false;
        foreach (GameObject Creature in AllCreatures)
        {
            #region Check if Creature Deleted
            if (Creature == null || Creature.tag == "Player")
            {
                continue;
            }
            #endregion
            MonsterChecks creatureCheck = Creature.GetComponent<MonsterChecks>();
            if (creatureCheck.AmLeader())
            {
                //Debug.Log(Creature.name + " is the Leader of " + this.name);
                // set leader
                specialLeader = creatureCheck.specialLeader;
                // set MY target to my leader
                this.SetSpecialTarget(creatureCheck.specialTarget);
                // set my leader's target to ME
                creatureCheck.SetSpecialTarget(this.gameObject);
                become = false;
            }
            else if (creatureCheck.specialLeader != null && specialTarget == this.gameObject)
            {
                // set leader
                GameObject lead = creatureCheck.specialLeader;
                creatureCheck = lead.GetComponent<MonsterChecks>();
                // set leader
                specialLeader = creatureCheck.specialLeader;
                // set MY target to my leader
                //this.SetSpecialTarget(creatureCheck.specialTarget);
                // set my leader's target to ME
                creatureCheck.SetSpecialTarget(this.gameObject);
                become = false;
            }
        }
        // no one is a leader, so be the leader
        if (become)
        {
            //Debug.Log(this.name + " is the Leader now!!!");
            amLeader = true;
            specialTarget = this.gameObject;
            specialLeader = this.gameObject;
        }
    }
    /// <summary>
    /// Checks creatures and returns closest leader
    /// </summary>
    public GameObject ClosestLeader()
    {
        bool foundLeader = false;
        bool foundSpecial = false;
        foreach (GameObject Creature in AllCreatures)
        {
            #region Check if Creature Deleted
            if (Creature == null)
            {
                continue;
            }
            #endregion
            if (Creature == specialLeader)
            {
                foundLeader = true;
            }
            if (Creature == specialTarget)
            {
                foundSpecial = true;
            }
        }
        // only return positions if target OR leader are found
        if (!foundLeader) { specialLeader = null; }
        //if (!foundSpecial) { specialTarget = null; }
        return (specialTarget == null ? this.gameObject : specialTarget);
    }

    // am leader
    public bool AmLeader()
    {
        return amLeader;
    }
    #endregion

    #region COUNT CHECKS
    /// COUNT FUNCTIONS
    /// <returns> Count of Enemy and Friend Lists </returns>
    public int NumberOfEnemies()
    {
        return Enemies.Count;
    }
    public int NumberOfFriends()
    {
        return Friends.Count;
    }
    #endregion

    #region (Re)Set Specials
    // set specials
    public void SetSpecialTarget(GameObject g)
    {
        this.specialTarget = g;
    }
    // set specials
    public void SetSpecialPosition(Vector2 v)
    {
        this.specialPosition = v;
    }
    // get specials
    public Vector2 GetSpecialPosition()
    {
        return this.specialPosition;
    }
    // reset
    public void ResetSpecials()
    {
        this.specialTarget = null;
        //this.specialLeader = null;
        this.specialPosition = new Vector2(0f, 0f);
    }
    #endregion

    // given any position, determine what tile that position is on(if it is)
    // if it's not, don't navigate there
    public TileNode GetNearestNode(Vector2 pos)
    {
        TileNode targetTile = null;
        float sizeCheck = .25f;
        int maxTries = 5;

        for (var i = 0; i < maxTries; i++)
        {
            var availableNodes = Physics2D.OverlapCircleAll(pos, sizeCheck);
            var validNodes = new List<Collider2D>();
            foreach (var node in availableNodes)
            {
                if (node.GetComponent<TileNode>() != null)
                {
                    validNodes.Add(node);
                }
            }

            if (validNodes.Count > 0)
            {
                // RETURN VALID TILE
                // Debug.Log("choosing from " + validNodes.Count + " possible nodes");
                targetTile = validNodes[(int)Random.Range(0, validNodes.Count)].GetComponent<TileNode>();
                return targetTile;
            }
            else
            {
                sizeCheck *= 2;
                // Debug.Log("DOUBLING TILECHECK SIZE");
            }
        }

        return targetTile;
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<TileNode>() != null)
        {
            if (collision.gameObject.GetComponent<TileNode>().walkable)
            {
                currentTile = collision.gameObject.GetComponent<TileNode>();
            }
        }
    }*/
    #region POSITION RANDOMIZATION
    /// <summary>
    /// Given some distribution, return a vector2 of:
    ///     the closest creauture
    ///     the weakest creature
    ///     the group average
    /// with null checking
    /// </summary>
    /// <returns> CharacterData record of given Creature as recorded by THIS creature </returns>
    public Vector2 GetRandomPositionType()
    {
        // position to return
        Vector2 pos;
        GameObject cre;
        if (specialTarget == null && specialPosition == (Vector2)transform.position)
        {
            // randomly choose
            pos = transform.position;
            cre = specialTarget;
            float rand = Random.Range(0, 100);
            // closest
            if (rand < 30)
            {
                cre = ClosestCreature();
                pos = (cre == null ? transform.position : cre.transform.position);
            }
            // weakest
            else if (rand < 60)
            {
                cre = WeakestCreature();
                pos = (cre == null ? transform.position : cre.transform.position);
            }
            // group
            else
            {
                cre = this.gameObject;
                pos = AverageGroupPosition();
            }

            // set specials
            specialTarget = cre;
            specialPosition = pos;
        }
        else
        {
            cre = specialTarget;
            pos = (cre == null ? specialPosition : (Vector2)cre.transform.position);
        }
        // return
        return pos;
    }

    public void SetRandomPosition(float xx, float yy)
    {
        // position to return
        float xxx = Random.Range(-xx, xx);
        float yyy = Random.Range(-yy, yy);
        if (specialPosition == new Vector2(0f, 0f))
        {
            specialPosition = transform.position + new Vector3(xxx, yyy, 0f);
        }
    }

    public List<TileNode> GetLongestPath(TileNode tile)
    {
        List<TileNode> path = null;
        path = AiData.Behavior.pathfinder.AStar(currentTile, tile);
        return path;
    }

    public void SetCurrentTile()
    {
        currentTile = GetNearestNode(this.transform.position);
    }
    #endregion
}
