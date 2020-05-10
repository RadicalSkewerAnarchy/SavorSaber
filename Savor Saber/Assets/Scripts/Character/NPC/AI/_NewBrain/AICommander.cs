using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICommander : MonoBehaviour
{
    public GameObject CommandableState;
    // default commands
    // subjects being commanded
    public List<GameObject> Subjects;
    private CrosshairController cross;

    // the protocol to follow
    public AIData.Protocols Verb;
    
    public GameObject Object;
    public GameObject Cursor;
    public Vector2 Location;

    private Queue<GameObject> lastSelected = new Queue<GameObject>();
    private GameObject[] selectedFruit = { null, null };
    private GameObject[] selectedDrone = { null, null };
    private GameObject[] selectedNode = { null, null };
    private GameObject[] selectedFood = { null, null };
    private GameObject nearestFruitant;
    private GameObject nearestDrone;
    private GameObject nearestTileNode;
    private GameObject nearestFood;


    // Start is called before the first frame update
    void Start()
    {
        if (CommandableState.GetComponent<AIStateCommandable>() == null)
            Debug.Log(this.name + " needs a commandable state to pass");

        cross = GameObject.FindObjectOfType<CrosshairController>();
        if (Cursor==null)Debug.Log(this.name + " NEEDS A REFERENCE TO THE PLAYER'S CURSOR! in the inspector");

        Debug.Log($"AICommander {this.name} has been awoken");
        
    }

    // Update is called once per frame
    void LateUpdate()
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
                if (Subjects.Contains(nearestFruitant))
                {
                    SelectEnqueue(nearestFruitant);
                    //nearestFruitant.GetComponent<Squeezer>().Wiggle(1, 5, 5, 0.1f, 0.1f);
                    Cursor.GetComponent<CrosshairClicker>().PlaySelectionSounds(lastSelected.Count);
                }
            }
            else if (lastSelected.Count == 1)
            {
                // check if there is a closest party member
                if (nearestDrone != null)
                {
                    SelectEnqueue(nearestDrone);
                    //nearestDrone.GetComponent<Squeezer>().Wiggle(1, 10, 10, 0.05f, 0.05f);
                    Cursor.GetComponent<CrosshairClicker>().PlaySelectionSounds(3);
                }
                else if (Subjects.Contains(nearestFruitant))
                {
                    // remove current selected fruitant
                    //lastSelected.Dequeue();
                    SelectEnqueue(nearestFruitant);
                    //nearestFruitant.GetComponent<Squeezer>().Wiggle(1, 5, 5, 0.1f, 0.1f);
                    Cursor.GetComponent<CrosshairClicker>().PlaySelectionSounds(lastSelected.Count);
                }
                else if (nearestTileNode != null)
                {
                    // go to tile node
                    SelectEnqueue(nearestTileNode);
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
                GameObject target = lastSelected.Dequeue();
                Debug.Log($"Click Command: Member {applyCommandTo.name}, Target {target.name}");
                // if FRUIT
                if (applyCommandTo.CompareTag("Prey"))
                {
                    if (target.CompareTag("Prey"))
                    {
                        Command(applyCommandTo, AIData.Protocols.Chase, target); //, target.transform.position);
                    }
                    else if (target.CompareTag("Predator"))
                    {
                        Command(applyCommandTo, AIData.Protocols.Attack, target);//, target.transform.position);
                    }
                    else // is tilenode
                    {
                        Command(applyCommandTo, AIData.Protocols.Runaway, target);//, Cursor.transform.position);
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
                Object = GameObject.FindGameObjectWithTag("Player");
                Debug.Log("Issuing Command: " +  Verb  + ", obj-- " + Object + ", loc-- " + Location);
                GroupCommand(Subjects, Verb, Object, Location);
            }
            // IDLE
            else if (in1)
            {
                // on first Lazy call: continue where you were going
                // on second: stop and wait in place
                if (Verb != AIData.Protocols.Lazy)
                {
                    Verb = AIData.Protocols.Lazy;
                    Location = Vector2.zero;
                    Debug.Log("Issuing Command: " + Verb  + ", obj-- " + Object + ", loc-- " + Location);
                    GroupCommand(Subjects, Verb, null, Location);
                }
            }
            // ATTACK
            else if (in3)
            {
                Verb = AIData.Protocols.Attack;
                Object = null;
                Debug.Log("Issuing Command: " + Verb  + ", obj-- " + Object + ", loc-- " + Location);
                GroupCommand(Subjects, Verb, Object, Location);
            }
        }
    }

    void SelectEnqueue(GameObject go)
    {
        lastSelected.Enqueue(go);
    }


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
        var collection = cross.GetComponent<CrosshairController>().lastClickedOn;
        foreach (GameObject member in collection)
        {
            dist = Vector2.Distance(this.transform.position, member.transform.position);
            // check each and set when needed
            if (member.GetComponent<AICharacterData>() != null || member.GetComponent<AIData>() != null)
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

        Debug.Log("Set Most Relevant Objects for Command: "
                + "=== Fruitant: " + (nearestFruitant != null ? nearestFruitant.name : "null")
                + "=== Drone: " + (nearestDrone != null ? nearestDrone.name : "null")
                + "=== TileNode: " + (nearestTileNode != null ? nearestTileNode.name : "null")
                + "=== Food: " + (nearestFood != null ? nearestFood.name : "null"));
    }


    public void SpecificCommand()
    {
        // act upon the selected
        if (lastSelected != null)
        {
            Verb = AIData.Protocols.Lazy;
            Object = nearestTileNode;
            Location = Cursor.transform.position;
            Debug.Log("Issuing Command: " + null + " " + Verb  + ", obj-- " + Object + ", loc-- " + Location);
            Command(null, Verb, Object);

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
    public void GroupCommand(List<GameObject> subs, AIData.Protocols verb, GameObject obj, Vector3 loc)
    {
        foreach (GameObject agent in subs)
        {
            Command(agent, verb, obj);
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
    public void Command(GameObject sub, AIData.Protocols verb, GameObject obj)
    {
        // if subject still exists
        if (sub != null)
        {
            AIBrain Brain = sub.GetComponent<AICharacterData>().Brain;
            if (Brain != null)
            {
                // set commandable state
                if (Brain.CurrentState is AIStateCommandable)
                {
                    (Brain.CurrentState as AIStateCommandable).SetCommand(verb);
                }
                // set specials
                if (obj != null)
                {
                    //set internals
                    this.Object = obj;
                    this.Location = obj.transform.position;
                    // set inside brain
                    (Brain.CurrentState as AIStateCommandable).SetTarget(Object);
                }
                else
                {
                    (Brain.CurrentState as AIStateCommandable).SetTarget(null);
                }
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
            AIBrain Brain = c.Subject.GetComponent<AICharacterData>().Brain;
            if (Brain != null)
            {
                // set commandable state
                if (Brain.CurrentState is AIStateCommandable)
                {
                    (Brain.CurrentState as AIStateCommandable).SetCommand(c.Verb);
                }
                // set specials
                if (c.Object != null)
                {
                    //set internals
                    this.Object = c.Object;
                    this.Location = c.Location;
                    // set inside brain
                    (Brain.CurrentState as AIStateCommandable).SetTarget(Object);
                }
                else
                {
                    (Brain.CurrentState as AIStateCommandable).SetTarget(null);
                }
                Brain.path = null;
            }
        }
    }
}
