using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIData))]

public class MonsterChecks : MonoBehaviour
{
    /// <summary>
    /// brings AIData for easy reference;
    /// </summary>
    public AIData AiData;

    private List<GameObject> Friends;
    private List<GameObject> Enemies;
    private List<GameObject> AllCreatures;

    private int numEnemiesNear = 0;
    private int numFriendsNear = 0;

    /// <summary>
    /// Closest Friend and Closest Enemy for quick access
    /// </summary>
    GameObject ClosestFriendly;
    GameObject ClosestEnemy;
    float closestDistance = 10000000f;


    private void Start()
    {
        AiData = GetComponent<AIData>();
        /*GameObject soma = GameObject.FindGameObjectWithTag("Player");
        Enemies.Add(soma);*/
        Friends = AiData.Friends;
        Enemies = AiData.Enemies;
        // clear often
        AllCreatures = new List<GameObject>();
    }

    /// <summary>
    /// empty array of nearby seen creatures
    /// </summary>


    /// Awareness and Evaluation

    /// <summary>
    /// checks for all creatures, Max of 10 (Change this in AIData in Start() for NearbyCreatures[]
    /// currently checks layer 11 for monsters.
    /// </summary>
    public void AwareHowMany()
    {
        Physics2D.OverlapCircleNonAlloc(transform.position, AiData.Perception, AiData.NearbyCreatures, 11);
    }



    /// <summary>
    /// for every creature in the nearbycreatures list, inserts them into a dictionary with (Collider2D : character data)
    /// </summary>
    public void AwareNearby()
    {
        // clear all creatues
        // update all creatues
        numEnemiesNear = 0;
        numFriendsNear = 0;
        foreach (GameObject Creature in AllCreatures)
        {
            // for now nothing
            // heauristic for sorting into new friends and new enemies
            if (Enemies.Contains(Creature)) { numEnemiesNear++; }
            if (Friends.Contains(Creature)) { numFriendsNear++; }
        }
    }


    /// MODIFIED WITH LISTS INSTEAD
    /// <summary>
    /// Checks closest enemy from enemy/friend dictionary
    /// </summary>
    /// <returns> Collider2D of closest enemy or friend </returns>
    public GameObject ClosestEnemyCreature()
    {
        AwareNearby();

        float closest = closestDistance;
        GameObject ClosestEnemy = null;

        foreach(GameObject Creature in AllCreatures)
        {
            if (Enemies.Contains(Creature))
            {
                float dist = Vector2.Distance(transform.position, Creature.transform.position);
                if (dist < closest)
                {
                    closest = dist;
                    ClosestEnemy = Creature;
                }
            }
        }
        return ClosestEnemy;
    }


    public GameObject ClosestFriendCreature()
    {
        AwareNearby();

        float closest = closestDistance;
        GameObject ClosestFriend = null;

        foreach (GameObject Creature in AllCreatures)
        {
            if (Friends.Contains(Creature))
            {
                float dist = Vector2.Distance(transform.position, Creature.transform.position);
                if (dist < closest)
                {
                    closest = dist;
                    ClosestFriend = Creature;
                }
            }
        }
        return ClosestFriend;
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
        return ClosestEnemyCreature().gameObject.transform.position;
    }
    public Vector2 NearestFriendPosition()
    {
        return ClosestFriendCreature().gameObject.transform.position;
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
