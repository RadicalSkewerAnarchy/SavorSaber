using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest")]
public class QuestData : ScriptableObject
{
    public string displayName;
    public string description;
    public Texture2D icon;
}
