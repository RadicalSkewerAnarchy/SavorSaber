using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanaceaMover : PoweredObject
{

    private bool playerNearby = false;
    [SerializeField]
    private Commander AICommander;
    [SerializeField]
    private GameObject escortTarget;
    private AIData escortData;

    public GameObject waypoint;

    private BasicCharacterController panaceaController;

    // Start is called before the first frame update
    void Start()
    {
        if (AICommander == null)
            AICommander = GameObject.Find("Gaia").GetComponent<Commander>();

        escortData = escortTarget.GetComponent<AIData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TurnOn()
    {
        base.TurnOn();
        if (playerNearby)
            StartMotion();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerNearby = true;
            if(active)
                StartMotion();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerNearby = false;
            if (active)
                StopMotion();
        }
    }

    public void StartMotion()
    {
        Debug.Log("Escort quest: Starting motion");
        AICommander.Command(escortTarget, AIData.Protocols.Chase, waypoint, waypoint.transform.position);
        escortData.Speed = 1.75f;
    }
    public void StopMotion()
    {
        Debug.Log("Escort quest: Stopping motion");
        AICommander.Command(escortTarget, AIData.Protocols.Lazy, waypoint, waypoint.transform.position);
        escortData.Speed = 0;

    }
}
