using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterData))]
public class HealthGatedEvent : MonoBehaviour
{

    //other components
    private CharacterData charData;

    //fields
    public int[] healthGates;
    public UnityEvent[] events;
    private int gateProgress = 0;


    // Start is called before the first frame update
    void Start()
    {
        charData = GetComponent<CharacterData>();
        CheckHealthGateValidity();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckGateProgress(int health)
    {
        Debug.Log("Entering CheckGateProgress");
        if(healthGates.Length != events.Length)
        {
            Debug.LogError("Warning: " + this.gameObject.name + " has an inequal number of health gates and events. Events will not fire.");
            return;
        }

        for(int i = 0; i < healthGates.Length; i++)
        {
            if(gateProgress <= i && health <= healthGates[i])
            {
                if(events[i] != null) events[i].Invoke();
                Debug.Log(this.gameObject.name + " firing event " + i);
                gateProgress++;
            }
        }
    }

    private void CheckHealthGateValidity()
    {
        foreach(int gate in healthGates)
        {
            if (gate > charData.maxHealth) Debug.LogError("Warning: Health gate exceeds maximum health of " + this.gameObject.name + ". Event will not fire.");
        }
    }
}
