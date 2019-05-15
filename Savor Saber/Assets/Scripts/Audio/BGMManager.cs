using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    private AudioSource mainSrc;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        mainSrc = GetComponent<AudioSource>();
    }

    public void SetBGM(AudioClip song)
    {
        mainSrc.clip = song;
    }
}
