using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string[] sceneNames;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var name in sceneNames)
            SceneManager.LoadScene(name, LoadSceneMode.Additive);
    }
}
