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
            m_onStart = MoveToNewPosition;
            sceneLoader.LoadScenes(scenesToLoad, MoveToNewPosition, null);

        }
    }

    void MoveToNewPosition()
    {
        playerObject.gameObject.transform.position = new Vector3(newPosition.transform.position.x, newPosition.transform.position.y, playerObject.gameObject.transform.position.z);
    }
}
