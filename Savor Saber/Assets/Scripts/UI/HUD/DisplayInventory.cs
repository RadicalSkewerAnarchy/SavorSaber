using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DisplayInventory : MonoBehaviour
{
    public static DisplayInventory instance;

    #region fields

    /// <summary>
    /// Fields related to inventory visual representation
    /// </summary>

    public Text conTextPrompt;
    public Text cookedText;
    public Inventory skewerInventory; 
    public Image[] skewerSpritesActive = new Image[3];
    public Image[] skewerSpritesUp   = new Image[3];
    public Image[] skewerSpritesDown  = new Image[3];
    public Image[] skewerHandleSprites = new Image[3];

    public Image[] flavorIcons = new Image[6];
    public Sprite[] flavorTextures = new Sprite[7];
    public Sprite emptySprite;

    private Dictionary<RecipeData.Flavors, Sprite> iconDictionary;

    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        iconDictionary = new Dictionary<RecipeData.Flavors, Sprite>();

        iconDictionary.Add(RecipeData.Flavors.Sweet, flavorTextures[0]);
        iconDictionary.Add(RecipeData.Flavors.Spicy, flavorTextures[1]);
        iconDictionary.Add(RecipeData.Flavors.Bitter, flavorTextures[2]);
        iconDictionary.Add(RecipeData.Flavors.Sour, flavorTextures[3]);
        iconDictionary.Add(RecipeData.Flavors.Salty, flavorTextures[4]);
        iconDictionary.Add(RecipeData.Flavors.Savory, flavorTextures[5]);
        iconDictionary.Add(RecipeData.Flavors.Acquired, flavorTextures[6]);

        Array.Reverse(flavorIcons);
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform);
        }
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateSkewerPrompt();
        //UpdateSkewerVisual();
        //UpdateFlavorIcons();
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
                //print("showing " + dropActiveArray[i].flavors + " at index " + i);
                skewerSpritesActive[i].sprite = dropActiveArray[i].image;
            }
            else
            {
                skewerSpritesActive[i].sprite = emptySprite;
            }

            if (i < dropLeftArray.Length)
            {
                //print("showing " + dropLeftArray[i].flavors + " at index " + i);
                skewerSpritesUp[i].sprite = dropLeftArray[i].image;
            }
            else
            {
                skewerSpritesUp[i].sprite = emptySprite;
            }

            if (i < dropRightArray.Length)
            {
                //print("showing " + dropRightArray[i].flavors + " at index " + i);
                skewerSpritesDown[i].sprite = dropRightArray[i].image;
            }
            else
            {
                skewerSpritesDown[i].sprite = emptySprite;
            }
        }
    }

    private void UpdateFlavorIcons()
    {
        //int numFlavors1, numFlavors2, numFlavors3;
        IngredientData[] dropActiveArray = skewerInventory.GetActiveSkewer().ToArray();
        Array.Reverse(dropActiveArray);

        //MAKING CHANGES

        //check each ingredient of the active skewer 
        for (int i = 0; i < 3; i++)
        {
            //failsafe to update all three slots even if there aren't three ingredients
            if(i >= dropActiveArray.Length)
            {
                int index1a = i * 2;
                int index2a = (i * 2) + 1;

                flavorIcons[index1a].sprite = emptySprite;
                flavorIcons[index2a].sprite = emptySprite;

                continue;
            }

            RecipeData.Flavors flavor1, flavor2;
            flavor1 = flavor2 = RecipeData.Flavors.None;

            int flavorsFound = 0;
            IngredientData currentIngredient = dropActiveArray[i];

            //check for the presence of each flavor, 
            for (int f = 1; f <= 64; f = f << 1)
            {
                if((f & (int)currentIngredient.flavors) > 0)
                {
                    flavorsFound++;

                    if (flavorsFound == 1)
                        flavor1 = (RecipeData.Flavors)f;
                    else if (flavorsFound == 2)
                        flavor2 = (RecipeData.Flavors)f;
                }
                if (flavorsFound >= 2)
                    break;
            }
            
            //assign icons appropriately
            int index1 = i * 2;
            int index2 = (i * 2) + 1;

            if (flavor1 != RecipeData.Flavors.None)
                flavorIcons[index1].sprite = iconDictionary[flavor1];
            else
                flavorIcons[index1].sprite = emptySprite;

            if (flavor2 != RecipeData.Flavors.None)
                flavorIcons[index2].sprite = iconDictionary[flavor2];
            else
                flavorIcons[index2].sprite = emptySprite;



        }

    }

    public void UpdateSkewerUI()
    {
        UpdateSkewerVisual();
        UpdateFlavorIcons();
    }

    public void SwapHandles(bool up)
    {
        if(up)
        {
            var tempSprite = skewerHandleSprites[0].sprite;
            skewerHandleSprites[0].sprite = skewerHandleSprites[1].sprite;
            skewerHandleSprites[1].sprite = skewerHandleSprites[2].sprite;
            skewerHandleSprites[2].sprite = tempSprite;
        }
        else
        {
            var tempSprite = skewerHandleSprites[2].sprite;
            skewerHandleSprites[2].sprite = skewerHandleSprites[1].sprite;
            skewerHandleSprites[1].sprite = skewerHandleSprites[0].sprite;
            skewerHandleSprites[0].sprite = tempSprite;
        }

    }
    #endregion
}
