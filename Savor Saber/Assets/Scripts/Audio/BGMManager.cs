using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    private AudioSource[] bgmSrc;
    private bool firstSelected = true;
    public AudioSource CurrBgmSource { get => firstSelected ? bgmSrc[0] : bgmSrc[1]; }
    private AudioSource FadeBgmSource { get => firstSelected ? bgmSrc[1] : bgmSrc[0]; }
    private AudioClip bgmBuffer;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        bgmSrc = GetComponents<AudioSource>();
    }

    public void SetBGM(AudioClip song)
    {
        CurrBgmSource.clip = song;
    }

    public void CrossFadeBgm(AudioClip song, float fadeTime = 3)
    {
        if (CurrBgmSource.volume < 1)
            bgmBuffer = song;
        FadeBgmSource.clip = song;
        FadeBgmSource.Play();
        StartCoroutine(CrossFadeWithBuffer(CurrBgmSource, FadeBgmSource, fadeTime));
        firstSelected = !firstSelected;
        
    }

    private IEnumerator CrossFadeWithBuffer(AudioSource CurrBgmSource, AudioSource FadeBgmSource, float fadeTime)
    {
        yield return StartCoroutine(CrossFade.CrossFadeAudioSource(CurrBgmSource, FadeBgmSource, fadeTime));
        if(bgmBuffer != null)
        {
            if(bgmBuffer.name == CurrBgmSource.clip.name)
            {
                bgmBuffer = null;
                yield break;
            }
            FadeBgmSource.clip = bgmBuffer;
            bgmBuffer = null;
            FadeBgmSource.Play();
            StartCoroutine(CrossFadeWithBuffer(CurrBgmSource, FadeBgmSource, fadeTime));
            firstSelected = !firstSelected;
        }

    }
}
