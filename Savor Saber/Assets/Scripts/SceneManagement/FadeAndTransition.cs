using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(EventFade))]
public class FadeAndTransition : MonoBehaviour
{
    public string sceneName;
    private EventFade fade;
    // Start is called before the first frame update
    void Start()
    {
        fade = GetComponent<EventFade>();
    }

    public void BeginEvent()
    {
        StartCoroutine(FadeAndTransitionCr());
    }

    private IEnumerator FadeAndTransitionCr()
    {
        yield return StartCoroutine(fade.PlayEvent());
        SceneManager.LoadScene(sceneName);
    }


}
