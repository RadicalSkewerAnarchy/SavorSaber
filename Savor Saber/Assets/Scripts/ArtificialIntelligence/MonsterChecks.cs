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
    /// <summary>
    /// Used for (ObjectCollider : DistanceToObject)
    /// </summary>
   // public Dictionary<Collider2D, float> CreatureDistancesDictionary;
    public Dictionary<Collider2D, CharacterData> FriendlyCreaturesDictionary;
    public Dictionary<Collider2D, CharacterData> EnemyCreaturesDictionary;
    public Dictionary<Collider2D, CharacterData> CreaturesDictionary;

    // list strategy
    public List<GameObject> Friendlies;
    public List<GameObject> Enemies;

    /// <summary>
    /// Closest Friend and Closest Enemy for quick access
    /// </summary>
    Collider2D ClosestFriendly;
    Collider2D ClosestEnemy;
    float closestDistance = 10000000;
    

    private void Start()
    {
        AiData = GetComponent<AIData>();
        GameObject soma = GameObject.FindGameObjectWithTag("Player");
        Enemies.Add(soma);
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
        foreach (Collider2D Creature in AiData.NearbyCreatures)
        {
            //var distance = Vector2.Distance(AiData.Position, Creature.gameObject.transform.position);
            var charData = Creature.gameObject.GetComponent<CharacterData>();

            //charData.distanceFrom = distance;
            CreaturesDictionary[Creature] = charData;
            if(charData.moods["Friendliness"] >= .5f)
            {
                FriendlyCreaturesDictionary[Creature] = charData;
            }
            //CreatureDistancesDictionary.Add(Creature, distance);            
        }
    }


    /// MODIFIED WITH LISTS INSTEAD
    /// <summary>
    /// Checks closest enemy from enemy/friend dictionary
    /// </summary>
    /// <returns> Collider2D of closest enemy or friend </returns>
    public GameObject ClosestEnemyCreature()
    {
        float closest = 10000000f;
        GameObject ClosestEnemy = null;
        foreach(GameObject Creature in Enemies)
        {
            float dist = Vector2.Distance(transform.position, Creature.transform.position);
            if(dist < closest)
            {
                closest = dist;
                ClosestEnemy = Creature;
            }
        }
        return ClosestEnemy;
    }
    /*public Collider2D ClosestEnemyCreature()
    {
        float closest = 10000000f;
        foreach(KeyValuePair<Collider2D, CharacterData> Creature in EnemyCreaturesDictionary)
        {
            if(Creature.Value.distanceFrom < closest)
            {
                closest = Creature.Value.distanceFrom;
                ClosestEnemy = Creature.Key;
            }
        }
        return ClosestEnemy;
    }*/


    public Collider2D ClosestFriendlyCreature()
    {
        float closest = 10000000;
        foreach (KeyValuePair<Collider2D, CharacterData> Creature in FriendlyCreaturesDictionary)
        {
            if (Creature.Value.distanceFrom < closest)
            {
                closest = Creature.Value.distanceFrom;
                ClosestFriendly = Creature.Key;
            }
        }
        return ClosestFriendly;
    }

    

    /// <returns> Count of Enemy and Friend Dictionaries </returns>
    public int NumberOfEnemies()
    {
        return Enemies.Count;
    }
    public int NumberOfFriends()
    {
        return Friendlies.Count;
    }



    /// <returns> Vector2 of Closest Enemy or Friend </returns>
    public Vector2 NearestEnemyPosition()
    {
        return ClosestEnemyCreature().gameObject.transform.position;
    }
    public Vector2 NearestFriendPosition()
    {
        return ClosestFriendlyCreature().gameObject.transform.position;
    }


   /// <summary>
   /// 
   /// </summary>
   /// <returns> CharacterData record of given Creature as recorded by THIS creature </returns>
    public CharacterData AssessCreature(Collider2D Creature)
    {
        return CreaturesDictionary[Creature];
    }

}
