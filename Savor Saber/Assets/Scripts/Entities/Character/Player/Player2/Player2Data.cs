using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="palyer2Data", menuName ="Player2 Data")]
public class Player2Data : ScriptableObject
{
    public string displayName;
    public Sprite charaSelectImage;
    public GameObject playerPrefab;
}
