using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int health;
    public int testField;
    public Vector2 playerPosition;
    public int activeSkewerIndex;
    public IngredientData[] activeSkewerIngredients;
    public IngredientData[] leftSkewerIngredients;
    public IngredientData[] rightSkewerIngredients;
    public GameData()
    {
        this.health = 5;
        this.testField = 0;
        this.playerPosition = new Vector2(-36.232f, -15.591f);
        this.activeSkewerIndex = 0;
        activeSkewerIngredients = new IngredientData[3];
        leftSkewerIngredients = new IngredientData[3];
        rightSkewerIngredients = new IngredientData[3];
    }
}
