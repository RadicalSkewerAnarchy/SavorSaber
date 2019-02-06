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
    public bool activate = true;
    // if both hit enemies and hit friends are false, hit EVERYTHING
    bool hitSelf = true;
    bool hitAll = true;
    bool hitEnemies = true;
    bool hitFriends = true;
    // Game object of who made this
    GameObject signalMaker = null;

    // Constructor
    // signalmaker, radius, moods, hitself, hitenemies, hitfriends
    public void SetSignalParameters(GameObject signalMaker, float radius, Dictionary<string, float> moods, bool hitall, bool hitself, bool hitenemies, bool hitfriends)
    {
        this.signalMaker = signalMaker;
        this.interactRadius = radius;
        this.moodMod = moods;
        this.hitAll = hitall;
        this.hitSelf = hitself;
        this.hitEnemies = hitenemies;
        this.hitFriends = hitfriends;
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
            }

            // destroy
            Destroy(this.gameObject);
        }
        else
        {
            //activate = true;
        }
    }

    // COLLECT THE HIT LIST
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // get objects
        GameObject go = collision.gameObject;
        AIData maindata = go.GetComponent<AIData>();
        if (maindata != null)
        {
            // if not in list...
            if (!hitList.Contains(go))
            {
                // extract signal maker data and lists
                AIData data = signalMaker.GetComponent<AIData>();// get lists
                List<GameObject> fr = data.Friends;
                List<GameObject> en = data.Enemies;

                // create boolean cases
                bool hitF = fr.Contains(go) && hitFriends;
                //Debug.Log("hit friends?: " + hitF + " who?: " + go.name);
                bool hitE = en.Contains(go) && hitEnemies;
                //Debug.Log("hit enemies?: " + hitE + " who?: " + go.name);
                bool hitS = hitSelf && data.Equals(maindata);
                //Debug.Log("hit self?: " + hitS + " who?: " + go.name);
                bool hitA = hitAll;
                //Debug.Log("hit ALL?: " + hitA + " who?: " + go.name);

                // if ANY of these...
                if (hitA || (hitF || hitE) || hitS)
                {
                    // add to list
                    //Debug.Log(signalMaker.name + "'s HIT LIST ++ : " + go.name);
                    //Debug.Log("Compare: (true plz) " + signalMaker.ToString().Equals(go.ToString()));
                    hitList.Add(go);
                }
                else
                {
                    // do nothing
                }
            }
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