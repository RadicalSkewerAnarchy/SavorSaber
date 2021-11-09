using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class DisplayInventory : MonoBehaviour
{
    public static DisplayInventory instance;

    #region fields

    /// <summary>
    /// Fields related to inventory visual representation
    /// </summary>

    public GameObject disableDuringCutscene;

    public Image HealthBarImage;
    public Text conTextPrompt;
    public Text cookedText;

    public Image[] skewerSpritesActive = new Image[3];
    public Image[] skewerSpritesSub1   = new Image[3];
    public Image[] skewerSpritesSub2   = new Image[3];
    public Image[] skewerHandleSprites = new Image[3];

    public Image[] flavorIconsActive = new Image[3];
    public Image[] flavorIconsSub1 = new Image[3];
    public Image[] flavorIconsSub2 = new Image[3];

    public Sprite[] flavorTextures = new Sprite[7];
    public Sprite emptySprite;
    public Sprite noFlavorSprite;

    private Dictionary<RecipeData.Flavors, Sprite> iconDictionary;
    private Inventory skewerInventory;

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
        iconDictionary.Add(RecipeData.Flavors.Umami, flavorTextures[5]);
        iconDictionary.Add(RecipeData.Flavors.Acquired, flavorTextures[6]);
        iconDictionary.Add(RecipeData.Flavors.None, noFlavorSprite);

        skewerInventory = PlayerController.instance.GetComponent<Inventory>();
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
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
    private void UpdateSkewerVisual(IngredientData[] ingredientData, Image[] ingredientImg)
    {
        // display the sprite associated with each IngredientData in the given array
        for (int i = 0; i < skewerInventory.maxItemsPerSkewer; i++)
        {
            if (i < ingredientData.Length)
            {
                ingredientImg[i].sprite = ingredientData[i].image;
            }
            else
            {
                ingredientImg[i].sprite = emptySprite;
            }
        }
    }

    private void UpdateFlavorIcons(IngredientData[] ingredientData, Image[] flavorIconImg)
    {
        // check each ingredient of the active skewer 
        for (int i = 0; i < 3; i++)
        {
            // failsafe to update all three slots even if there aren't three ingredients
            if(i >= ingredientData.Length)
            {
                flavorIconImg[i].sprite = emptySprite;
                continue;
            }

            RecipeData.Flavors flavor1 = RecipeData.Flavors.None;

            int flavorsFound = 0;
            IngredientData currentIngredient = ingredientData[i];

            // check for the presence of each flavor, 
            for (int f = 1; f <= 64; f = f << 1)
            {
                if((f & (int)currentIngredient.flavors) > 0)
                {
                    flavorsFound++;

                    if (flavorsFound == 1)
                        flavor1 = (RecipeData.Flavors)f;
                }
                if (flavorsFound >= 1)
                    break;
            }
            
            flavorIconImg[i].sprite = iconDictionary[flavor1];
        }

    }

    public void UpdateSkewerUI()
    {
        // Get ingredient arrays (Reverse of the skewer stacks)
        IngredientData[] skewerIngredientsActive  = skewerInventory.GetActiveSkewer().Reverse().ToArray();
        IngredientData[] skewerIngredientsLeft  = skewerInventory.GetLeftSkewer().Reverse().ToArray();
        IngredientData[] skewerIngredientsRight = skewerInventory.GetRightSkewer().Reverse().ToArray();
        // Display ingredient sprites
        UpdateSkewerVisual(skewerIngredientsActive, skewerSpritesActive);
        // UpdateSkewerVisual(skewerIngredientsLeft, skewerSpritesSub1);
        // UpdateSkewerVisual(skewerIngredientsRight, skewerSpritesSub2);

        // Display Flavor Icons
        UpdateFlavorIcons(skewerIngredientsActive, flavorIconsActive);
        UpdateFlavorIcons(skewerIngredientsLeft, flavorIconsSub1);
        UpdateFlavorIcons(skewerIngredientsRight, flavorIconsSub2);
    }

    public void UpdateSkewerUI(int slot, IngredientData data)
    {
        skewerSpritesActive[slot].gameObject.GetComponent<Animator>().SetTrigger("AddItem");
        skewerSpritesActive[slot].sprite = data.image;

        IngredientData[] skewerIngredientsActive = skewerInventory.GetActiveSkewer().Reverse().ToArray();
        IngredientData[] skewerIngredientsLeft = skewerInventory.GetLeftSkewer().Reverse().ToArray();
        IngredientData[] skewerIngredientsRight = skewerInventory.GetRightSkewer().Reverse().ToArray();

        UpdateFlavorIcons(skewerIngredientsActive, flavorIconsActive);
        UpdateFlavorIcons(skewerIngredientsLeft, flavorIconsSub1);
        UpdateFlavorIcons(skewerIngredientsRight, flavorIconsSub2);
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
