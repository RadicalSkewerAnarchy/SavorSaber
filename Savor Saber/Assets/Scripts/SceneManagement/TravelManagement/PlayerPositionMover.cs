using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerPositionMover : MonoBehaviour
{

    public GameObject newPosition;
    public SceneReference[] scenesToLoad;
    private SceneLoadingManager sceneLoader;
    private Collider2D playerObject;
    private GameObject player;
    private EventTrigger transitionScene;

    public delegate void EventDelegate();
    public EventDelegate m_onStart;
    public EventDelegate m_onEnd;
    [HideInInspector]
    public bool emergencyDismountFlag = false; //temporary measure to avoid horseradishes breaking the game

    public UnityEvent eventOnContact;[Header("Alternate Scene conditions")]

    public bool hasAlternateScenes = false;
    public SceneReference[] alternateScenes;
    public string flag;
    public string value;

    // Start is called before the first frame update
    void Start()
    {
        transitionScene = GetComponentInChildren<EventTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneLoader == null)
        {
            sceneLoader = FindObjectOfType<SceneLoadingManager>();
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {


        if (other.gameObject.tag == "Player")
        {
            eventOnContact.Invoke();

            playerObject = other;
            player = other.gameObject;
            if (emergencyDismountFlag)
            {
                player.GetComponent<PlayerController>().currentSaddle.GetComponent<FruitantMount>().Demount();
            }

            //find DayNightController to save time of day
            DayNightController dnc = FindObjectOfType<DayNightController>();
            dnc.timeOfDayTransitionHolder = dnc.CurrTimeOfDay;
            m_onStart = MoveToNewPosition;
            transitionScene.Trigger();
        }
    }

    public void LoadScene()
    {
        if (hasAlternateScenes && FlagManager.GetFlag(flag) == value)
            sceneLoader.LoadScenes(alternateScenes, MoveToNewPosition, null);
        else
            sceneLoader.LoadScenes(scenesToLoad, MoveToNewPosition, null);
    }

    void MoveToNewPosition()
    {
        player.transform.position = new Vector3(newPosition.transform.position.x, newPosition.transform.position.y, playerObject.gameObject.transform.position.z);

        player.GetComponent<SpriteRenderer>().color = Color.white;
        //temporarily dismount if riding a fruitant
        PlayerController somaController = player.GetComponent<PlayerController>();
        somaController.Stop();
        if (somaController.riding)
        {
            PlayerData somaData = player.GetComponent<PlayerData>();
            GameObject companion = somaData.party[0];
            if(companion != null)
            {
                FruitantMount saddle = companion.GetComponentInChildren<FruitantMount>();
                saddle.DemountOnLoad();
            }

            //preserve the fact that we *were* riding before the transition
            somaController.loadRiding = true;
        }
    }
}
