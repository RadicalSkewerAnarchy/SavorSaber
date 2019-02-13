using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneChange : MonoBehaviour
{
    public Image loadingBox;

    public void ChangeScene(string sceneName)
    {
        loadingBox.rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
