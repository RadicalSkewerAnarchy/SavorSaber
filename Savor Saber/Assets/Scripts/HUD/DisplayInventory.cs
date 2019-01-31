using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{

    #region fields

    /// <summary>
    /// Fields related to inventory visual representation
    /// </summary>

    public Text conTextPrompt;
    public Text cookedText;
    public Inventory skewerInventory; 
    public Image[] skewerSpritesActive = new Image[3];
    public Image[] skewerSpritesLeft   = new Image[3];
    public Image[] skewerSpritesRight  = new Image[3];
    public Sprite emptySprite;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSkewerPrompt();
        UpdateSkewerVisual();
    }

    #region utility functions

    /// <summary>
    /// Update the text prompt to display the current skewer action
    /// </summary>
    private void UpdateSkewerPrompt()
    {
        conTextPrompt.text = "";
        if (skewerInventory.GetActiveSkewer().GetCount() > 0)
        {
            conTextPrompt.text = "[C] : COOK";
        }
        if (skewerInventory.ActiveSkewerCooked()) {
            conTextPrompt.text = "[SPACE] : THROW";
            cookedText.text = "!SKEWER COOKED!";
        }
        else
        {
            cookedText.text = "";
            if (skewerInventory.ActiveSkewerFull()) cookedText.text = "FULL CAPACITY";
        }
    }

    /// <summary>
    /// Update the visuals to display the current inventory state
    /// </summary>
    private void UpdateSkewerVisual()
    {
        //convert the active skewer stack to an array and reverse it
        IngredientData[] dropActiveArray = skewerInventory.GetActiveSkewer().ToArray();
        IngredientData[] dropLeftArray = skewerInventory.GetLeftSkewer().ToArray();
        IngredientData[] dropRightArray = skewerInventory.GetRightSkewer().ToArray();
        IngredientData[] reverseDropActiveArray = new IngredientData[dropActiveArray.Length];
        IngredientData[] reverseDropLeftArray = new IngredientData[dropLeftArray.Length];
        IngredientData[] reverseDropRightArray = new IngredientData[dropRightArray.Length];
        for (int a = 0; a < dropActiveArray.Length; a++)
        {
            reverseDropActiveArray[a] = dropActiveArray[dropActiveArray.Length - (a + 1)];
        }
        for (int a = 0; a < dropLeftArray.Length; a++)
        {
            reverseDropLeftArray[a] = dropLeftArray[dropLeftArray.Length - (a + 1)];
        }
        for (int a = 0; a < dropRightArray.Length; a++)
        {
            reverseDropRightArray[a] = dropRightArray[dropRightArray.Length - (a + 1)];
        }
        dropActiveArray = reverseDropActiveArray;
        dropLeftArray = reverseDropLeftArray;
        dropRightArray = reverseDropRightArray;

        //display the sprite associated with each IngredientData in the resulting array
        for (int i = 0; i < skewerInventory.maxItemsPerSkewer; i++)
        {
            if (i < dropActiveArray.Length)
            {
                print("showing " + dropActiveArray[i].flavors + " at index " + i);
                skewerSpritesActive[i].sprite = dropActiveArray[i].image;
            }
            else
            {
                skewerSpritesActive[i].sprite = emptySprite;
            }

            if (i < dropLeftArray.Length)
            {
                print("showing " + dropLeftArray[i].flavors + " at index " + i);
                skewerSpritesLeft[i].sprite = dropLeftArray[i].image;
            }
            else
            {
                skewerSpritesLeft[i].sprite = emptySprite;
            }

            if (i < dropRightArray.Length)
            {
                print("showing " + dropRightArray[i].flavors + " at index " + i);
                skewerSpritesRight[i].sprite = dropRightArray[i].image;
            }
            else
            {
                skewerSpritesRight[i].sprite = emptySprite;
            }
        }
    }
    #endregion
}
