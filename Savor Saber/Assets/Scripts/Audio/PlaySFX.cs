using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    public AudioClip sfx;
    public GameObject sfxPlayer;

    public void Play(AudioClip toPlay)
    {
        var sfxSoundObj = Instantiate(sfxPlayer, transform.position, transform.rotation);
        if (sfxSoundObj.GetComponent<PlayAndDestroy>() != null)
            sfxSoundObj.GetComponent<PlayAndDestroy>().Play(toPlay);
    }
}
