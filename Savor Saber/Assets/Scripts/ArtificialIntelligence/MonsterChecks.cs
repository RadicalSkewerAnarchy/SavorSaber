using System.Collections;
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

    public List<GameObject> Friends;
    public List<GameObject> Enemies;
    public List<GameObject> AllCreatures;
    public List<GameObject> AllDrops;

    private int numEnemiesNear = 0;
    private int numFriendsNear = 0;

    /// <summary>
    /// Closest Friend and Closest Enemy for quick access
    /// </summary>
    GameObject ClosestFriendly;
    GameObject ClosestEnemy;
    float closestDistance = 10000000f;
    #endregion

    private void Start()
    {
        AiData = GetComponent<AIData>();
        GameObject soma = GameObject.FindGameObjectWithTag("Player");
        //Enemies.Add(soma);
        Friends = AiData.Friends;
        Enemies = AiData.Enemies;
        Enemies.Add(soma);
        Friends.Add(soma);
        // clear often
        AllCreatures = new List<GameObject>();
        AllDrops = new List<GameObject>();
    }

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
        GameObject obtainSurroundings = Instantiate(signalPrefab, this.transform, false) as GameObject;
        SignalApplication signalModifier = obtainSurroundings.GetComponent<SignalApplication>();
        signalModifier.SetSignalParameters(this.gameObject, AiData.Perception, new Dictionary<string, float>() { }, true, true);
        AiData.Awareness = signalModifier;
        // the signal will notify the signal creator of this data once it is dead
    }


 
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
            #region Check if Creature Deleted
            if (Creature == null)
            {
                continue;
            }
            #endregion
            float dist = Vector2.Distance(transform.position, Creature.transform.position);
            if (dist < close)
            {
                close = dist;
                closestCreature = Creature;
            }
        }

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

        return weakest;
    }
    
    /// <summary>
    /// Checks creatures and returns weakest
    /// </summary>
    /// <returns> Collider2D of closest enemy or friend </returns>
    public Vector2 AverageGroupPosition()
    {
        float avgx=0f, avgy=0f;
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

    public GameObject ClosestDrop()
    {
        #region Initialize Friend and Enemy
        float close = closestDistance;
        GameObject closestDrop = null;
        #endregion
        foreach (GameObject Drop in AllCreatures)
        {
            #region Check if Creature Deleted
            if (Drop == null)
            {
                continue;
            }
            #endregion
            //Debug.Log("Potential Drop: " + Creature.GetInstanceID());
            if (Drop.tag == "SkewerableObject")
            {
                Debug.Log(Drop.GetInstanceID() + " is a drop");
                float dist = Vector2.Distance(transform.position, Drop.transform.position);
                if (dist < close)
                {
                    close = dist;
                    closestDrop = Drop;
                }
            }
        }
        return closestDrop;
    }

    /// <returns> Count of Enemy and Friend Dictionaries </returns>
    public int NumberOfEnemies()
    {
        return numEnemiesNear;
    }
    public int NumberOfFriends()
    {
        return numFriendsNear;
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
   
        // randomly choose
        float rand = Random.Range(0, 100);
        // closest
        if (rand < 30)
        {
            cre = ClosestCreature();
            pos = (cre == null ? transform.position : ClosestCreature().transform.position);
        }
        // weakest
        else if (rand < 60)
        {
            cre = WeakestCreature();
            pos = (cre == null ? transform.position : ClosestCreature().transform.position);
        }
        // group
        {
            pos = AverageGroupPosition();
        }

        // return
        return pos;
    }

}
