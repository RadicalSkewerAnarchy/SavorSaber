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
}
