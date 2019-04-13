using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class SignalApplication : MonoBehaviour
{
    // variables
    [SerializeField]
    public float interactRadius;

    // hit and mod list
    [SerializeField]
    public List<GameObject> hitList = new List<GameObject>();
    public List<GameObject> plantList = new List<GameObject>();
    public List<GameObject> dropList = new List<GameObject>();
    public Dictionary<string, float> moodMod = new Dictionary<string, float>();
    #region MoodMods
    [Range(-1f, 1f)]
    public float fearMod = 0;
    [Range(-1f, 1f)]
    public float hungerMod = 0;
    [Range(-1f, 1f)]
    public float hostileMod = 0;
    [Range(-1f, 1f)]
    public float friendMod = 0;
    #endregion

    // private variables
    public bool hasActivated = false;
    public bool isForAwareness = false;
    // if both hit enemies and hit friends are false, hit EVERYTHING
    bool hitSelf = true;
    bool hitAll = true;
    // Game object of who made this
    public GameObject signalMaker = null;

    //Animator AnimationBody;

    public GameObject ChildAnimationAgent;

    // Constructor
    // signalmaker, radius, moods, hitself, hitenemies, hitfriends
    public void SetSignalParameters(GameObject signalMaker, float radius, Dictionary<string, float> moods, bool hitall, bool hitself)
    {
        this.signalMaker = signalMaker;
        this.interactRadius = radius;
        // update radius
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.radius = interactRadius;
        this.moodMod = moods;
        this.hitAll = hitall;
        this.hitSelf = hitself;

        // set whether or not this is for awareness:
        //      if the dictionary is empty,
        //      it is for awareness
        isForAwareness = (this.moodMod.Count == 0);

        //Debug.Log("Signal Created: is " + (isForAwareness?"":"NOT ") + "for awareness, x = " + transform.position.x + ", y = " + transform.position.y);
    }


    // Start is called before the first frame update
    void Awake()
    {
        // update radius
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.radius = interactRadius;
        //AnimationBody = GetComponent<Animator>();
        //ChildAnimationAgent.AddComponent<Animator>();
        //Debug.Log("signal with radius = " + interactRadius);
        gameObject.name = gameObject.name + gameObject.GetInstanceID().ToString();

        // set any default dictionary values
        if (fearMod != 0)
            moodMod.Add("Fear", fearMod);
        if (hungerMod != 0)
            moodMod.Add("Hunger", hungerMod);
        if (hostileMod != 0)
            moodMod.Add("Hostility", hostileMod);
        if (friendMod != 0)
            moodMod.Add("Friendliness", friendMod);
    }

    public void Activate()
    {
        var objects = Physics2D.OverlapCircleAll(transform.position, interactRadius);
        foreach (var go in objects)
        {
            string sm = (signalMaker != null ? signalMaker.name : "null character");
            //if(interactRadius < 5)
            //if (!isForAwareness)
            //Debug.Log(sm + " has found --> " + go.name + " with signal of radius " + interactRadius);

            // check tags
            if (go.tag == "Prey" || go.tag == "Predator" || go.tag == "Player")
            {
                //Debug.Log(go.name + "is tagged properly --> " + go.tag);

                // create boolean cases
                bool isMe = (signalMaker != null ? this.signalMaker.name.Equals(go.name) : false);

                // if ANY of these...
                if ((hitSelf && hitAll) || (isMe && hitSelf) || (hitAll && !isMe))
                {
                    // add to list
                    //Debug.Log(sm + "'s HIT LIST ++ --> " + go.name);
                    //Debug.Log("Compare: (true plz) " + signalMaker.ToString().Equals(go.ToString()));
                    hitList.Add(go.gameObject);
                    //Debug.Log("MY ID: " + signalMaker.GetInstanceID() + " CREATURE ADDED TO HITLIST ID: " + go.gameObject.GetInstanceID());
                }
            }
            else if (isForAwareness)
            {
                // keep track of drops
                if(go.tag == "SkewerableObject")
                    dropList.Add(go.gameObject);
                else if(go.tag == "ThrowThrough")
                    plantList.Add(go.gameObject);
            }
        }

        // APPLICATION
        if (!isForAwareness)
        {
            ApplyToAll();
        }
        // inform signal maker of those here
        else if (signalMaker != null)
        {
            {
                signalMaker.GetComponent<MonsterChecks>().AllCreatures = hitList;
                signalMaker.GetComponent<MonsterChecks>().AllPlants = plantList;
                signalMaker.GetComponent<MonsterChecks>().AllDrops = dropList;
                signalMaker.GetComponent<AIData>().Awareness = null;
            }
        }
        hasActivated = true;
        Destroy(gameObject, Time.fixedDeltaTime);
    }

    private void Start()
    {
        if (!hasActivated)
            Activate();
    }

    // ApplyToAll
    private void ApplyToAll()
    {
        foreach (GameObject g in hitList)
        {
            if (g != null)
            {
                if (g.tag != "Player")
                {
                    //Debug.Log("APPLY TO HIT: " + g.name);
                    // get data
                    AIData data = g.GetComponent<AIData>();
                    // apply
                    Apply(g, data);
                }
            }
        }
    }

    // Apply
    // Override this given the type of
    // signal that needs to be applied
    private void Apply(GameObject g, AIData data)
    {
        if (ReferenceEquals(g, null))
            return;

        //modification
        #region SignalAnimations
        string mood = null;
        float mostInfluential = 0;
        int sign = 0;
        #endregion
        if (moodMod != null)
        {
            // iterate thru all the keys
            foreach (string key in moodMod.Keys)
            {
                // extract modifier
                float mod = moodMod[key];
                //Debug.Log("Mod of signal is: key, mod" + key + ", " + mod);

                // as long is it's non zero....
                if (mod != 0)
                {
                    // update signal animation
                    if (Mathf.Abs(mod) >= Mathf.Abs(mostInfluential))
                    {
                        mostInfluential = mod;
                        mood = key;
                        sign = (int)Mathf.Sign(mod);
                    }
                    //Debug.Log("Signal Animator(mostInfluential, mood, sign) : (" + mostInfluential + ", " + mood + ", " + sign + ")");

                    float value = data.moods[key];
                    value = Mathf.Clamp((value + mod), 0f, 1f);
                    data.moods[key] = value;
                    //Debug.Log("--->" + g.name + "'s " + key + " value should be " + value);
                }
            }
        }
        else
        {
            //Debug.Log("****************Mood mod of this signal is null: " + this.gameObject.name);
        }
        //change middle argument based on creatures offset
        GameObject child = null;
        SignalAnimator(mostInfluential, mood, sign, child, g);

        // child.GetComponent<Animator>().Play("FearUpAnimation");

        // set decision timer to 0
        // THIS MAKES THEM THINK WAYYYYYYY TOO FAST
        // data.ManualDecision();
    }
    private void SignalAnimator(float mostInfluential, string mood, int sign, GameObject emoter, GameObject parent)
    {
        emoter = Instantiate(ChildAnimationAgent, parent.transform.position, Quaternion.identity, parent.transform);
        //Debug.Log("Signal Animator(mostInfluential, mood, sign) : (" + mostInfluential + ", " + mood + ", " + sign + ")");
        if(sign > 0)
        {
            if (mood == "Friendliness")
            {
                //Debug.Log("Setting animation to friendly");
                emoter.GetComponent<Animator>().Play("FriendUpAnimation");
            }
            else if (mood == "Hostility")
            {
                emoter.GetComponent<Animator>().Play("HostilityUpAnimation");
            }
            else if (mood == "Fear")
            {
                emoter.GetComponent<Animator>().Play("FearUpAnimation");
            }
            else if (mood == "Hunger")
            {

                //Debug.Log("Setting animation to FAMINE");
                emoter.GetComponent<Animator>().Play("HungerUpAnimation");
            }
        }
        else if(sign < 0)
        {
            if (mood == "Friendliness")
            {
                emoter.GetComponent<Animator>().Play("FriendDownAnimation");
            }
            else if (mood == "Hostility")
            {
                emoter.GetComponent<Animator>().Play("HostilityDownAnimation");
            }
            else if (mood == "Fear")
            {
                emoter.GetComponent<Animator>().Play("FearDownAnimation");
            }
            else if (mood == "Hunger")
            {

                //Debug.Log("YUMMY");
                emoter.GetComponent<Animator>().Play("HungerDownAnimation");
            }
        }
        else
        {
            //Debug.Log("Destroying emoter");
            Destroy(emoter);
        }
    }
}
