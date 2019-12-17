﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour
{
    // default commands
    // subjects being commanded
    public List<GameObject> Subjects;
    public List<GameObject> FamilyTrees;
    private Dictionary<string, GameObject> Families;
    private CrosshairController cross;
    public string FamilyChoice;

    // the protocol to follow
    public AIData.Protocols Verb;

    // the criteria of the object
    public enum Criteria
    {
        None,
        NearestEnemy,
        NearestFriend
    };
    public Criteria ObjectCriteria = Criteria.None;
    public GameObject Object;
    public GameObject Cursor;
    public Vector2 Location;

    #region Object Trackers
    private Queue<GameObject> lastSelected = new Queue<GameObject>();
    private GameObject[] selectedFruit = { null, null };
    private GameObject[] selectedDrone = { null, null };
    private GameObject[] selectedNode = { null, null };
    private GameObject[] selectedFood = { null, null };
    private GameObject nearestFruitant;
    private GameObject nearestDrone;
    private GameObject nearestTileNode;
    private GameObject nearestFood;
    #endregion

    // AI specific knowledge
    AIData Brain;
    // player ref
    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        // DO THIS IN THE INSPECTOR:
        // give the Commander knowledge of the groups
        // of agents that can be commanded all at once
        //--------------------------
        Families = new Dictionary<string, GameObject>();
        foreach(GameObject fam in FamilyTrees)
        {
            Families.Add(fam.name, fam);
        }

        player = GameObject.FindGameObjectWithTag("Player");
        cross = GameObject.FindObjectOfType<CrosshairController>();
        if (Cursor==null)Debug.Log(this.name + " NEEDS A REFERENCE TO THE PLAYER'S CURSOR! in the inspector");
        
    }

    // Update is called once per frame
    void Update()
    {
        //=========
        // CLICK
        //=========
        if (InputManager.GetButtonDown(Control.Skewer, InputAxis.Skewer))
        {
            // SET ALL THE NEARESTS
            SetMostRelevant();

            // SELECT AGENT AND TARGET
            // if we dont have anything selected...
            // lastSelected can be a FRUIT or a DRONE
            if (lastSelected.Count == 0)
            {
                // check if there is a closest party member
                if (player.GetComponent<PlayerData>().party.Contains(nearestFruitant))
                {
                    SelectFruitant(nearestFruitant);
                    nearestFruitant.GetComponent<Squeezer>().Wiggle(1, 5, 5, 0.1f, 0.1f);
                    Cursor.GetComponent<CrosshairClicker>().PlaySelectionSounds(lastSelected.Count);
                }
                /*else if (nearestDrone != null)
                {
                    SelectDrone(nearestDrone);
                    nearestDrone.GetComponent<Squeezer>().Wiggle(1, 10, 10, 0.05f, 0.05f);
                    Cursor.GetComponent<CrosshairClicker>().PlaySelectionSounds(lastSelected.Count);
                }*/
            }
            else if (lastSelected.Count == 1)
            {
                // check if there is a closest party member
                if (nearestDrone != null)
                {
                    SelectDrone(nearestDrone);
                    nearestDrone.GetComponent<Squeezer>().Wiggle(1, 10, 10, 0.05f, 0.05f);
                    Cursor.GetComponent<CrosshairClicker>().PlaySelectionSounds(3);
                }
                else if (player.GetComponent<PlayerData>().party.Contains(nearestFruitant))
                {
                    // remove current selected fruitant
                    //lastSelected.Dequeue();
                    SelectFruitant(nearestFruitant);
                    nearestFruitant.GetComponent<Squeezer>().Wiggle(1, 5, 5, 0.1f, 0.1f);
                    Cursor.GetComponent<CrosshairClicker>().PlaySelectionSounds(lastSelected.Count);
                }
                else if (nearestTileNode != null)
                {
                    SelectNode(nearestTileNode);
                    Cursor.GetComponent<CrosshairClicker>().PlaySelectionSounds(lastSelected.Count);
                }
                
                else Cursor.GetComponent<CrosshairClicker>().PlaySelectionSounds(0);
            }

            // ISSUE COMMAND
            // now we have a specific agent to give a command to...
            // a specific command
            if (lastSelected.Count > 1)
            {
                // could be FRUIT or DRONE or TILE or FOOD
                GameObject applyCommandTo = lastSelected.Dequeue();
                GameObject target = lastSelected.Dequeue(); ;
                // if FRUIT
                if (applyCommandTo.CompareTag("Prey"))
                {
                    if (target.CompareTag("Prey"))
                    {
                        Command(applyCommandTo, AIData.Protocols.Chase, target, target.transform.position);
                    }
                    else if (target.CompareTag("Predator"))
                    {
                        Command(applyCommandTo, AIData.Protocols.Attack, target, target.transform.position);
                    }
                    else // is tilenode
                    {
                        Command(applyCommandTo, AIData.Protocols.Lazy, target, Cursor.transform.position);
                    }
                }
                // can "technically" command drones, but that's for later
                else if (applyCommandTo.CompareTag("Predator"))
                {

                }
                // clear selection after 2?
                lastSelected.Clear();
            }
        }

        #region keyboard input
        // CYCLE SETTINGS OF COMMANDS

        if (Input.GetKeyDown(KeyCode.O))
        {
            CycleTargetFamily();
        }

        // PRESS numbers TO ISSUE COMMAND
        // or replace with general command enum?
        bool in1 = Input.GetKeyDown(KeyCode.Alpha1);
        bool in2 = Input.GetKeyDown(KeyCode.Alpha2);
        bool in3 = Input.GetKeyDown(KeyCode.Alpha3);
        if (in1 || in2 || in3)
        {
            // CHASE
            if (in2)
            {
                Verb = AIData.Protocols.Chase;
                ObjectCriteria = Criteria.None;
                Object = GameObject.FindGameObjectWithTag("Player");
                Debug.Log("Issuing Command: " + FamilyChoice + " " + Verb + ": crit-- " + ObjectCriteria + ", obj-- " + Object + ", loc-- " + Location);
                GroupCommand(player.GetComponent<PlayerData>().party, Verb, ObjectCriteria, Object, Location);
            }
            // IDLE
            else if (in1)
            {
                // on first Lazy call: continue where you were going
                // on second: stop and wait in place
                if (Verb != AIData.Protocols.Lazy)
                {
                    Verb = AIData.Protocols.Lazy;
                    ObjectCriteria = Criteria.None;
                    Location = Vector2.zero;
                    Debug.Log("Issuing Command: " + FamilyChoice + " " + Verb + ": crit-- " + ObjectCriteria + ", obj-- " + Object + ", loc-- " + Location);
                    GroupCommand(player.GetComponent<PlayerData>().party, Verb, ObjectCriteria, null, Location);
                }
            }
            // ATTACK
            else if (in3)
            {
                Verb = AIData.Protocols.Attack;
                ObjectCriteria = Criteria.NearestEnemy;
                Debug.Log("Issuing Command: " + FamilyChoice + " " + Verb + ": crit-- " + ObjectCriteria + ", obj-- " + Object + ", loc-- " + Location);
                GroupCommand(player.GetComponent<PlayerData>().party, Verb, ObjectCriteria, Object, Location);
            }
        }
        #endregion
    }

    private void SelectFruitant(GameObject go)
    {
        selectedFruit[1] = selectedFruit[0];
        selectedFruit[0] = go;
        lastSelected.Enqueue(go);
    }
    private void SelectDrone(GameObject go)
    {
        selectedDrone[1] = selectedDrone[0];
        selectedDrone[0] = go;
        lastSelected.Enqueue(go);
    }
    private void SelectNode(GameObject go)
    {
        selectedNode[1] = selectedNode[0];
        selectedNode[0] = go;
        lastSelected.Enqueue(go);
    }
    private void SelectFood(GameObject go)
    {
        selectedFood[1] = selectedFood[0];
        selectedFood[0] = go;
        lastSelected.Enqueue(go);
    }

    private bool isCloserThan(GameObject a, GameObject b)
    {
        return (Vector3.Distance(player.transform.position, a.transform.position) 
              < Vector3.Distance(player.transform.position, b.transform.position));
    }

    //
    // All Regions
    //

    #region Specific Family Functions

    /// <summary>
    /// Change the current family of agents by dir
    /// AND update Soma's party
    /// </summary>
    /// <param name="dir">number of indexes to move</param>
    private void CycleTargetFamily(int dir=1)
    {
        List<string> keys = new List<string>(Families.Keys);

        // Clear action queues
        foreach (Transform t in Families[FamilyChoice].transform)
        {
            GameObject member = t.gameObject;
            //member.GetComponent<AIData>().ClearActionQueue();
        }

        // Cycle
        int newIndex = keys.IndexOf(FamilyChoice) + dir;
        if (newIndex < 0)
        {
            newIndex = Families.Count - 1;
        }
        else if (newIndex > Families.Count - 1)
        {
            newIndex %= Families.Count;
        }

        // New family
        FamilyChoice = keys[newIndex];

        Debug.Log("Current Family = " + FamilyChoice + " => Soma's Party");

        // JOIN THE PARTY, BROS
        FamilyReunion();
    }

    /// <summary>
    /// Make the current family Soma's party
    /// </summary>
    public void FamilyReunion()
    {
        // clear the party
        ClearParty();

        // add the family to the party
        foreach (Transform t in Families[FamilyChoice].transform)
        {
            GameObject member = t.gameObject;
            JoinTeam(member);
        }
    }

    #endregion

    #region Party Manipulation
    /// <summary>
    /// Add any fruitant to the player's party.
    /// </summary>
    /// <param name="member">the fruitant</param>
    /// <param name="partysize">size to fit to</param>
    /// <param name="partyoverride">remove fruitants in order to fit</param>
    public void JoinTeam(GameObject member, int partysize = 3, bool partyoverride=false)
    {
        // if subject still exists
        if (member != null)
        {
            // get brain
            Brain = member.GetComponent<AIData>();
            if (Brain != null)
            {
                if (player == null) player = GameObject.FindGameObjectWithTag("Player");
                //else Debug.Log("Player not null, so should add to party");
                // set player party
                if (player != null)
                {
                    PlayerData pd = player.GetComponent<PlayerData>();
                    //if (pd == null) Debug.Log("Player Data is null!!!!");
                    if (pd.party.Contains(member))
                    {
                        // do nothing
                    }
                    else if (partyoverride)
                    {
                        AddMember(member, Brain);
                    }
                    else if (pd.party.Count >= partysize)
                    {
                        // remove if over size
                        AddMember(member, Brain);
                        while (pd.party.Count > partysize)
                        {
                            LeaveTeam(pd.party[0]);
                        }
                    }
                    else if (pd.party.Count < partysize)
                    {
                        AddMember(member, Brain);
                    }

                }
                // set mind set
                Brain.CommandCompleted = false;
                Brain.path = null;
            }
            else Debug.Log(member.name + " : has no brain! cannot add to party");
        }
        else Debug.Log(this.name + " : is trying to add a null member to the party");
    }

    private void AddMember(GameObject member, AIData brain)
    {
        PlayerData pd = player.GetComponent<PlayerData>();
        pd.party.Add(member);
        brain.CommandCompleted = false;
        brain.path = null;
        Debug.Log(member.name + " : has joined the party");
    }

    public void LeaveTeam(GameObject member)
    {
        // if subject still exists
        if (member != null)
        {
            Brain = member.GetComponent<AIData>();
            if (Brain != null)
            {
                // set player party
                if (player != null)
                {
                    PlayerData pd = player.GetComponent<PlayerData>();
                    pd.party.Remove(member);
                    Debug.Log(member.name + " : has left the party");
                }
                else player = GameObject.FindGameObjectWithTag("Player");
                // set mind set
                Brain.CommandCompleted = true;
                Brain.path = null;
            }
        }
    }

    private void ClearParty()
    {
        // set player party
        if (player != null)
        {
            PlayerData pd = player.GetComponent<PlayerData>();
            foreach (GameObject member in pd.party)
            {
                Brain = member.GetComponent<AIData>();
                if (Brain != null)
                {
                    // set mind set
                    Brain.CommandCompleted = true;
                    Brain.path = null;
                }
            }

            pd.party.Clear();

            Debug.Log("Party has been cleared!");
        }
        else player = GameObject.FindGameObjectWithTag("Player");
    }

    #endregion


    #region Set Most Relevenat using Click
    public void SetMostRelevant()
    {
        // null reset
        nearestFruitant = null;
        nearestDrone = null;
        nearestTileNode = null;
        nearestFood = null;

        float minToFruit = Mathf.Infinity;
        float minToDrone = Mathf.Infinity;
        float minToNode = Mathf.Infinity;
        float minToFood = Mathf.Infinity;

        // navigate the last clicked on:
        float dist;
        foreach (GameObject member in cross.GetComponent<CrosshairController>().lastClickedOn)
        {
            dist = Vector2.Distance(this.transform.position, member.transform.position);
            // check each and set when needed
            if (member.GetComponent<AIData>() != null)
            {
                if (member.tag == "Prey")
                {
                    // fruitant
                    if (dist < minToFruit)
                    {
                        minToFruit = dist;
                        nearestFruitant = member;
                    }
                }
                else
                {
                    // drone
                    if (dist < minToDrone)
                    {
                        minToDrone = dist;
                        nearestDrone = member;
                    }
                }
            }
            else if (member.GetComponent<TileNode>() != null)
            {
                // tile node
                if (dist < minToNode)
                {
                    minToNode = dist;
                    nearestTileNode = member;
                }
            }
            else if (member.GetComponent<SkewerableObject>() != null)
            {
                // tile node
                if (dist < minToFood)
                {
                    minToFood = dist;
                    nearestFood = member;
                }
            }
        }

        Debug.Log("Set Most Relevant Objects: "
                + "=== Fruitant: " + (nearestFruitant != null ? nearestFruitant.name : "null")
                + "=== Drone: " + (nearestDrone != null ? nearestDrone.name : "null")
                + "=== TileNode: " + (nearestTileNode != null ? nearestTileNode.name : "null")
                + "=== Food: " + (nearestFood != null ? nearestFood.name : "null"));
    }

    #endregion

    #region Giving Commands
    public void SpecificCommand()
    {
        // act upon the selected
        if (lastSelected != null)
        {
            Verb = AIData.Protocols.Lazy;
            Object = nearestTileNode;
            Location = Cursor.transform.position;
            ObjectCriteria = Criteria.None;
            Debug.Log("Issuing Command: " + null + " " + Verb + ": crit-- " + ObjectCriteria + ", obj-- " + Object + ", loc-- " + Location);
            Command(null, Verb, Object, Location);

            lastSelected = null;
        }
        else Debug.Log(this.name + " is giving a SPECIFIC COMMAND to a null agent!");
    }

    /// <summary>
    /// Apply commands to multiple agents
    /// </summary>
    /// <param name="subs">list of agents</param>
    /// <param name="verb">protocol</param>
    /// <param name="crit">criteria</param>
    /// <param name="obj">target</param>
    /// <param name="loc">location</param>
    public void GroupCommand(List<GameObject> subs, AIData.Protocols verb, Criteria crit, GameObject obj, Vector3 loc)
    {
        if (crit == Criteria.None)
        {
            foreach (GameObject agent in subs)
            {
                Command(agent, verb, obj, loc);
            }
        }
        else
        {
            foreach (GameObject agent in subs)
            {
                Command(agent, verb, crit);
            }
        }
    }

    /// <summary>
    /// Apply commands to multiple families
    /// </summary>
    /// <param name="subs">list of agents</param>
    /// <param name="verb">protocol</param>
    /// <param name="crit">criteria</param>
    /// <param name="obj">target</param>
    /// <param name="loc">location</param>
    public void FamilyCommand(string fam, AIData.Protocols verb, Criteria crit, GameObject obj, Vector3 loc)
    {
        if (Families.ContainsKey(fam))
        {
            GameObject go = Families[fam];
            List<GameObject> subjects = new List<GameObject>();
            foreach(Transform child in go.transform)
            {
                subjects.Add(child.gameObject);
            }

            GroupCommand(subjects, verb, crit, obj, loc);
        }
    }

    /// <summary>
    /// Set the protocol of an agent with criteria
    /// </summary>
    /// <param name="sub">the agent</param>
    /// <param name="verb">the protocol</param>
    /// <param name="obj">the criteria</param>
    public void Command(GameObject sub, AIData.Protocols verb, Criteria obj)
    {
        // if subject still exists
        if (sub != null)
        {
            Brain = sub.GetComponent<AIData>();
            if (Brain != null)
            {
                // set protocol
                Brain.currentProtocol = verb;
                // set specials
                GameObject go = ParseObjectCriteria(sub, obj);
                if (go != null)
                {
                    //set internals
                    this.Object = go;
                    this.Location = go.transform.position;
                    // set inside brain
                    Brain.Checks.specialTarget = Object;
                    Brain.Checks.specialPosition = Location;
                }
                Brain.path = null;
            }
        }
    }

    /// <summary>
    /// Set the protocol of an agent with criteria
    /// If obj == null, loc = obj.position, else default to loc
    /// </summary>
    /// <param name="sub">the agent</param>
    /// <param name="verb">the protocol</param>
    /// <param name="obj">specific object</param>
    /// <param name="loc">specific location</param>
    public void Command(GameObject sub, AIData.Protocols verb, GameObject obj, Vector3 loc)
    {
        // if subject still exists
        if (sub != null)
        {
            Brain = sub.GetComponent<AIData>();
            if (Brain != null)
            {
                // set protocol
                Brain.currentProtocol = verb;
                // set specials
                //set internals
                this.Object = obj;
                this.Location = (loc!=Vector3.zero ? loc : obj.transform.position);
                // set inside brain
                Brain.Checks.specialTarget = this.Object;
                Brain.Checks.specialPosition = this.Location;
                // reset pathfinder
                Brain.path = null;
            }
        }
    }


    /// <summary>
    /// Using the Command object
    /// </summary>
    /// <param name="c">the command</param>
    public void Command(Command c)
    {
        // if subject still exists
        if (c.Subject != null)
        {
            Brain = c.Subject.GetComponent<AIData>();
            if (Brain != null)
            {
                // set protocol
                Brain.currentProtocol = c.Verb;
                // set specials
                //set internals
                this.Object = c.Object;
                this.Location = (this.Object == null ? c.Location : (Vector2)this.Object.transform.position);

                // set inside brain
                Brain.Checks.specialTarget = this.Object;
                Brain.Checks.specialPosition = this.Location;
                Brain.path = null;
            }
        }
    }
    

    #endregion

    #region Misc Helpers
    /// <summary>
    /// Changes the current command verb
    /// Currently --> Chase > Lazy > Attack > Wander >>
    /// </summary>
    private void CycleTargetVerb()
    {
        if (Verb == AIData.Protocols.Chase)
        {
            Verb = AIData.Protocols.Lazy;
        }
        else if (Verb == AIData.Protocols.Lazy)
        {
            Verb = AIData.Protocols.Attack;
        }
        else if (Verb == AIData.Protocols.Attack)
        {
            Verb = AIData.Protocols.Wander;
        }
        else
        {
            Verb = AIData.Protocols.Chase;
        }
    }

    /// <summary>
    /// Based on criteria, find the appropriate agent
    /// </summary>
    /// <param name="sub"></param>
    /// <param name="crit"></param>
    /// <returns>gameobject of the proper criteria</returns>
    GameObject ParseObjectCriteria(GameObject sub, Criteria crit)
    {
        if (crit == Criteria.NearestEnemy)
            return Brain.Checks.ClosestCreature(new string[] { (sub.tag == "Prey" ? "Prey" : "Predator") });
        else if (crit == Criteria.NearestFriend)
            return Brain.Checks.ClosestCreature(new string[] { (sub.tag == "Prey" ? "Predator" : "Prey") });
        else
            return null;
    }
    #endregion
}
