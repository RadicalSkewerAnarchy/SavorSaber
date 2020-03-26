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

    public override void Feed(IngredientData ingredient, bool fedByPlayer, CharacterData feeder)
    {
        int damageDone = 0;
        if (ingredient.displayName == "Bullet")
        {
            //characterData.DoDamage(damageFromBulletSkewer, true);
            damageDone += damageFromBulletSkewer;
        }
        else
        {
            //characterData.DoDamage(damageFromBaseSkewer, false);
            if (!characterData.armored)
                damageDone += damageFromBaseSkewer;

            //spit out the rejected object
            GameObject rejectedObject = Instantiate(rejectedObjectTemplate, transform.position, Quaternion.identity);
            SpriteRenderer rejectedSR = rejectedObject.GetComponent<SpriteRenderer>();
            SkewerableObject rejectedSO = rejectedObject.GetComponent<SkewerableObject>();
            rejectedSR.sprite = ingredient.image;
            rejectedSO.data = ingredient;
        }
        characterData.DoDamage(damageDone, true);
    }
    
}
