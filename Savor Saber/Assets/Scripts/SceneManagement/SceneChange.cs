using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneChange : MonoBehaviour
{
    public Image loadingBox;
    public GameDataManager gdm; 

    public void ChangeScene(string sceneName)
    {
        loadingBox.rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeSceneNewGame(string sceneName)
    {
        if (gdm == null)
            gdm = FindObjectOfType<GameDataManager>();
        gdm.isNewGame = true;
        ChangeScene(sceneName);
    }
    public void ChangeSceneContinue(string sceneName)
    {
        if (gdm == null)
            gdm = FindObjectOfType<GameDataManager>();
        gdm.isNewGame = false;
        ChangeScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;

    }
}
