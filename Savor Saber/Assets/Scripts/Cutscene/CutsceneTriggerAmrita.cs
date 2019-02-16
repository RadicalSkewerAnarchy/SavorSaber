using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
public class CutsceneTriggerAmrita : Cutscene
{
    public string ingredientName;
    private void Start()
    {
        events = GetComponents<EventScript>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var inv = player.GetComponent<Inventory>();
        if ((inv.GetActiveSkewer().ingredientStack.Count >= 1 && inv.GetActiveSkewer().ingredientStack.All((ing) => ing.displayName == ingredientName)) ||
            (inv.GetLeftSkewer().ingredientStack.Count >= 1 && inv.GetLeftSkewer().ingredientStack.All((ing) => ing.displayName == ingredientName))   ||
            (inv.GetRightSkewer().ingredientStack.Count >= 1 && inv.GetRightSkewer().ingredientStack.All((ing) => ing.displayName == ingredientName)))
        {
            Debug.Log("Triggering Event: " + name);
            Activate();
        }
    }
}
