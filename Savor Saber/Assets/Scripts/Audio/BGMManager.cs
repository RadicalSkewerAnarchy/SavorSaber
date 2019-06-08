using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    public AreaChange initialData;
    public AudioClip AreaBgmDay { get; set; }
    public AudioClip AreaBgmNight { get; set; }
    public AudioClip AreaBgsDay { get; set; }
    public AudioClip AreaBgsNight { get; set; }

    public GameObject bgmContainer;
    private AudioSource[] bgmSrc;
    private bool firstSelectedBgm = true;
    public AudioSource CurrBgmSource { get => firstSelectedBgm ? bgmSrc[0] : bgmSrc[1]; }
    private AudioSource FadeBgmSource { get => firstSelectedBgm ? bgmSrc[1] : bgmSrc[0]; }
    private AudioClip bgmBuffer;

    public GameObject bgsContainer;
    private AudioSource[] bgsSrc;
    private bool firstSelectedBgs = true;
    public AudioSource CurrBgsSource { get => firstSelectedBgs ? bgsSrc[0] : bgsSrc[1]; }
    private AudioSource FadeBgsSource { get => firstSelectedBgs ? bgsSrc[1] : bgsSrc[0]; }
    private AudioClip bgsBuffer;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        AreaBgmDay = initialData.dayMusic;
        AreaBgmNight = initialData.nightMusic;
        AreaBgsDay = initialData.dayBgs;
        AreaBgsNight = initialData.nightBgs;
        bgmSrc = bgmContainer.GetComponents<AudioSource>();
        bgsSrc = bgsContainer.GetComponents<AudioSource>();
        DayNightController.instance.OnDay += GoToDayMusic;
        DayNightController.instance.OnNight += GoToNightMusic;
    }

    public void GoToNightMusic()
    {
        if(CurrBgmSource.clip == AreaBgmDay)
        {
            FadeToAreaSounds();
        }
    }

    public void GoToDayMusic()
    {
        if (CurrBgmSource.clip == AreaBgmNight)
        {
            FadeToAreaSounds();
        }
    }

    public void SetBGM(AudioClip song)
    {
        CurrBgmSource.clip = song;
    }

    public void FadeBGMToSilence(float fadeTime = 3)
    {
        CrossFadeBgm(null, fadeTime);
    }

    public void FadeToAreaSounds(float fadeTime = 3)
    {
        if(DayNightController.instance.IsDayTime)
        {
            CrossFadeBgm(AreaBgmDay, fadeTime);
            CrossFadeBgs(AreaBgsDay, fadeTime);
        }
        else
        {
            CrossFadeBgm(AreaBgmNight, fadeTime);
            CrossFadeBgs(AreaBgsNight, fadeTime);
        }
    }

    public void CrossFadeBgm(AudioClip song, float fadeTime = 3)
    {
        if (CurrBgmSource.volume < 1)
        {
            bgmBuffer = song;
            return;
        }
            
        FadeBgmSource.clip = song;
        FadeBgmSource.Play();
        StartCoroutine(CrossFadeBgmWithBuffer(CurrBgmSource, FadeBgmSource, fadeTime));
        firstSelectedBgm = !firstSelectedBgm;       
    }

    public void CrossFadeBgs(AudioClip song, float fadeTime = 3)
    {
        if (CurrBgsSource.volume < 1)
        {
            bgmBuffer = song;
            return;
        }
            
        FadeBgsSource.clip = song;
        FadeBgsSource.Play();
        StartCoroutine(CrossFadeBgsWithBuffer(CurrBgsSource, FadeBgsSource, fadeTime));
        firstSelectedBgs = !firstSelectedBgs;
    }

    private IEnumerator CrossFadeBgmWithBuffer(AudioSource CurrBgmSource, AudioSource FadeBgmSource, float fadeTime)
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
            StartCoroutine(CrossFadeBgmWithBuffer(CurrBgmSource, FadeBgmSource, fadeTime));
            firstSelectedBgm = !firstSelectedBgm;
        }

    }

    private IEnumerator CrossFadeBgsWithBuffer(AudioSource CurrBgsSource, AudioSource FadeBgsSource, float fadeTime)
    {
        yield return StartCoroutine(CrossFade.CrossFadeAudioSource(CurrBgsSource, FadeBgsSource, fadeTime));
        if (bgsBuffer != null)
        {
            if (bgsBuffer.name == CurrBgsSource.clip.name)
            {
                bgsBuffer = null;
                yield break;
            }
            FadeBgsSource.clip = bgsBuffer;
            bgsBuffer = null;
            FadeBgsSource.Play();
            StartCoroutine(CrossFadeBgsWithBuffer(CurrBgsSource, FadeBgsSource, fadeTime));
            firstSelectedBgs = !firstSelectedBgs;
        }

    }
}
