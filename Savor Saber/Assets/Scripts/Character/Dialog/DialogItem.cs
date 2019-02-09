using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogItem
{
    public string actor;
    public string text;
    public Gameflow.DialogNodeVN.Emotion emotion; 
    public DialogItem(string text, string actor, Gameflow.DialogNodeVN.Emotion emotion)
    {
        this.text = text;
        this.actor = actor;
        this.emotion = emotion;
    }
}
