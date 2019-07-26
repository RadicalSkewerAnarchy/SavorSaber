using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevFlavorInputManager : FlavorInputManager
{
    // the weather that needs to be turned on or off
    public List<GameObject> weatherStates;
    public int currentWeatherState = 0;

    // the way to transition to the next weather
    public List<IngredientData> requestStates;
    public IngredientData currentRequestState;

    private void Start()
    {
        InitializeDictionary();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sfxPlayer = GetComponent<AudioSource>();
        currentRequestState = requestStates[0];

        StartWeather();
    }

    // when fed compare with desired request
    // if it matches:
    //      then respond to ingredients
    // else:
    //      spawn the same items back
    public override void Feed(IngredientData[] ingredientArray, bool fedByPlayer)
    {
        IngredientData check = requestStates[currentWeatherState];

        bool correct = true;
        int i = 0;
        foreach(var id in ingredientArray)
        {
            if (id != check)
            {
                correct = false;
                break;
            }

            i++;
        }

        if (correct)
        {
            RespondToIngredients(fedByPlayer);
        }
        else
        {
            SpawnReward(ingredientArray, fedByPlayer);
        }
    }

    // cycle the weather
    public override void RespondToIngredients(bool fedByPlayer)
    {
        if (sfxPlayer != null)
        {
            sfxPlayer.clip = rewardSFX;
            sfxPlayer.Play();
        }
        CycleWeather();
    }

    // "rewards" are spawned if the food is rejected!
    public override void SpawnReward(IngredientData[] ingredientArray, bool fedByPlayer)
    {
        if (sfxPlayer != null)
        {
            sfxPlayer.clip = rejectSFX;
            sfxPlayer.Play();
        }
        foreach (var food in ingredientArray)
        {
            // spawn food
            var spawn = Instantiate(rewardItem, transform.position, Quaternion.identity);
            spawn.GetComponent<SkewerableObject>().data = food;
        }
    }

    public virtual void StartWeather()
    {
        CycleWeather(0);
    }

    public virtual void CycleWeather(int direction=1)
    {
        currentWeatherState += direction;
        currentWeatherState %= weatherStates.Count;

        for(int i = 0; i < weatherStates.Count; i++)
        {
            if (i==currentWeatherState)
            {
                // activate this weather
                weatherStates[i].SetActive(true);
            }
            else
            {
                //deactivate these weathers
                weatherStates[i].SetActive(false);
            }
        }

        // update new food request
        currentRequestState = requestStates[currentWeatherState];
    }
}
