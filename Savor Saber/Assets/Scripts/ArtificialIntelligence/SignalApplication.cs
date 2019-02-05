using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class SignalApplication : MonoBehaviour
{
    // variables
    [SerializeField]
    [Range(0f, 10f)]
    public float interactRadius;

    // hit and mod list
    [SerializeField]
    public List<GameObject> hitList = new List<GameObject>();
    public Dictionary<string, float> moodMod = new Dictionary<string, float>();

    [Range(-1f, 1f)]
    public float fearMod = 0;
    [Range(-1f, 1f)]
    public float hungerMod = 0;
    [Range(-1f, 1f)]
    public float hostileMod = 0;
    [Range(-1f, 1f)]
    public float friendMod = 0;

    // private variables
    private bool activate = true;

    // Start is called before the first frame update
    void Start()
    {
        // update radius
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.radius = interactRadius;
        Debug.Log("signal with radius = " + interactRadius);

        // modifications?
        moodMod.Add("Fear", fearMod);
        moodMod.Add("Hunger", hungerMod);
        moodMod.Add("Hostility", hostileMod);
        moodMod.Add("Friendliness", friendMod);
    }

    private void Update()
    {
        // if data is being requested,
        // then obtain hit list after
        // instantiation
        if (activate)
        {
            // ensure proper values
            moodMod["Fear"] = fearMod;
            moodMod["Hunger"] = hungerMod;
            moodMod["Hostility"] = hostileMod;
            moodMod["Friendliness"] = friendMod;
            // apply
            ApplyToAll();
            // destroy
            Destroy(this.gameObject);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        //Debug.Log("Object Found: " + go);
        if (!hitList.Contains(go))
        {
            Debug.Log("Found Object Not in List");
            AIData data = go.GetComponent<AIData>();
            if (data != null)
            {
                Debug.Log("---NPC Added");
                hitList.Add(go);
            }
        }
    }

    // ApplyToAll
    private void ApplyToAll()
    {
        foreach (GameObject g in hitList)
        {
            Apply(g);
        }
    }

    // Apply
    // Override this given the type of 
    // signal that needs to be applied
    private void Apply(GameObject g)
    {
        AIData data = g.GetComponent<AIData>();
        //modification
        foreach(string key in moodMod.Keys)
        {
            // using the keys, change the values
            // of "moods" in data
            float mod = moodMod[key];
            if (mod > 0)
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