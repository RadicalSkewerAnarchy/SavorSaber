using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEatIngredient : MonoBehaviour
{
    /// <summary>
    /// what input axis, if any, should be accepted to trigger this attack
    /// </summary>
    public Control control;
    public InputAxis axis;
    public float IngredientEffectCooldown = 16;

    private Inventory inv;
    private TrustMeter trust; //used because the code to apply flavor effects to the player already exists here

    private WaitForSeconds IngredientEffectTimer;
    // Start is called before the first frame update
    void Start()
    {
        inv = GetComponent<Inventory>();
        trust = GetComponent<TrustMeter>();
        IngredientEffectTimer = new WaitForSeconds(IngredientEffectCooldown);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetButtonDown(control, axis))
        {
            EatIngredient();
        }
    }

    private void EatIngredient()
    {
        IngredientData ingredient = inv.RemoveFromSkewer();
        RecipeData.Flavors flavor = ingredient.flavors;
        trust.SetTrustEffect(flavor);
        StopAllCoroutines();
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        yield return IngredientEffectTimer;
        trust.SetTrustEffect(RecipeData.Flavors.None);
        Debug.Log("Bonus effect should have returned to normal by now...");
        yield return null;
    }
}
