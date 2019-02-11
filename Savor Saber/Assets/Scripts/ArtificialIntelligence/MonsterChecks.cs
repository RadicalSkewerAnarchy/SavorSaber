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
    /// Checks closest enemy from enemy/friend dictionary
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

    public GameObject ClosestDrop()
    {
        #region Initialize Friend and Enemy
        float close = closestDistance;
        GameObject closestDrop = null;
        #endregion
        foreach (GameObject Creature in AllCreatures)
        {
            #region Check if Creature Deleted
            if (Creature == null)
            {
                continue;
            }
            #endregion
            if (Creature.tag == "SkewerableObject")
            {
                float dist = Vector2.Distance(transform.position, Creature.transform.position);
                if (dist < close)
                {
                    close = dist;
                    closestDrop = Creature;
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
   /// 
   /// </summary>
   /// <returns> CharacterData record of given Creature as recorded by THIS creature </returns>
    /*public CharacterData AssessCreature(Collider2D Creature)
    {
        return CreaturesDictionary[Creature];
    }*/

}
