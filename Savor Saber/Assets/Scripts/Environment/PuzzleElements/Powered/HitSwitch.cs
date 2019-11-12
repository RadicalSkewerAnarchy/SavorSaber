using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HitSwitch : FlavorInputManager
{

    public PoweredObject[] TargetObjects;
    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Feed(IngredientData[] ingredientArray, bool fedByPlayer)
    {
        if (ingredientArray.Length >= 0)
        {
            if (active)
            {
                foreach (PoweredObject target in TargetObjects)
                {
                    target.ShutOff();
                }
                active = false;
            }
            else
            {
                foreach (PoweredObject target in TargetObjects)
                {
                    target.TurnOn();
                }
                active = true;
            }

        }
    }

}
