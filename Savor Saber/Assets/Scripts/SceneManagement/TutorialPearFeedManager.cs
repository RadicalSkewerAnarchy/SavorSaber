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
    // Start is called before the first frame update
    void Start()
    {
        
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
            Debug.Log("Fed max food, triggering scene");
            scene.Trigger();
            FlagManager.SetFlag("PearCheck", "true");
            bridge.TurnOn();
            foreach(GameObject fruitant in fruitantsToAdd)
            {
                partyCommander.JoinTeam(fruitant); 
            }
        }
    }


}
