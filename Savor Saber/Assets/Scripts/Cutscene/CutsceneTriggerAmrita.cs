using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
public class CutsceneTriggerAmrita : Cutscene
{
    public string ingredientName;
    public int number = 2;
    private void Start()
    {
        InitializeBase();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var inv = player.GetComponent<Inventory>();
        var ingredientStacks = new Stack<IngredientData>[3] { inv.GetActiveSkewer().ingredientStack, inv.GetLeftSkewer().ingredientStack, inv.GetRightSkewer().ingredientStack };
        if (ingredientStacks.Any((skewer) => skewer.Count((obj) => obj.displayName == ingredientName) >= number))
        {
            Debug.Log("Triggering Event: " + name);
            Activate();
        }
    }
}
