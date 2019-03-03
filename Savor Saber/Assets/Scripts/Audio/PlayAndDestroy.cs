using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAndDestroy : MonoBehaviour
{
    public virtual void Play(AudioClip toPlay)
    {
        GetComponent<AudioSource>().PlayOneShot(toPlay);
        Destroy(gameObject, toPlay.length);
    }
}
