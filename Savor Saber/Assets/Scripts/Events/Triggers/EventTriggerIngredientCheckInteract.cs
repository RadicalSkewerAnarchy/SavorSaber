using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
public class EventTriggerIngredientCheckInteract : EventTrigger
{
    public string ingredientName;
    public int number = 2;
    public string successflagName;
    private bool playerInRange;

    private void Update()
    {
        if (!playerInRange)
            return;
        if (InputManager.GetButtonDown(Control.Interact))
        {
            FlagManager.SetFlag(successflagName, CheckSkewer().ToString());
            Trigger();
        }           
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }
    private void Start()
    {
        InitializeBase();
    }
    public bool CheckSkewer()
    {
        var inv = player.GetComponent<Inventory>();
        var ingredientStacks = new Stack<IngredientData>[3] { inv.GetActiveSkewer().ingredientStack, inv.GetLeftSkewer().ingredientStack, inv.GetRightSkewer().ingredientStack };
        return ingredientStacks.Any((skewer) => skewer.Count((obj) => obj.displayName == ingredientName) >= number);
    }
}
