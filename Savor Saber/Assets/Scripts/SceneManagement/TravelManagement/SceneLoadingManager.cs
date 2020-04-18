using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : MonoBehaviour
{

    public SceneReference[] ScenesToLoad;
    public GameObject LoadingScreenCanvas;

    private List<SceneReference> CurrentlyLoadedScenes = new List<SceneReference>();

    public delegate void EventDelegate();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadAllAsyncScene(ScenesToLoad, null, null));
    }


    public Coroutine LoadScenes(SceneReference[] sceneReferences, EventDelegate OnStart, EventDelegate OnEnd)
    {
        return StartCoroutine(LoadAllAsyncScene(sceneReferences, OnStart, OnEnd));
    }

    IEnumerator LoadAllAsyncScene(SceneReference[] sceneReferences, EventDelegate OnStart, EventDelegate OnEnd)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        LoadingScreenCanvas.SetActive(true);
        Time.timeScale = 0.0f;

        if (OnStart != null) OnStart();

        List<SceneReference> NewScenes = new List<SceneReference>();

        foreach (SceneReference sceneName in sceneReferences)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            NewScenes.Add(sceneName);
        }

        foreach (SceneReference sceneName in CurrentlyLoadedScenes)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

            // Wait until the asynchronous scene fully loads
            while (!asyncUnload.isDone)
            {
                yield return null;
            }
        }

        CurrentlyLoadedScenes = new List<SceneReference>();
        CurrentlyLoadedScenes = NewScenes;

        SceneManager.SetActiveScene(SceneManager.GetSceneByPath(NewScenes[0]));

        yield return new WaitForSecondsRealtime(0.25f);

        if (OnEnd != null) OnEnd();

        LoadingScreenCanvas.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
