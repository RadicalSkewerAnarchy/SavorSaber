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
    private bool activate = true;
    // if both hit enemies and hit friends are false, hit EVERYTHING
    bool hitSelf = true;
    bool hitEnemies = true;
    bool hitFriends = true;
    // Game object of who made this
    GameObject signalMaker = null;

    // Constructor
    // signalmaker, radius, moods, hitself, hitenemies, hitfriends
    public SignalApplication(GameObject signalMaker, float radius, Dictionary<string, float> moods, bool hitself, bool hitenemies, bool hitfriends)
    {
        this.signalMaker = signalMaker;
        this.interactRadius = radius;
        this.moodMod = moods;
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
        Debug.Log("signal with radius = " + interactRadius);
    }

    private void Update()
    {
        // if data is being requested,
        // then obtain hit list after
        // instantiation
        if (activate)
        {
            // apply
            ApplyToAll();
            // destroy
            Destroy(this.gameObject);
            
        }
    }

    // COLLECT THE HIT LIST
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // get objects
        GameObject go = collision.gameObject;
        // if not in list...
        if (!hitList.Contains(go))
        {
            // if someone made the signal...
            if (signalMaker != null)
            {
                // extract signal maker data and lists
                AIData data = signalMaker.GetComponent<AIData>();// get lists
                List<GameObject> fr = data.Friends;
                List<GameObject> en = data.Enemies;
                // create boolean cases
                bool hitF = fr.Contains(go) && hitFriends;
                bool hitE = en.Contains(go) && hitEnemies;
                bool hitS = hitSelf && go == signalMaker;
                bool hitA = !hitEnemies && !hitFriends && hitSelf;
                // if ANY of these...
                if (hitF || hitE || hitS || hitA)
                {
                    // add to list
                    hitList.Add(go);
                }
            }
            else
            {
                // do nothing
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
                Debug.Log(g + "'s " + key + " value should be " + value);
            }
        }
        // set decision timer to 0
        data.ManualDecision();
    }
}