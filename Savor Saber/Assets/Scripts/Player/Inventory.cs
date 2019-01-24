using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for functions related to managing and displaying inventory
/// </summary>
public class Inventory : MonoBehaviour {

    #region fields
    /// <summary>
    /// Fields related to inventory visual representation
    /// </summary>
    //public Canvas canvas = null;
    public int maxItemsPerSkewer = 3;
    public Image[] skewerSprites = new Image[3];
    public Sprite emptySprite;


    /// <summary>
    /// Fields related to skewer switching
    /// </summary>
    private int activeSkewer = 0;
    private Stack<IngredientData>[] quiver = new Stack<IngredientData>[3];
    #endregion

    void Start () {
        quiver[0] = new Stack<IngredientData>();
        quiver[1] = new Stack<IngredientData>();
        quiver[2] = new Stack<IngredientData>();
    }

    /// <summary>
    /// Returns true if the active skewer is full
    /// </summary>
    public bool ActiveSkewerFull()
    {
        return (quiver[activeSkewer].Count == 3);
    }

    /// <summary>
    /// Add or remove ingredients from active skewer
    /// </summary>
    public void AddToSkewer(IngredientData ingredient)
    {
        quiver[activeSkewer].Push(ingredient);
        UpdateSkewerVisual();
    }

    public IngredientData RemoveFromSkewer()
    {
        IngredientData topIngredient = quiver[activeSkewer].Pop();
        return topIngredient;
    }

    /// <summary>
    /// Update the visuals to display the current inventory state
    /// </summary>
    private void UpdateSkewerVisual()
    {
        //convert the active skewer stack to an array and reverse it
        IngredientData[] dropArray = quiver[activeSkewer].ToArray();
        IngredientData[] reverseDropArray = new IngredientData[dropArray.Length];
        for(int a = 0; a < dropArray.Length; a++)
        {
            reverseDropArray[a] = dropArray[dropArray.Length - (a + 1)];
        }
        dropArray = reverseDropArray;

        //display the sprite associated with each IngredientData in the resulting array
        for (int i = 0; i < maxItemsPerSkewer; i++)
        {
            if(i < dropArray.Length)
            {
                print("showing " + dropArray[i].flavors + " at index " + i);
                skewerSprites[i].sprite = dropArray[i].image;
            }
            else
            {
                skewerSprites[i].sprite = emptySprite;
            }
        }
    }

}
