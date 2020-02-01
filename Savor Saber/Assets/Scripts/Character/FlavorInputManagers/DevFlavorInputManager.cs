using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevFlavorInputManager : FlavorInputManager
{
    // the weather that needs to be turned on or off
    public List<GameObject> weatherStates;
    public int currentWeatherState = 0;

    public FavoriteFoodBubble speechBubble;

    // the way to transition to the next weather
    public List<IngredientData> requestStates;
    public IngredientData currentRequestState;
    [Header("Cutscene fields")]
    public EventTrigger optionalScene;
    private bool sceneTriggered = false;
    public bool sceneReady = false;

    private void Start()
    {
        InitializeDictionary();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sfxPlayer = GetComponent<AudioSource>();
        currentRequestState = requestStates[0];

        StartWeather();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CycleWeather(1);
        }
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

        if (i != 3)
            correct = false;

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
        if (!sceneTriggered && optionalScene != null && sceneReady)
        {
            optionalScene.Trigger();
            sceneTriggered = true;
        }

        CycleWeather();



    }

    // "rewards" are spawned if the food is rejected!
    public void SpawnReward(IngredientData[] ingredientArray, bool fedByPlayer)
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

    /// <summary>
    /// iterate over the children objects of each state
    /// acquire their WeatherUpdate components and use
    /// WeatherUpdate.WeatherActivate(bool turnOn)
    /// </summary>
    /// <param name="direction">up a state (+), or down a state (-)</param>
    public virtual void CycleWeather(int direction=1)
    {
        currentWeatherState += direction;
        currentWeatherState %= weatherStates.Count;

        WeatherUpdate wu;

        //Debug.Log("!!!CYCLING THE WEATHER!!! Current State = " + currentWeatherState);
        for (int i = 0; i < weatherStates.Count; i++)
        {
            if (i==currentWeatherState)
            {
                // activate this weather
                foreach(Transform t in weatherStates[i].transform)
                {
                    //Debug.Log("State = " + i + ":: About to ACTIVATE some Weather effects...");
                    wu = t.GetComponent<WeatherUpdate>();
                    wu.WeatherActivate(true);
                }
            }
            else
            {
                // deactivate this weather
                foreach (Transform t in weatherStates[i].transform)
                {
                    //Debug.Log("State = " + i + ":: About to DEactivate some Weather effects...");
                    wu = t.GetComponent<WeatherUpdate>();
                    wu.WeatherActivate(false);
                }
            }
        }

        // update new food request
        currentRequestState = requestStates[currentWeatherState];
        speechBubble.favoriteFood1 = requestStates[currentWeatherState];
        speechBubble.favoriteFood2 = requestStates[currentWeatherState];
        speechBubble.favoriteFood3 = requestStates[currentWeatherState];
        speechBubble.reset = true;
    }

    public void SetSceneReady(bool isReady)
    {
        sceneReady = isReady;
    }
}
