using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Controller for functions related to managing and displaying inventory
/// </summary>
///
public class Inventory : MonoBehaviour {

    #region fields

    public bool CanSwap { get; set; }

    public int maxItemsPerSkewer = 3;

    /// <summary>
    /// Fields related to actual inventory tracking
    /// </summary>
    public int activeSkewer = 0;
    private int numberOfSkewers = 3;
    private Skewer[] quiver;

    /// <summary>
    /// Fields related to cooking
    /// </summary>
    public bool nearCampfire = false;

    /// <summary>
    /// Fields related to audio
    /// </summary>
    public AudioClip swapSFX;
    public AudioClip cookSFX;
    public AudioClip cantCookSFX;
    public AudioClip fullSFX;
    private PlaySFX sfxPlayer;

    #endregion

    void Start ()
    {

        quiver = new Skewer[numberOfSkewers];
        quiver[0] = new Skewer();
        quiver[1] = new Skewer();
        quiver[2] = new Skewer();
        CanSwap = true;
        sfxPlayer = GetComponent<PlaySFX>();

        //initialize dictionaries 
        for(int i = 0; i < quiver.Length; i++)
        {
            quiver[i].InitializeDictionary();
        }
    }

    private void Update()
    {
        //Detect swapping input
        GetSkewerSwapInput();
    }

    #region utility functions

    /// <summary>
    /// returns the currently active skewer
    /// </summary>
    public Skewer GetActiveSkewer()
    {
        return quiver[activeSkewer];
    }

    /// <summary>
    /// returns the left skewer
    /// </summary>
    public Skewer GetLeftSkewer()
    {
        int leftIndex = activeSkewer - 1;
        if (leftIndex < 0) leftIndex = quiver.Length-1;
        return quiver[leftIndex];
    }

    /// <summary>
    /// returns the right skewer
    /// </summary>
    public Skewer GetRightSkewer()
    {
        int rightIndex = activeSkewer + 1;
        if (rightIndex > quiver.Length - 1) rightIndex = 0;
        return quiver[rightIndex];
    }

    /// <summary>
    /// returns an array of all skewers
    /// </summary>
    public Skewer[] GetAllskewers()
    {
        return quiver;
    }

    /// <summary>
    /// Returns true if the active skewer is full
    /// </summary>
    public bool ActiveSkewerFull()
    {
        return (quiver[activeSkewer].GetCount() == maxItemsPerSkewer);
    }

    /// <summary>
    /// returns true if all skewers are full
    /// </summary>
    public bool AllSkewersFull()
    {
        return GetRightSkewer().GetCount() == maxItemsPerSkewer &&
               GetLeftSkewer().GetCount() == maxItemsPerSkewer &&
               GetActiveSkewer().GetCount() == maxItemsPerSkewer;
    }

    /// <summary>
    /// Returns true if the active skewer is empty
    /// </summary>
    public bool ActiveSkewerEmpty()
    {
        return (quiver[activeSkewer].GetCount() == 0);
    }

    /// <summary>
    /// returns true if all skewers are empty
    /// </summary>
    public bool AllSkewersEmpty()
    {
        return GetRightSkewer().GetCount() == 0 &&
               GetLeftSkewer().GetCount() == 0 &&
               GetActiveSkewer().GetCount() == 0;
    }

    /// <summary>
    /// Returns true if the active skewer has been cooked
    /// </summary>
    public bool ActiveSkewerCooked()
    {
        return quiver[activeSkewer].IsCooked();
    }

    /// <summary>
    /// Add ingredients to active skewer
    /// </summary>
    public void AddToSkewer(IngredientData ingredient)
    {
        //Do not allow adding more ingredients if full or already cooked
        if (!ActiveSkewerCooked() && !ActiveSkewerFull())
        {
            quiver[activeSkewer].PushIngredient(ingredient);
            //UpdateSkewerVisual();
            UpdateUI();
        }
        else if (ActiveSkewerFull())
        {
            sfxPlayer.Play(fullSFX);
        }
    }

    /// <summary>
    /// Remove an ingredient from the active skewer
    /// </summary>
    public IngredientData RemoveFromSkewer()
    {
        IngredientData topIngredient = quiver[activeSkewer].PopIngredient();
        UpdateUI();
        return topIngredient;
    }

    /// <summary>
    /// Clears all ingredients from a skewer but does NOT remove cooked recipes
    /// </summary>
    public void ClearActiveSkewer()
    {
        quiver[activeSkewer].ClearItems();
        quiver[activeSkewer].ResetDictionary();
        UpdateUI();
        //UpdateSkewerVisual();
    }

    /// <summary>
    /// Clears cooked recipes but does NOT remove ingredients
    /// </summary>
    public void ClearActiveRecipe()
    {
        quiver[activeSkewer].finishedRecipe = null;
        UpdateUI();
    }

    /// <summary>
    /// Tells the player's ranged attack to use the current skewer's effect, if any
    /// </summary>
    public RecipeData GetActiveEffect()
    {
        return quiver[activeSkewer].finishedRecipe;
    }

    /// <summary>
    /// Returns the flavor count dictionary of the active skewer
    /// </summary>
    public Dictionary<RecipeData.Flavors, int> GetActiveFlavorDictionary()
    {
        return quiver[activeSkewer].flavorCountDictionary;
    }

