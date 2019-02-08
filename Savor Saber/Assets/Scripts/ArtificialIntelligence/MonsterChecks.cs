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
    }

    /// <summary>
    /// checks for all creatures, Max of 10 (Change this in AIData in Start() for NearbyCreatures[]
    /// </summary>
    public void AwareHowMany()
    {
        //AiData.Awareness.activate = true;
        // reset nums
        numEnemiesNear = 0;
        numFriendsNear = 0;
        foreach (GameObject Creature in AllCreatures)
        {
            // for now nothing
            // heauristic for sorting into new friends and new enemies
            Debug.Log("Checking if Friend or Enemy...");
            if (Enemies.Contains(Creature)) { numEnemiesNear++; }
            if (Friends.Contains(Creature)) { numFriendsNear++; }
        }
        Debug.Log(this + " is surrounded by " + AllCreatures.Count + " Creature" + (AllCreatures.Count==1?": ":"s: ") +  " Friends = " + numFriendsNear + " Enemies = " + numEnemiesNear);
    }

    /// <summary>
    /// for every creature in the nearbycreatures list, inserts them into a dictionary with (Collider2D : character data)
    /// </summary>
    public void AwareNearby()
    {
        // clear all creatues
        AllCreatures.Clear();
        // update all creatures
        GameObject obtainSurroundings = Instantiate(signalPrefab, this.transform, true) as GameObject;
        SignalApplication signalModifier = obtainSurroundings.GetComponent<SignalApplication>();
        signalModifier.SetSignalParameters(this.gameObject, AiData.Perception, new Dictionary<string, float>() { }, true, true, true, true);
        AiData.Awareness = signalModifier;
        // the signal will notify the signal creator of this data once it is dead
    }


 
    /// <summary>
    /// Checks closest enemy from enemy/friend dictionary
    /// </summary>
    /// <returns> Collider2D of closest enemy or friend </returns>
    public GameObject ClosestCreature(string frenemy)
    {
        #region Initialize Friend and Enemy
        float closestF = closestDistance;
        float closestE = closestDistance;
        float closestN = closestDistance;
        GameObject ClosestFriend = null;
        GameObject ClosestEnemy = null;
        GameObject ClosestNeutral = null;
        float dist;
        #endregion
        #region Debug
        if (frenemy != "friend" || frenemy != "enemy" || frenemy != "neutral")
        {
            Debug.Log(frenemy + "is not recognized. 'friend', 'enemy', and 'neutral' are recognized inputs");
        }
        #endregion
        foreach (GameObject Creature in AllCreatures)
        {
            if (Enemies.Contains(Creature))
            {
                dist = Vector2.Distance(transform.position, Creature.transform.position);
                if (dist < closestE)
                {
                    closestE = dist;
                    ClosestEnemy = Creature;
                }
            }
            if (Friends.Contains(Creature))
            {
                dist = Vector2.Distance(transform.position, Creature.transform.position);
                if(dist < closestF)
                {
                    closestF = dist;
                    ClosestFriend = Creature;
                }
            }
            #region if (NeutralCreature)
            dist = Vector2.Distance(transform.position, Creature.transform.position);
            if(dist < closestN)
            {
                closestN = dist;
                ClosestNeutral = Creature;
            }
            #endregion
        }
        if (frenemy == "friend")
        {
            return ClosestFriend;
        }else if(frenemy == "enemy")
        {
            return ClosestEnemy;
        }else if(frenemy == "neutral")
        {
            return ClosestNeutral;
        }
        return null;
        
    }
    public GameObject ClosestCreature()
    {
        float closest = closestDistance;
        GameObject ClosestCreature = null;

        foreach (GameObject Creature in AllCreatures)
        {
            float dist = Vector2.Distance(transform.position, Creature.transform.position);
            if (dist < closest)
            {
                closest = dist;
                ClosestCreature = Creature;
            }
           
        }
        return ClosestCreature;
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
        return ClosestCreature("enemy").gameObject.transform.position;
    }
    public Vector2 NearestFriendPosition()
    {
        return ClosestCreature("friend").gameObject.transform.position;
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
