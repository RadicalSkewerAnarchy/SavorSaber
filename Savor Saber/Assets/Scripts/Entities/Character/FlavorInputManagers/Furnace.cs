using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class Furnace : FlavorInputManager
{
    public bool isToggle = true;
    //public int cooldownTime = 0; //if 0, effect is permanent
    public PoweredObject[] TargetObjects;
    protected bool targetsActive = false;
    //protected AudioSource burnSFXPlayer;

    public bool active = true;
    public RecipeData.Flavors favoriteFlavors;
    public bool useFavoriteFlavors = true;

    // Start is called before the first frame update
    void Start()
    {
        //burnSFXPlayer = GetComponent<AudioSource>();
        sfxPlayer = GetComponent<AudioSource>();

        //if "none" is marked as a favorite flavor, disregard the whole system and take anything.
        if (favoriteFlavors == RecipeData.Flavors.None)
        {
            useFavoriteFlavors = false;
        }

        //set animation according to favorite flavor
        Animator animator = GetComponent<Animator>();
        if(favoriteFlavors == RecipeData.Flavors.Sour)
        {
            animator.SetInteger("Color", 2);
        }
        else if(favoriteFlavors == RecipeData.Flavors.Spicy)
        {
            animator.SetInteger("Color", 1);
        }
        else
        {
            animator.SetInteger("Color", 0);
        }
    }

    public override void Feed(IngredientData ingredient, bool fedByPlayer, CharacterData feeder)
    {
        if (!active)
            return;

        if (useFavoriteFlavors)
        {
            if ((favoriteFlavors & ingredient.flavors) == 0)
            {
                SpawnRejectedIngredient(ingredient);
                return;
            }
                
        }
        if (targetsActive)
        {
            foreach (PoweredObject target in TargetObjects)
            {
                target.ShutOff();
            }

            if(isToggle)
                targetsActive = false;
        }
        else
        {
            foreach (PoweredObject target in TargetObjects)
            {
                target.TurnOn();
            }

            if(isToggle)
                targetsActive = true;
        }
        sfxPlayer.clip = rewardSFX;
        sfxPlayer.Play();
    }
}
