using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CrossFade
{
    public static IEnumerator CrossFadeAudioSource(AudioSource fadeOut, AudioSource fadeIn, float time, float goal = 1)
    {
        float diffA = fadeOut.volume;
        float diffB = Mathf.Abs(goal - fadeIn.volume);
        while(fadeIn.volume < goal)
        {
            yield return new WaitForEndOfFrame();
            float percent = (Time.deltaTime / time);
            fadeIn.volume += percent * diffB;
            fadeOut.volume -= percent * diffB;
        }
        fadeIn.volume = goal;
        fadeOut.volume = 0;
    }
    public static IEnumerator FadeAudioSourceOut(AudioSource fade, float time)
    {
        float diff = fade.volume;
        while (fade.volume > 0)
        {
            yield return new WaitForEndOfFrame();
            float percent = (Time.deltaTime / time);
            fade.volume -= percent * diff;
        }
        fade.volume = 0;
    }

    public static IEnumerator FadeAudioSourceIn(AudioSource fade, float time, float goal = 1)
    {
        fade.Play();
        float diff = goal - fade.volume;
        while (fade.volume < goal)
        {
            yield return new WaitForEndOfFrame();
            float percent = (Time.deltaTime / time);
            fade.volume += percent * diff;
        }
        fade.volume = goal;
    }
}
