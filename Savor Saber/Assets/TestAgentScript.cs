using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAgentScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {        
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = new Vector3(5f,5f,5f);        
    }
}
