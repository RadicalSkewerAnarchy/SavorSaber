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

    public delegate void EventDelegate();
    public EventDelegate m_onStart;
    public EventDelegate m_onEnd;

    // Start is called before the first frame update
    void Start()
    {

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
            playerObject = other;
            player = other.gameObject;
            m_onStart = MoveToNewPosition;
            sceneLoader.LoadScenes(scenesToLoad, MoveToNewPosition, null);

        }
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
