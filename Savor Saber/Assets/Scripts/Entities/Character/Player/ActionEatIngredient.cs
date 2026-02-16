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
    private SelfIngredientEffect selfEffect; //used because the code to apply flavor effects to the player already exists here
    private PlayerData PData;

    private WaitForSeconds IngredientEffectTimer;
    public GameObject SaltyEffectTemplate;
    public GameObject SourEffectTemplate;

    private WaitForSeconds effectTimer;
    private GameObject instantiatedEffect;
    private int potency;

    // Start is called before the first frame update
    void Start()
    {
        inv = GetComponent<Inventory>();
        selfEffect = GetComponent<SelfIngredientEffect>();
        IngredientEffectTimer = new WaitForSeconds(IngredientEffectCooldown);
        PData = GetComponent<PlayerData>();
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
        potency = inv.GetEnergy();
        inv.SetEnergy(0);
        if (potency == 0) return;


        IngredientData[] ingredientArray = inv.GetActiveSkewer().ToArray();
        foreach (IngredientData ingredient in ingredientArray)
        {
            if ((ingredient.flavors & RecipeData.Flavors.Sweet) > 0)
            {
                //heal
                PData.DoHeal(potency);
                Debug.Log("Eating with sweet flavor");
                StartCoroutine(durationTimer());
            }
            if ((ingredient.flavors & RecipeData.Flavors.Spicy) > 0)
            {
                //instantiate DoT applicator
                Debug.Log("Eating with spicy flavor");
                StartCoroutine(durationTimer());
            }
            if ((ingredient.flavors & RecipeData.Flavors.Sour) > 0)
            {
                instantiatedEffect = Instantiate(SourEffectTemplate, transform.position, Quaternion.identity, this.gameObject.transform);
                Debug.Log("Eating with sour flavor");
                StartCoroutine(durationTimer());
            }
            if ((ingredient.flavors & RecipeData.Flavors.Salty) > 0)
            {
                //shield
                instantiatedEffect = Instantiate(SaltyEffectTemplate, transform.position, Quaternion.identity, this.gameObject.transform);
                Debug.Log("Eating with salty flavor");
                StartCoroutine(durationTimer());
            }
        }

    }

    private IEnumerator Cooldown()
    {
        yield return IngredientEffectTimer;
        selfEffect.SetPassiveEffect(RecipeData.Flavors.None);
        Debug.Log("Bonus effect should have returned to normal by now...");
        yield return null;
    }

    private IEnumerator durationTimer()
    {
        yield return new WaitForSeconds(potency);
        Destroy(instantiatedEffect);
    }
}
