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

        // modifications?
        moodMod.Add("Fear", fearMod);
        moodMod.Add("Hunger", hungerMod);
        moodMod.Add("Hostility", hostileMod);
        moodMod.Add("Friendliness", friendMod);
    }

    private void Update()
    {
        if (activate)
        {
            ApplyToAll();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        //Debug.Log("Object Found: " + go);
        if (!hitList.Contains(go))
        {
            //Debug.Log("---Object Not in List");
            if (go.GetComponent<AIData>() != null)
            {
                //Debug.Log("------Object Added");
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
        foreach(string key in moodMod.Keys)
        {
            // using the keys, change the values
            // of "moods" in data
            float value = data.moods[key];
            value = Mathf.Clamp((value + moodMod[key]), 0f, 1f);
            data.moods[key] = value;
        }
    }
}