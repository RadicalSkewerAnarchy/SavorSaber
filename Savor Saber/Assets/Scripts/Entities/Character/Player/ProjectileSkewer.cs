using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileSkewer : BaseProjectile
{

    //SignalApplication signalApplication;
    //GameObject signal;
    //Dictionary<string, float> moodMod = new Dictionary<string, float>();
    //bool detonating = false;
    public GameObject audioPlayer;
    [HideInInspector]
    public bool fed = false;
    private bool hitEnemy = false;

    /// <summary>
    /// how much of each flavor is present on the skewer
    /// </summary>
    public Dictionary<RecipeData.Flavors, int> flavorCountDictionary;
    public Dictionary<string, int> ingredientCountDictionary;
    [HideInInspector]
    public IngredientData[] ingredientArray;

    public GameObject templateBonusSweet;
    public GameObject templateBonusSpicy;
    public GameObject templateBonusSour;
    public GameObject templateBonusSalty;

    // Start is called before the first frame update
    void Start()
    {
        projectileCollider = GetComponent<CapsuleCollider2D>();
        projectileCollider.size = new Vector2(projectileLength, projectileWidth);

        // set projectile velocity vector
        SetGeometry();
        spawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        //ignore specified target classes
        Debug.Log("Skewer collided with " + go);
        foreach (string tag in tagsToIgnore)
        {
            if (go.tag == tag)
                return;
        }

        if (!fed)
        { 
            if (collision.tag == "SkewerableObject")
                return;
            //Debug.Log("Skewer collided with " + collision.gameObject);

            if (ingredientArray != null)
            {
                //if what you hit is destructible environment
                if (go.tag == "ThrowThrough")
                {
                    DestructableEnvironment envData = go.GetComponent<DestructableEnvironment>();
                    //myCharData.damageDealt += (int)projectileDamage;
                    Debug.Log("Dealing DMG to destructible environment");
                    if (envData != null && flavorCountDictionary[RecipeData.Flavors.Spicy] > 0)
                    {
                        envData.Health -= (int)Mathf.Max(projectileDamage, 1);
                        //if (!penetrateTargets)
                        //Destroy(this.gameObject);
                        return;
                    }
                }
                //check to see if the thing we hit has a FlavorInputManager
                FlavorInputManager flavorInput = collision.gameObject.GetComponent<FlavorInputManager>();
                if (flavorInput != null)
                {
                    Debug.Log("Found FlavorInputManager");
                    //Debug.Log("Flavor input of " + collision.gameObject + " not null");
                    flavorInput.Feed(ingredientArray[0], true, myCharData);
                    fed = true;
                    if (go.tag == "Predator") hitEnemy = true;
                }
                ApplyBonusEffects(go);
                Destroy(this.gameObject);
            }
            else if (!penetrateTargets)
                Destroy(this.gameObject);
        }
    }

    private void ApplyBonusEffects(GameObject target)
    {
        foreach(IngredientData ingredient in ingredientArray)
        {
            if((ingredient.flavors & RecipeData.Flavors.Sweet) > 0)
            {
                //lifesteal
                Debug.Log("Impact with sweet flavor");
                CharacterData attackerData = attacker.GetComponent<CharacterData>();
                if(attackerData != null)
                {
                    attackerData.DoHeal(1);
                }
            }
            if ((ingredient.flavors & RecipeData.Flavors.Spicy) > 0)
            {
                //instantiate DoT applicator
                GameObject bonus = Instantiate(templateBonusSpicy, transform.position, Quaternion.identity);
                SkewerBonusEffect dot = bonus.GetComponent<SkewerBonusEffect>();
                dot.SetTarget(target, 5);
                Debug.Log("Impact with spicy flavor");
            }
            if ((ingredient.flavors & RecipeData.Flavors.Sour) > 0)
            {
                //instantiate Tesla Field
                GameObject bonus = Instantiate(templateBonusSour, transform.position, Quaternion.identity);
                Debug.Log("Impact with sour flavor");
            }
            if ((ingredient.flavors & RecipeData.Flavors.Salty) > 0)
            {
                //immobilize target
                GameObject bonus = Instantiate(templateBonusSalty, transform.position, Quaternion.identity);
                Debug.Log("Impact with salty flavor");
                SkewerBonusEffect slow = bonus.GetComponent<SkewerBonusEffect>();
                slow.SetTarget(target, 5);
            }
        }
    }

}
