using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains data to be used by conversation dialog boxes
/// </summary>
public class DialogData : MonoBehaviour
{

    public enum Emotions
    {
        Neutral,
        Happy,
        Sad,
        Angry
    }

    public string displayName;

    public Sprite portraitSpriteNeutral;


}
