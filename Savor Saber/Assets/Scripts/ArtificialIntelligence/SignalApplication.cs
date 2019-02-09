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

    // look at aidata


    // private variables
    public bool activate = false;
    // if both hit enemies and hit friends are false, hit EVERYTHING
    bool hitSelf = true;
    bool hitAll = true;
    // Game object of who made this
    GameObject signalMaker = null;

    // Constructor
    // signalmaker, radius, moods, hitself, hitenemies, hitfriends
    public void SetSignalParameters(GameObject signalMaker, float radius, Dictionary<string, float> moods, bool hitall, bool hitself)
    {
        Debug.Log("Signal Created via Constructor");
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
            if (signalMaker != null)
            {
                signalMaker.GetComponent<MonsterChecks>().AllCreatures = hitList;
                Debug.Log(signalMaker.gameObject.name + " number surrounded by " + hitList.Count);
                signalMaker.GetComponent<MonsterChecks>().AllDrops = dropList;
                signalMaker.GetComponent<AIData>().Awareness = null;
            }

            // destroy
            Destroy(this.gameObject);
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
        Debug.Log(signalMaker.name + "has found --> " + go.name);
        AIData maindata = go.GetComponent<AIData>();
        // 11 is monster layer
        if (go.layer == 11)
        {
            // extract signal maker data and lists
            AIData data = signalMaker.GetComponent<AIData>();// get lists

            // create boolean cases
            bool hitS = hitSelf && data.Equals(maindata);
            bool hitA = hitAll;

            // if ANY of these...
            if (hitA || hitS)
            {
                // add to list
                Debug.Log(signalMaker.name + "'s HIT LIST ++ --> " + go.name);
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
            // get data
            AIData data = g.GetComponent<AIData>();
            // apply
            Apply(g, data);
        }
    }

    // Apply
    // Override this given the type of 
    // signal that needs to be applied
    private void Apply(GameObject g, AIData data)
    {
        //modification
        foreach (string key in moodMod.Keys)
        {
            // using the keys, change the values
            // of "moods" in data
            float mod = moodMod[key];
            if (mod != 0)
            {
                float value = data.moods[key];
                value = Mathf.Clamp((value + mod), 0f, 1f);
                data.moods[key] = value;
                //Debug.Log(g + "'s " + key + " value should be " + value);
            }
        }
        // set decision timer to 0
        // THIS MAKES THEM THINK WAYYYYYYY TOO FAST
        // data.ManualDecision();
    }
}