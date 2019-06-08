using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BossAreaMusic : MonoBehaviour
{
    public const string bossFlag = "bossFight";
    public AudioClip music;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (FlagManager.GetFlag("bossFight") == "true" && BGMManager.instance.CurrBgmSource.clip != music)
            BGMManager.instance.CrossFadeBgm(music, 2);
    }
}
