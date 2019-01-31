using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnCreate : MonoBehaviour
{
    public AudioClip sfx;
    public GameObject sfxPlayer;

    // Start is called before the first frame update
    void Start()
    {
        Play(sfx);
    }

    public void Play(AudioClip toPlay)
    {
        var sfxSoundObj = Instantiate(sfxPlayer, transform.position, transform.rotation);
        sfxSoundObj.GetComponent<PlayAndDestroy>().Play(toPlay);
    }
}
