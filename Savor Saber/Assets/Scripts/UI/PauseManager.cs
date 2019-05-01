using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseManager : MonoBehaviour
{
    bool paused = false;
    Transform pauseCanvas;
    AudioSource pauseSound;
    public UnityEngine.UI.Button startButton;

    // Start is called before the first frame update
    void Start()
    {
        pauseCanvas = transform.GetChild(0);
        pauseSound = GetComponent<AudioSource>();
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
        startButton.Select();
        paused = true;
        PlaySound();
    }

    private void Unpause()
    {
        Time.timeScale = 1;
        pauseCanvas.gameObject.SetActive(false);
        paused = false;
        PlaySound();
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

    public void PlaySound()
    {
        pauseSound.Play();
    }

}
