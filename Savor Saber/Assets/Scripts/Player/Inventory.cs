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
    private Stack<IngredientData> ingredientStack = new Stack<IngredientData>();

    public RecipeData finishedRecipe = null;


    //methods
    public int GetCount()
    {
        return ingredientStack.Count;
    }

    public Stack<IngredientData> GetStack()
    {
        return ingredientStack;
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
    /// Fields related to actual inventory tracking
    /// </summary>
    private int activeSkewer = 0;
    private Skewer[] quiver = new Skewer[3];

    /// <summary>
    /// Fields related to cooking
    /// </summary>
    public GameObject recipeDatabaseObject;
    private RecipeDatabase recipeDatabase;

    #endregion

    void Start () {
        quiver[0] = new Skewer();
        quiver[1] = new Skewer();
        quiver[2] = new Skewer();

        recipeDatabase = recipeDatabaseObject.GetComponent<RecipeDatabase>();
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
    /// Clears all ingredients from a skewer
    /// </summary>
    public void ClearActiveSkewer()
    {
        quiver[activeSkewer].Clear();
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

    private void LongCook()
    {
        RecipeData cookedRecipe = recipeDatabase.CompareToRecipes(quiver[activeSkewer].GetStack());
        //if it actually returned a recipe match
        if(cookedRecipe != null)
        {
            quiver[activeSkewer].finishedRecipe = cookedRecipe;
        }

    }
}
