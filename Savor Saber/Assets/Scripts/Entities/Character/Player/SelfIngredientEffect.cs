using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfIngredientEffect : MonoBehaviour
{
    [Range(0,100)]
    [SerializeField]
    private int trust = 0;
    private TrustEffectDisplay trustText;
    private int maxTrust = 100;
    private int minTrust = 0;   
    public enum UpgradeTier {Neutral = 1, Warm, Allied, Honored}
    private UpgradeTier stage;

    //component fields
    private PlayerData somaData;
    private PlayerController somaController;
    private AttackRangedThrowSkewer somaSkewer;
    private float baseSpeed;
    private int baseMaxHealth;
    private Inventory playerInventory;
    public GameObject saltyTemplate;
    public GameObject sourTemplate;
    public GameObject spicyTemplate;
    private GameObject currentObject;
    private bool currentObjectFollowsPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        somaData = GetComponent<PlayerData>();
        somaController = GetComponent<PlayerController>();
        somaSkewer = GetComponent<AttackRangedThrowSkewer>();
        baseSpeed = somaController.GetSpeed();
        baseMaxHealth = somaData.maxHealth;
        stage = UpgradeTier.Neutral;
    }

    // Update is called once per frame
    void Update()
    {

        //If the passive effect is a gameobject that should follow the player, do it now
        if (currentObjectFollowsPlayer)
        {
            currentObject.transform.position = transform.position;
        }
    }

    public void SetPassiveEffect(RecipeData.Flavors flavor)
    {
        ResetPlayerParameters();
        int flavorStrength = playerInventory.GetFlavorStrength(flavor);
        switch (flavor)
        {
            //sweet companions buff health and speed
            case RecipeData.Flavors.Sweet:
                //Debug.Log("Setting trust effect for sweet");
                currentObjectFollowsPlayer = false;
                somaController.dashRechargeMultiplier = flavorStrength + 1;
                somaController.maxDashes = 3 + flavorStrength;
                somaSkewer.SetFlavor(RecipeData.Flavors.Sweet, flavorStrength);
                trustText.UpdateDisplayText("Ing. Bonus: +" + flavorStrength + " dash");
                break;
            //spicy companions add DoT effects to skewer throws vs. enemies
            case RecipeData.Flavors.Spicy:
                //Debug.Log("Setting trust effect for spicy");
                currentObjectFollowsPlayer = false;
                somaSkewer.SetFlavor(RecipeData.Flavors.Spicy, flavorStrength);
                somaSkewer.spicyTemplate = spicyTemplate;
                trustText.UpdateDisplayText("Ing. Bonus: +" + flavorStrength + " DoT");
                break;
            //salty companions generate a shield
            case RecipeData.Flavors.Salty:
                //Debug.Log("Setting trust effect for salty");
                currentObjectFollowsPlayer = true;
                currentObject = Instantiate(saltyTemplate, transform.position, Quaternion.identity);
                currentObject.GetComponent<SaltShield>().SetOwner(this.gameObject);
                somaSkewer.SetFlavor(RecipeData.Flavors.Salty, flavorStrength);
                trustText.UpdateDisplayText("Ing. Bonus: Shield");
                break;
            //sour companions generate a tesla field
            case RecipeData.Flavors.Sour:
                //Debug.Log("Setting trust effect for sour");
                currentObjectFollowsPlayer = false;
                somaSkewer.SetFlavor(RecipeData.Flavors.Sour, flavorStrength);
                somaSkewer.sourTemplate = sourTemplate;
                somaSkewer.spawnEffectOnMiss = true;
                trustText.UpdateDisplayText("Ing. Bonus: Tesla Skewers");
                break;

            case RecipeData.Flavors.None:
                Debug.Log("Setting trust effect for None");
                somaSkewer.SetFlavor(RecipeData.Flavors.None, 1);
                ResetPlayerParameters();
                break;
        }
    }

    //resets player stats to their default values, unaffected by fruitant companion buffs
    private void ResetPlayerParameters()
    {
        somaController.SetSpeed(baseSpeed);
        somaController.maxDashes = 3;
        somaController.dashRechargeMultiplier = 1;
        somaSkewer.extraDamage = 0;
        somaData.maxHealth = baseMaxHealth;
        if (somaData.health > baseMaxHealth)
            somaData.health = baseMaxHealth;

        //clear any instantiated support objects
        Destroy(currentObject);
    }
}
