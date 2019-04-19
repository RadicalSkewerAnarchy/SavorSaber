using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventFade : MonoBehaviour
{
    public Image fade;
    public Color color;
    public bool fadeOut = true;
    public float speed = 0.1f;
    public void Play()
    {
        //Debug.Log("derp");
        StartCoroutine(PlayEvent());
    }
    public IEnumerator PlayEvent()
    {
        fade.enabled = true;
        float start = fadeOut ? 0 : 1;
        float end = fadeOut ? 1 : 0;
        float sign = fadeOut ? 1 : -1;
        fade.color = new Color(color.r, color.g, color.b, start);
        while (fade.color.a != end)
        {
            yield return new WaitForFixedUpdate();
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, Mathf.Clamp01(fade.color.a + Time.fixedDeltaTime * speed * sign));
        }
        fade.enabled = fadeOut;
    }
}
