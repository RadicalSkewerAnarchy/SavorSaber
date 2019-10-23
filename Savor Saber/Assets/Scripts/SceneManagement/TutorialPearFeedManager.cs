using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPearFeedManager : MonoBehaviour
{
    private int food;
    [SerializeField]
    private int maxFood;

    public EventTrigger scene;
    public ExtendingBridge bridge;

    [Header("Party management references")]
    public Commander partyCommander;
    public GameObject[] fruitantsToAdd;
    public GameObject player;

    private Commander.Criteria ObjectCriteria = Commander.Criteria.None;
    private GameObject Object;
    private AIData.Protocols Verb;
    private Vector2 Location = Vector2.zero
        ;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = GameObject.Find("Soma");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Feed(int numFood)
    {
        food += numFood;
        if(food >= maxFood)
        {
            //Debug.Log("Fed max food, triggering scene");
            scene.Trigger();
            FlagManager.SetFlag("PearCheck", "true");
            bridge.TurnOn();
            
            foreach(GameObject fruitant in fruitantsToAdd)
            {
                partyCommander.JoinTeam(fruitant); 
            }
            //partyCommander.FamilyReunion();
        }
    }

    public void SetFollow()
    {
        Verb = AIData.Protocols.Chase;
        ObjectCriteria = Commander.Criteria.None;
        Object = GameObject.FindGameObjectWithTag("Player");       
        partyCommander.GroupCommand(player.GetComponent<PlayerData>().party, Verb, ObjectCriteria, Object, Location);
    }


}
