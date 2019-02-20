using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to any entity that should bring up dialog boxes when interacted with
/// </summary>
public class DialogData : MonoBehaviour
{
    public string displayName;

    public AudioClip textBlipSound;

    [System.NonSerialized]
    public bool inConversation = false;

    public PortraitDictionary portraitDictionary = new PortraitDictionary();

    [System.Serializable]
    public class PortraitDictionary:SerializableCollections.SDictionary<Gameflow.DialogNodeVN.Emotion, Sprite>
    {

    }
}
