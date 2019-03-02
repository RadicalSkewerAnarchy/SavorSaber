using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Emotion
{
    Neutral,
    Happy,
    Glad,
    Angry,
    Sad,
    Surprised,
    Special,
    NoSprite,
}

[System.Serializable]
public class DialogItem
{
    public string text;
    public Emotion emotion; 
    public DialogItem(string text, Emotion emotion)
    {
        this.text = text;
        this.emotion = emotion;
    }
}
