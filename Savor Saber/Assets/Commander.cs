using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour
{
    // default commands
    // subjects being commanded
    public List<GameObject> Subjects;
    public List<GameObject> FamilyTrees;
    private Dictionary<string, GameObject> Families;
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
    public Vector2 Location;

    // AI specific knowledge
    AIData Brain;
    // player ref
    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        // give the Commander knowledge of the groups
        // of agents that can be commanded all at once
        Families = new Dictionary<string, GameObject>();
        foreach(GameObject fam in FamilyTrees)
        {
            Families.Add(fam.name, fam);
        }

        CycleTargetFamily(0);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // CYCLE SETTINGS OF COMMANDS
        if (Input.GetKeyDown(KeyCode.O))
        {
            CycleTargetFamily();
        }

        // PRESS ENTER TO ISSUE COMMAND
        if (Input.GetKeyDown(KeyCode.J))
        {
            Verb = AIData.Protocols.Chase;
            ObjectCriteria = Criteria.None;
            Object = GameObject.FindGameObjectWithTag("Player");
            Debug.Log("Issuing Command: " + FamilyChoice + " " + Verb + ": crit-- " + ObjectCriteria + ", obj-- " + Object + ", loc-- " + Location);
            GroupCommand(player.GetComponent<PlayerData>().party, Verb, ObjectCriteria, Object, Location);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            Verb = AIData.Protocols.Lazy;
            ObjectCriteria = Criteria.None;
            Debug.Log("Issuing Command: " + FamilyChoice + " " + Verb + ": crit-- " + ObjectCriteria + ", obj-- " + Object + ", loc-- " + Location);
            GroupCommand(player.GetComponent<PlayerData>().party, Verb, ObjectCriteria, Object, Location);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            Verb = AIData.Protocols.Attack;
            ObjectCriteria = Criteria.NearestEnemy;
            Debug.Log("Issuing Command: " + FamilyChoice + " " + Verb + ": crit-- " + ObjectCriteria + ", obj-- " + Object + ", loc-- " + Location);
            GroupCommand(player.GetComponent<PlayerData>().party, Verb, ObjectCriteria, Object, Location);
        }
    }

    /// <summary>
    /// Change the current family of agents by dir
    /// AND update Soma's party
    /// </summary>
    /// <param name="dir">number of indexes to move</param>
    private void CycleTargetFamily(int dir=1)
    {
        List<string> keys = new List<string>(Families.Keys);
        int newIndex = keys.IndexOf(FamilyChoice) + dir;
        if (newIndex < 0)
        {
            newIndex = Families.Count - 1;
        }
        else if (newIndex > Families.Count - 1)
        {
            newIndex %= Families.Count;
        }

        FamilyChoice = keys[newIndex];

        Debug.Log("Current Family = " + FamilyChoice + " => Soma's Party");

        // JOIN THE PARTY, BROS
        FamilyReunion();
    }

    /// <summary>
    /// Make the current family Soma's party
    /// </summary>
    private void FamilyReunion()
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

    public void JoinTeam(GameObject member)
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
                    pd.party.Add(member);
                }
                else player = GameObject.FindGameObjectWithTag("Player");
                // set mind set
                Brain.CommandCompleted = false;
                Brain.path = null;
            }
        }
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
        }
        else player = GameObject.FindGameObjectWithTag("Player");
    }

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
    /// Pass obj as null to make the location take effect
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
                this.Location = (obj==null ? loc : obj.transform.position);
                // set inside brain
                Brain.Checks.specialTarget = this.Object;
                Brain.Checks.specialPosition = this.Location;
                
                Brain.path = null;
            }
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
}
