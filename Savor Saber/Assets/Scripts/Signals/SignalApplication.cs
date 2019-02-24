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
    public List<GameObject> dropList = new List<GameObject>();
    public Dictionary<string, float> moodMod;
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
    public bool activate = false;
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
        //Debug.Log("Signal Created: rad = " + radius + ", x = " + transform.position.x + ", y = " + transform.position.y + "(" + moods["Friendliness"] + ")");
        this.signalMaker = signalMaker;
        this.interactRadius = radius;
        // update radius
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.radius = interactRadius;
        this.moodMod = moods;
        this.hitAll = hitall;
        this.hitSelf = hitself;
    }


    // Start is called before the first frame update
    void Start()
    {
        // update radius
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.radius = interactRadius;
        //AnimationBody = GetComponent<Animator>();
        //ChildAnimationAgent.AddComponent<Animator>();
        //Debug.Log("signal with radius = " + interactRadius);
    }

    public void Update()
    {
        // if data is being requested,
        // then obtain hit list after
        // instantiation
        if (activate)
        {
            // apply
            ApplyToAll();
            // inform signal maker of those here
            if (signalMaker != null && !ReferenceEquals(signalMaker, null))
            {
                signalMaker.GetComponent<MonsterChecks>().AllCreatures = hitList;
                //Debug.Log(signalMaker.gameObject.name + " number surrounded by " + hitList.Count);
                signalMaker.GetComponent<MonsterChecks>().AllDrops = dropList;
                signalMaker.GetComponent<AIData>().Awareness = null;
            }
            // destroy
            Destroy(this.gameObject, Time.fixedDeltaTime);
        }
        else
        {
            activate = true;
        }
    }

    // COLLECT THE HIT LIST
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // get objects
        GameObject go = collision.gameObject;

        string sm = (signalMaker != null ? signalMaker.name : "null character" );
        if(interactRadius < 5)
            Debug.Log(sm + " has found --> " + go.name + " with signal of radius " + interactRadius);

        // check tags
        if (go.tag == "Prey" || go.tag == "Predator" || go.tag == "Player")
        {
            //Debug.Log(go.name + "is tagged properly --> " + go.tag);

            // create boolean cases
            bool hitS = hitSelf && this.Equals(go);
            bool hitA = hitAll && !hitS;

            // if ANY of these...
            if (hitA || hitS)
            {
                // add to list
                //Debug.Log(sm + "'s HIT LIST ++ --> " + go.name);
                //Debug.Log("Compare: (true plz) " + signalMaker.ToString().Equals(go.ToString()));
                hitList.Add(go);
            }
        }
        else if (go.tag == "SkewerableObject")
        {
            // keep track of drops
            dropList.Add(go);
        }
    }

    // ApplyToAll
    private void ApplyToAll()
    {
        foreach (GameObject g in hitList)
        {
            //Debug.Log("APPLY TO HIT: " + g.name);
            if (g != null && g.tag != "Player")
            {
                // get data
                AIData data = g.GetComponent<AIData>();
                // apply
                Apply(g, data);
            }
        }
    }

    // Apply
    // Override this given the type of 
    // signal that needs to be applied
    private void Apply(GameObject g, AIData data)
    {
        //Debug.Log("Applying signal to " + g);
        //modification  
        #region SignalAnimations
        string mood = null;
        float mostInfluential = 0;
        int sign = 0;
        #endregion
        foreach (string key in moodMod.Keys)
        {
            // using the keys, change the values
            // of "moods" in data
            float mod = moodMod[key];
            //Debug.Log("Mod of signal is: key, mod" + key + ", " + mod);
            if (mod != 0)
            {
                if(Mathf.Abs(mod) >= Mathf.Abs(mostInfluential)) { mostInfluential = mod; mood = key; sign = (int)(mod / Mathf.Abs(mod)); }
                //Debug.Log("Signal Animator(mostInfluential, mood, sign) : (" + mostInfluential + ", " + mood + ", " + sign + ")");
                float value = data.moods[key];
                value = Mathf.Clamp((value + mod), 0f, 1f);
                data.moods[key] = value;
                //Debug.Log(g + "'s " + key + " value should be " + value);
            }
        }
        //change middle argument based on creatures offset
        GameObject child = null;
        if (signalMaker == null)
            SignalAnimator(mostInfluential, mood, sign, child, g);
        // child.GetComponent<Animator>().Play("FearUpAnimation");

        // set decision timer to 0
        // THIS MAKES THEM THINK WAYYYYYYY TOO FAST
        // data.ManualDecision();
    }
    private void SignalAnimator(float mostInfluential, string mood, int sign, GameObject emoter, GameObject parent)
    {
        emoter = Instantiate(ChildAnimationAgent, parent.transform.position + new Vector3(0f, 1.2f, 0f), Quaternion.identity, parent.transform);
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
                emoter.GetComponent<Animator>().Play("HungerDownAnimation");
            }
        }else{
            Debug.Log("Destroying emoter");
            Destroy(emoter);
        }        
    }    
}