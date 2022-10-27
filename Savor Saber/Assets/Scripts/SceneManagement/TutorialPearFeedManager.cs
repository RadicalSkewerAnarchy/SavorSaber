using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPearFeedManager : MonoBehaviour
{
    public bool fruitantJoinsParty = false;
    private int food;
    [SerializeField]
    private int maxFood = 2;

    public EventTrigger scene;
    public ExtendingBridge bridge;

    [Header("Party management references")]
    public Commander partyCommander;
    public GameObject[] fruitantsToAdd;
    public GameObject player;

    private Commander.Criteria ObjectCriteria = Commander.Criteria.None;
    private GameObject Object;

    private AIData.Protocols Verb;
    private Vector2 Location = Vector2.zero;
    private Rigidbody2D fruitantRB;

    private bool active = true; 

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
    
    public void Activate()
    {
        active = true;
    }

    public void Deactivate()
    {
        active = false;
    }

    public void Feed(int numFood)
    {
        if (!active) return;

        food += numFood;
        if(food >= maxFood)
        {
            //Debug.Log("Fed max food, triggering scene");
            FlagManager.SetFlag("PearCheck", "true");
            scene.Trigger();

            if(bridge != null)
                bridge.TurnOn();
            if (!fruitantJoinsParty) return;

            foreach(GameObject fruitant in fruitantsToAdd)
            {
                PlayerData pd = GameObject.FindObjectOfType<PlayerData>();
                pd.JoinTeam(fruitant);
                fruitantRB = fruitant.GetComponent<Rigidbody2D>();
                fruitantRB.constraints = RigidbodyConstraints2D.None;
                fruitantRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            //partyCommander.FamilyReunion();
        }
    }

    public void SetFollow()
    {
        if (!fruitantJoinsParty) return;
        Verb = AIData.Protocols.Chase;
        ObjectCriteria = Commander.Criteria.None;
        Object = GameObject.FindGameObjectWithTag("Player");       
        partyCommander.GroupCommand(player.GetComponent<PlayerData>().party, Verb, ObjectCriteria, Object, Location);
    }


}
