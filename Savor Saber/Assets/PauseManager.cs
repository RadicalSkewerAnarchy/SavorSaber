using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    bool paused = false;
    Transform pauseCanvas;

    // Start is called before the first frame update
    void Start()
    {
        pauseCanvas = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetButtonDown(Control.Pause) && !paused)
        {
            Pause();
        }
        else if(InputManager.GetButtonDown(Control.Pause) && paused)
        {
            Unpause();
        }
    }

    private void Pause()
    {
        Time.timeScale = 0;
        pauseCanvas.gameObject.SetActive(true);
        paused = true;
    }

    private void Unpause()
    {
        Time.timeScale = 1;
        pauseCanvas.gameObject.SetActive(false);
        paused = false;
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }

    public void QuitToMenu()
    {
        paused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }
}
