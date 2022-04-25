using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusUnlockFruitant : MonoBehaviour
{
    public IngredientData fruitant;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnlockFruitant()
    {
        if (fruitant == null) return;
        PlayerCompanionSummon.instance.UnlockFruitant(fruitant);
    }
}
