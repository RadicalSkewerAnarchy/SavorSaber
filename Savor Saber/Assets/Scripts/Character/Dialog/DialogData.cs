using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to any entity that should bring up dialog boxes when interacted with
/// </summary>
public class DialogData : MonoBehaviour
{

    public enum Emotions:int
    {
        Neutral,
        Happy,
        Sad,
        Angry
    }

    public string displayName;

    [System.NonSerialized]
    public bool inConversation = false;

    public PortraitDictionary portraitDictionary = new PortraitDictionary();

    [System.Serializable]
    public class PortraitDictionary:SerializableCollections.SDictionary<Emotions, Sprite>
    {

    }
}
