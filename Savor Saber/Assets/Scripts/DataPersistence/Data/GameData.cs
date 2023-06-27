using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int health;
    public int testField;
    public GameData()
    {
        this.health = 5;
        this.testField = 0;
    }
}
