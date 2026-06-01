using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoUnlockIngredients : MonoBehaviour
{
    public IngredientData[] ingredients;
    [SerializeField]
    private Inventory inv;
    // Start is called before the first frame update
    void Start()
    {
        foreach(IngredientData ingredient in ingredients)
        {
            inv.UnlockIngredient(ingredient);
        }
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
