using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CharacterData))]
public class DroneFlavorInput : FlavorInputManager
{
    //private int slowDuration = 6;
    //private bool slowed = false;
    //private int slowTimer = 0;
    //public bool hasElectricField = false;
    //private ElectricAOE electricField;

    private WaitForSeconds OneSecondTic = new WaitForSeconds(1);
    public int damageFromBaseSkewer = 1;
    public int damageFromBulletSkewer = 2;

    // Start is called before the first frame update
    void Start()
    {
        InitializeDictionary();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterData = GetComponent<AIData>();

        //if (hasElectricField)
            //electricField = GetComponentInChildren<ElectricAOE>();
    }

    public override void Feed(IngredientData[] ingredientArray, bool fedByPlayer)
    {

        //no longer processes flavors, so this is no longer useful but kept around just in case
        #region Old Flavor Processing
        //Debug.Log("Skewer of size " + ingredientArray.Length);
        /*
        for (int i = 0; i < ingredientArray.Length; i++)
        {
            IngredientData ingredient = ingredientArray[i];
            if (ingredientCountDictionary.ContainsKey(ingredient))
            {
                ingredientCountDictionary[ingredient] = ingredientCountDictionary[ingredient] + 1;
                //Debug.Log("Ate one " + ingredient.displayName);
            }
            else
            {
                ingredientCountDictionary.Add(ingredient, 1);
                //Debug.Log("Ate one " + ingredient.displayName);
            }

            // favorite flavors
            
            for (int f = 1; f <= 64; f = f << 1)
            {

                if ((f & (int)ingredient.flavors) > 0)
                {
                    RecipeData.Flavors foundFlavor = (RecipeData.Flavors)f;
                    flavorCountDictionary[foundFlavor] = flavorCountDictionary[foundFlavor] + 1;
                    Debug.Log(ingredient.displayName + " has flavor " + foundFlavor);
                }
            }
            
        }
        */
        //RespondToIngredients(fedByPlayer);

        // fruitants no longer spawn a reward
        //SpawnReward(ingredientArray, fedByPlayer);
        #endregion

        int damageDone = 0;

        foreach(IngredientData data in ingredientArray)
        {
            if(data.displayName == "Bullet")
            {
                //characterData.DoDamage(damageFromBulletSkewer, true);
                damageDone += damageFromBulletSkewer;
            }
            else
            {
                //characterData.DoDamage(damageFromBaseSkewer, false);
                if(!characterData.armored)
                    damageDone += damageFromBaseSkewer;

                //spit out the rejected object
                GameObject rejectedObject = Instantiate(rejectedObjectTemplate, transform.position, Quaternion.identity);
                SpriteRenderer rejectedSR = rejectedObject.GetComponent<SpriteRenderer>();
                SkewerableObject rejectedSO = rejectedObject.GetComponent<SkewerableObject>();
                rejectedSR.sprite = data.image;
                rejectedSO.data = data;
            }
        }
        characterData.DoDamage(damageDone, true);

    }

    #region Old Code

    public override void RespondToIngredients(bool fedByPlayer)
    {
        #region Old Flavor Responses
        /*
        //handle spicy - Does damage over time
        if (flavorCountDictionary[RecipeData.Flavors.Spicy] > 0)
        {
            spriteRenderer.color = Color.red;
            DamageOverTime(2 * flavorCountDictionary[RecipeData.Flavors.Spicy], dotTicLength);

            flavorCountDictionary[RecipeData.Flavors.Spicy] = 0;
        }

        //handle sweet - Pacifies drone
        if (flavorCountDictionary[RecipeData.Flavors.Sweet] > 0)
        {
            flavorCountDictionary[RecipeData.Flavors.Sweet] = 0;
        }

        //handle sour - Does more damage to high-health targets
        if(flavorCountDictionary[RecipeData.Flavors.Sour] > 0)
        {

            float health = characterData.health;
            //damage boost equals 1/6 of the creature's health at one flavor, 1/2 health at full flavor
            int damage = 1 + 1 * (int)(characterData.health / 6) * flavorCountDictionary[RecipeData.Flavors.Sour];
            characterData.DoDamage(damage);

            if (hasElectricField && electricField != null)
                electricField.DisableForSeconds();

            flavorCountDictionary[RecipeData.Flavors.Sour] = 0;
        }

        //handle salty - Reduce Speed
        if (flavorCountDictionary[RecipeData.Flavors.Salty] > 0)
        {
            
            //if you're already slowed, just increase the duration. Don't stack coroutines.
            if (!slowed)
                StartCoroutine(SlowDown());
            else
                slowTimer += 2 * flavorCountDictionary[RecipeData.Flavors.Salty];

            flavorCountDictionary[RecipeData.Flavors.Salty] = 0;
        }
        */
        #endregion

    }
    /*
    private IEnumerator SlowDown()
    {
        slowed = true;
        float baseSpeed = characterData.Speed;
        characterData.Speed = baseSpeed / (1 + flavorCountDictionary[RecipeData.Flavors.Salty]);

        Debug.Log("Begin slow. Speed reduced to " + characterData.Speed);

        slowTimer = slowDuration;
        while(slowTimer > 0)
        {
            Debug.Log("Seconds remaining on slow: " + slowTimer);
            yield return OneSecondTic;
            slowTimer--;
        }
        
        characterData.Speed = baseSpeed;
        slowed = false;
        Debug.Log("End slow. Speed returned to " + characterData.Speed);
    }
    */
    #endregion
}
