using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFXRandPitch : MonoBehaviour
{
    public AudioClip sfx;
    public GameObject sfxPlayer;

    public void PlayRandPitch(AudioClip toPlay)
    {
        var sfxSoundObj = Instantiate(sfxPlayer, transform.position, transform.rotation);
        if (sfxSoundObj.GetComponent<PlayAndDestroyRandPitch>() != null)
            sfxSoundObj.GetComponent<PlayAndDestroyRandPitch>().Play(toPlay);
    }
}
