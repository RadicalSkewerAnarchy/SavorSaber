using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFadeAudio : EventScript
{
    public enum Type
    {
        In,
        Out,
    }

    public bool wait = false;
    public Type fadeType;
    public AudioSource src;
    public float time;

    public override IEnumerator PlayEvent(GameObject player)
    {
        if(wait)
        {
            if (fadeType == Type.In)
                yield return StartCoroutine(CrossFade.FadeAudioSourceIn(src, time));
            else
                yield return StartCoroutine(CrossFade.FadeAudioSourceOut(src, time));
        }
        else
        {
            if (fadeType == Type.In)
                StartCoroutine(CrossFade.FadeAudioSourceIn(src, time));
            else
                StartCoroutine(CrossFade.FadeAudioSourceOut(src, time));
            yield break;
        }

    }


}
