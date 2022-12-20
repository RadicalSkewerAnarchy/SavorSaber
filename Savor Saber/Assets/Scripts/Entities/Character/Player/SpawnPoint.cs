using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPoint : MonoBehaviour
{
    public GameObject location;
    public SceneReference[] scenesToLoad;
    private SceneLoadingManager sceneLoader;

    public delegate void EventDelegate();
    public EventDelegate m_onStart;
    public EventDelegate m_onEnd;
    public bool emergencyDismountFlag = false; //temporary measure to avoid horseradishes breaking the game
    private GameObject player;
    public GameObject newPosition;
    private Collider2D playerObject;

    public bool hasAlternateScenes = false;
    public string[] alternateSceneFlags;
    public SceneReference[] alternateScenes;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (sceneLoader == null)
        {
            sceneLoader = FindObjectOfType<SceneLoadingManager>();
        }
    }

    public void Respawn(GameObject player)
    {
        //respawn
        if (emergencyDismountFlag)
        {
            player.GetComponent<PlayerController>().currentSaddle.GetComponent<FruitantMount>().Demount();
        }
        var cData = player.GetComponent<CharacterData>();
        cData.health = cData.maxHealth;
        //Add more fancy death scene later
        player.transform.position = location.transform.position;
        player.GetComponent<Animator>().Play("Idle");

        //load correct scene

        m_onStart = DoNothing;
        
    
        //this is terrible code but I can't be arsed to do anything better right now
        if(hasAlternateScenes)
        {
            if(alternateSceneFlags.Length != alternateScenes.Length)
            {
                
                sceneLoader.LoadScenes(scenesToLoad, DoNothing, null);
                return;
            }
            for(int i = alternateSceneFlags.Length -1; i >= 0; i--)
            {
                if(FlagManager.GetFlag(alternateSceneFlags[i]) == "True")
                {
                    SceneReference[] tempSceneArray = new SceneReference[1];
                    tempSceneArray[0] = alternateScenes[i];
                    sceneLoader.LoadScenes(tempSceneArray, DoNothing, null);
                }
            }
        }

        sceneLoader.LoadScenes(scenesToLoad, DoNothing, null);
    }

    void DoNothing()
    {
        return;
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
            if (companion != null)
            {
                FruitantMount saddle = companion.GetComponentInChildren<FruitantMount>();
                saddle.DemountOnLoad();
            }

            //preserve the fact that we *were* riding before the transition
            somaController.loadRiding = true;
        }
    }
}
