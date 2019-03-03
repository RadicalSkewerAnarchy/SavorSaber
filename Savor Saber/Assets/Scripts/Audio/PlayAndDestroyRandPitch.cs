using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAndDestroyRandPitch : PlayAndDestroy
{
    public override void Play(AudioClip toPlay)
    {
        GetComponent<AudioSource>().pitch = 0.95f + Random.Range(0.0f, 0.1f);
        GetComponent<AudioSource>().PlayOneShot(toPlay);
        Destroy(gameObject, toPlay.length);
    }
}
