using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrustMeter : MonoBehaviour
{
    [Range(0,100)]
    [SerializeField]
    private int trust = 0;
    [SerializeField]
    private Slider meterSlider;
    private Image meterFill;

    private int maxTrust = 100;
    private int minTrust = 0;
    
    public enum TrustStage {Neutral, Warm, Allied, Honored}
    private TrustStage stage;

    private RecipeData.Flavors companionFlavor;

    //component fields
    private PlayerData somaData;
    private PlayerController somaController;
    private AttackRangedThrowSkewer somaSkewer;
    private float baseSpeed;
    private int baseMaxHealth;

    public GameObject shieldTemplate;
    public GameObject teslaTemplate;
    private GameObject currentObject;

    // Start is called before the first frame update
    void Start()
    {
        somaData = GetComponent<PlayerData>();
        somaController = GetComponent<PlayerController>();
        somaSkewer = GetComponent<AttackRangedThrowSkewer>();
        baseSpeed = somaController.GetSpeed();
        baseMaxHealth = somaData.maxHealth;


        meterFill = meterSlider.fillRect.gameObject.GetComponent<Image>();
        ChangeTrust(10);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChangeTrust(10);
        }
    }

    public void ChangeTrust(int amount)
    {
        trust += amount;
        if (trust > maxTrust)
            trust = maxTrust;
        else if (trust < 0)
            trust = 0;

        //update HUD meter
        if(meterSlider != null)
        {
            meterSlider.value = trust;

            float colorvalue = 0.8f - ((float)trust / 100f) / 2;
            meterFill.color = new Color(1, colorvalue, 0.9f);
        }

        //set trust level
        TrustStage lastStage = stage;
        if(trust < 25)
        {
            stage = TrustStage.Neutral;
        }
        else if(trust < 50)
        {
            stage = TrustStage.Warm;
        }
        else if (trust < 75)
        {
            stage = TrustStage.Allied;
        }
        else if (trust < 100)
        {
            stage = TrustStage.Honored;
        }
        //if the stage has changed, reset the trust effect with the same flavor but new magnitude
        if (stage != lastStage)
            SetTrustEffect(companionFlavor);
    }

    public int GetTrust()
    {
        return trust;
    }

    public void SetTrustEffect(RecipeData.Flavors flavor)
    {
        ResetPlayerParameters();
        switch (flavor)
        {
            //sweet companions buff health and speed
            case RecipeData.Flavors.Sweet:
                Debug.Log("Setting trust effect for sweet");
                somaController.dashRechargeMultiplier = (int)stage + 2; 
                break;
            //spicy companions add DoT effects to skewer throws vs. enemies
            case RecipeData.Flavors.Spicy:
                Debug.Log("Setting trust effect for spicy");
                somaSkewer.extraDamage = (int)stage + 1;
                break;
            //salty companions generate a shield
            case RecipeData.Flavors.Salty:
                Debug.Log("Setting trust effect for salty");
                currentObject = Instantiate(shieldTemplate, transform.position, Quaternion.identity, transform);
                currentObject.GetComponent<SaltShield>().SetOwner(this.gameObject);
                break;
            //sour companions generate a tesla field
            case RecipeData.Flavors.Sour:
                Debug.Log("Setting trust effect for sour");
                currentObject = Instantiate(teslaTemplate, transform);
                break;
        }
    }

    //resets player stats to their default values, unaffected by fruitant companion buffs
    private void ResetPlayerParameters()
    {
        somaController.SetSpeed(baseSpeed);
        somaController.dashRechargeMultiplier = 1;
        somaSkewer.extraDamage = 0;
        somaData.maxHealth = baseMaxHealth;
        if (somaData.health > baseMaxHealth)
            somaData.health = baseMaxHealth;

        //clear any instantiated support objects
        Destroy(currentObject);
    }
}
