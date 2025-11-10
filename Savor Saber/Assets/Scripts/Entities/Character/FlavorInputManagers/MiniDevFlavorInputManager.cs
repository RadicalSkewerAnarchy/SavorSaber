using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MiniDevFlavorInputManager : FlavorInputManager
{
    public GameObject rejectedItemTemplate;

    //events to be called when fed favorite ingredient
    public UnityEvent callOnFeed = new UnityEvent();

    //cutscenes to trigger
    [Header("Cutscene fields")]
    public EventTrigger optionalScene;
    private bool sceneTriggered = false;
    public bool sceneReady = false;    //will the scene be triggered upon next feeding?
    [SerializeField]
    private int maxFood = 25;
    private int currentFood = 0;
    public Slider energySlider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sfxPlayer = GetComponent<AudioSource>();
        //currentRequestState = requestStates[0];

        //StartWeather();
    }

    // when fed compare with desired request
    // if it matches:
    //      then respond to ingredients
    // else:
    //      spawn the same items back
    public override void Feed(IngredientData ingredient, bool fedByPlayer, CharacterData feeder)
    {
        Debug.Log("Mini Devourer fed");
        currentFood++;
        Debug.Log("Current food: " + currentFood + "/" + maxFood);
        if (sfxPlayer != null)
        {
            sfxPlayer.clip = rewardSFX;
            sfxPlayer.Play();
        }
        //if the ingredient is a favorite, add an extra food to the bar
        foreach (IngredientData favorite in favoriteIngredients)
        {
            if(ingredient == favorite)
            {
                currentFood++; 
            }
        }

        UpdateUI();
        //things to happen when food is maxed out
        if(currentFood >= maxFood)
        {
            if (!sceneTriggered && optionalScene != null && sceneReady)
            {
                Debug.Log("Starting devourer scene...");
                optionalScene.Trigger();
                sceneTriggered = true;
            }
            if (callOnFeed != null)
            {
                callOnFeed.Invoke();
            }
        }
    }


    public void SetSceneReady(bool isReady)
    {
        sceneReady = isReady;
    }

    private void UpdateUI()
    {
        if(energySlider != null)
        {
            energySlider.value = ((float)currentFood / (float)maxFood);
        }
    }
}