    public Dictionary<string, int> GetActiveIngredientDictionary()
    {
        return quiver[activeSkewer].ingredientCountDictionary;
    }
    /// <summary>
    /// Returns the majority flavor on either the active skewer or all skewers. 
    /// Can return either Acquired or None if there is a tie
    /// </summary>
    public RecipeData.Flavors GetMajorityFlavor(bool checkAllSkewers, bool acquiredTies)
    {

        RecipeData.Flavors majorityFlavor = RecipeData.Flavors.None;
        bool tie = true;

        if (!checkAllSkewers)
        {
            if (ActiveSkewerEmpty())
            {
                //Debug.Log("Warning: Cannot find majority flavor on empty skewer");
                return RecipeData.Flavors.None;
             }

            int highestNumber = 0;
            int lastNumber = 0;
            //iterate over all possible flavors
            for (int f = 1; f <= 64; f = f << 1)
            {
                if(quiver[activeSkewer].flavorCountDictionary[(RecipeData.Flavors)f] >= highestNumber)
                {
                    highestNumber = quiver[activeSkewer].flavorCountDictionary[(RecipeData.Flavors)f];

                    if (lastNumber == highestNumber)
                        tie = true;
                    else
                        tie = false;

                    lastNumber = highestNumber;
                    majorityFlavor = (RecipeData.Flavors)f;
                }
            }
            if (!tie)
                return majorityFlavor;
            else
                return acquiredTies ? RecipeData.Flavors.Acquired : RecipeData.Flavors.None;
        }
        else
        {
            if (AllSkewersEmpty())
            {
                //Debug.Log("Warning: Cannot find majority flavor on empty skewers");
                return RecipeData.Flavors.None;
            }

            int highestNumber = 0;
            int lastNumber = 0;
            //iterate over all possible flavors
            for (int s = 0; s < numberOfSkewers; s++)
            {
                for (int f = 1; f <= 64; f = f << 1)
                {
                    if (quiver[s].flavorCountDictionary[(RecipeData.Flavors)f] > highestNumber)
                    {
                        highestNumber = quiver[s].flavorCountDictionary[(RecipeData.Flavors)f];

                        if (lastNumber == highestNumber)
                            tie = true;
                        else
                            tie = false;

                        lastNumber = highestNumber;
                        majorityFlavor = (RecipeData.Flavors)f;
                    }
                }
            }
            if (!tie)
                return majorityFlavor;
            else
                return acquiredTies ? RecipeData.Flavors.Acquired : RecipeData.Flavors.None;
        }
    }

    /// <summary>
    /// Returns true if the inventory has the target number of a certain ingredient. 
    /// Bool parameters set whether to check all skewers and whether to require the exact number.
    /// </summary>
    public bool HasIngredients(string displayName, int target, bool checkAllSkewers = false, bool requireExact = false)
    {
        if (!checkAllSkewers)
        {
            int numIngredient = 0;
            IngredientData[] skewerArray = quiver[activeSkewer].ToArray();
            foreach(IngredientData ingredient in skewerArray)
            {
                if(ingredient.displayName == displayName)
                {
                    numIngredient++;
                }
            }
            if (!requireExact && numIngredient >= target)
                return true;
            else if (requireExact && numIngredient == target)
                return true;
            else
                return false;
        }
        else
        {
            int numIngredient = 0;
            IngredientData[] skewerArray;
            for (int s = 0; s < numberOfSkewers; s++)
            {
                skewerArray = quiver[s].ToArray();
                foreach (IngredientData ingredient in skewerArray)
                {
                    if (ingredient.displayName == displayName)
                    {
                        numIngredient++;
                    }
                }
            }
            if (!requireExact && numIngredient >= target)
                return true;
            else if (requireExact && numIngredient == target)
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// Update the visuals to display the current inventory state
    /// </summary>
    /// 
    /*
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
                if (skewerSprites[i] != null)
                    skewerSprites[i].sprite = dropArray[i].image;
            }
            else
            {
                if(skewerSprites[i] != null)
                    skewerSprites[i].sprite = emptySprite;
            }
        }
    }
    */

    /// <summary>
    /// Handle input for buttons to swap skewers
    /// </summary>
    private void GetSkewerSwapInput()
    {
        if (!CanSwap)
            return;
        if (InputManager.GetButtonDown(Control.SwapSkewerLeft))
        {
            activeSkewer--;
            if (activeSkewer < 0)
                activeSkewer = numberOfSkewers - 1;

            sfxPlayer.Play(swapSFX);
            UpdateUI();
            //DisplayInventory.instance?.SwapHandles(true);
            //Debug.Log("Swapping skewer to " + activeSkewer);
        }
        else if (InputManager.GetButtonDown(Control.SwapSkewerRight))
        {
            activeSkewer++;
            if (activeSkewer >= numberOfSkewers)
                activeSkewer = 0;

            sfxPlayer.Play(swapSFX);
            UpdateUI();
            //DisplayInventory.instance?.SwapHandles(false);
            //Debug.Log("Swapping skewer to " + activeSkewer);
        }
    }

    /// <summary>
    /// Triggers to check if the player is near a campfire
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Campfire")
        {
            //Debug.Log("Player near campfire");
            nearCampfire = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Campfire")
        {
            //Debug.Log("Player left campfire");
            nearCampfire = false;
        }
    }

    public void UpdateUI()
    {
        DisplayInventory.instance?.UpdateSkewerUI();
    }

    #endregion

}
