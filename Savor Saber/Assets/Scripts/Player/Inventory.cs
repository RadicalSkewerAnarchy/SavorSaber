using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for packaging a stack of ingredients and a completed recipe.
/// </summary>
public class Skewer
{
    //fields
    public Stack<IngredientData> ingredientStack = new Stack<IngredientData>();

    public RecipeData finishedRecipe = null;


    //methods
    public int GetCount()
    {
        return ingredientStack.Count;
    }

    public void PushIngredient(IngredientData ingredient)
    {
        ingredientStack.Push(ingredient);
    }

    public IngredientData PopIngredient()
    {
        return ingredientStack.Pop();
    }

    public IngredientData[] ToArray()
    {
        return ingredientStack.ToArray();
    }

    public bool IsCooked()
    {
        return finishedRecipe != null;
    }

    public void Clear()
    {
        ingredientStack.Clear();
    }
}

/// <summary>
/// Controller for functions related to managing and displaying inventory
/// </summary>
/// 
[RequireComponent(typeof(AttackRanged))]
public class Inventory : MonoBehaviour {

    #region fields

    /// <summary>
    /// Fields related to inventory visual representation
    /// </summary>
    public int maxItemsPerSkewer = 3;
    public Image[] skewerSprites = new Image[3];
    public Sprite emptySprite;


    /// <summary>
    /// Fields related to actual inventory tracking
    /// </summary>
    private int activeSkewer = 0;
    private int numberOfSkewers = 3;
    private Skewer[] quiver;

    /// <summary>
    /// Fields related to cooking
    /// </summary>
    public GameObject recipeDatabaseObject;
    private RecipeDatabase recipeDatabase;
    public AttackRanged rangedAttack;
    //[System.NonSerialized]
    public bool nearCampfire = false;

    #endregion

    void Start ()
    {

        quiver = new Skewer[numberOfSkewers];
        quiver[0] = new Skewer();
        quiver[1] = new Skewer();
        quiver[2] = new Skewer();

        recipeDatabase = recipeDatabaseObject.GetComponent<RecipeDatabase>();
        rangedAttack = GetComponent<AttackRanged>();
    }

    private void Update()
    {
        GetCookingInput();
        GetSkewerSwapInput();

        //disable the player's ranged attack if the active skewer is not cooked
        if (ActiveSkewerCooked())
            rangedAttack.enabled = true;
        else
            rangedAttack.enabled = false;

        //clear the skewer of recipes and ingredients after throwing
        if (quiver[activeSkewer].finishedRecipe != null && rangedAttack.Attacking)
        {
            quiver[activeSkewer].finishedRecipe = null;
            ClearActiveSkewer();
        }

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
    /// Returns true if the active skewer is full
    /// </summary>
    public bool ActiveSkewerFull()
    {
        return (quiver[activeSkewer].GetCount() == maxItemsPerSkewer);
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
            UpdateSkewerVisual();
        }
    }

    /// <summary>
    /// Remove an ingredient from the active skewer
    /// </summary>
    public IngredientData RemoveFromSkewer()
    {
        IngredientData topIngredient = quiver[activeSkewer].PopIngredient();
        return topIngredient;
    }

    /// <summary>
    /// Clears all ingredients from a skewer but does NOT remove cooked recipes
    /// </summary>
    public void ClearActiveSkewer()
    {
        quiver[activeSkewer].Clear();
        UpdateSkewerVisual();
    }

    /// <summary>
    /// Tells the player's ranged attack to use the current skewer's effect, if any
    /// </summary>
    private void SetActiveEffect()
    {
        rangedAttack.effectRecipeData = quiver[activeSkewer].finishedRecipe;
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
    #endregion

    #region cooking functions

    private void GetSkewerSwapInput()
    {
        if (Input.GetButtonDown("SwapLeft"))
        {
            activeSkewer--;
            if (activeSkewer < 0)
                activeSkewer = numberOfSkewers - 1;

            Debug.Log("Swapping skewer to " + activeSkewer);
            UpdateSkewerVisual();
            SetActiveEffect();
        }
        else if (Input.GetButtonDown("SwapRight"))
        {
            activeSkewer++;
            if (activeSkewer >= numberOfSkewers)
                activeSkewer = 0;

            Debug.Log("Swapping skewer to " + activeSkewer);
            UpdateSkewerVisual();
            SetActiveEffect();
        }
    }

    private void GetCookingInput()
    {
        //Press C to cook
        if (Input.GetKeyDown(KeyCode.C) && nearCampfire)
        {
            if (quiver[activeSkewer].GetCount() > 0)
            {
                LongCook();
            }
            else if (quiver[activeSkewer].GetCount() <= 0)
            {
                Debug.Log("Your inventory is empty, cannot cook");
            }
        }
        else if (Input.GetKeyDown(KeyCode.C) && !nearCampfire)
        {
            if (quiver[activeSkewer].GetCount() > 0)
            {
                ShortCook();
            }
            else if (quiver[activeSkewer].GetCount() <= 0)
            {
                Debug.Log("Your inventory is empty, cannot cook");
            }
        }
    }
    /// <summary>
    /// Execute a long cook, access full database
    /// </summary>
    private void LongCook()
    {
        Debug.Log("Cooking at campfire...");
        RecipeData cookedRecipe = recipeDatabase.CompareToRecipes(quiver[activeSkewer].ingredientStack);
        //if it actually returned a recipe match
        if(cookedRecipe != null)
        {
            quiver[activeSkewer].finishedRecipe = cookedRecipe;
            SetActiveEffect();
            ClearActiveSkewer();
        }

    }

    /// <summary>
    /// Execute a short cook, only use recipes not tagged complex
    /// </summary>
    private void ShortCook()
    {
        Debug.Log("Cooking in the field...");
        RecipeData cookedRecipe = recipeDatabase.CompareToSimpleRecipes(quiver[activeSkewer].ingredientStack);
        //if it actually returned a recipe match
        if (cookedRecipe != null)
        {
            quiver[activeSkewer].finishedRecipe = cookedRecipe;
            SetActiveEffect();
            ClearActiveSkewer();
        }
    }

    /// <summary>
    /// Triggers to check if the player is near a campfire
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Campfire")
        {
            Debug.Log("Player near campfire");
            nearCampfire = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Campfire")
        {
            Debug.Log("Player left campfire");
            nearCampfire = false;
        }
    }

    #endregion

}
