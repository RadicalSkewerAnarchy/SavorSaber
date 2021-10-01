using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevFlavorInputManager : FlavorInputManager
{
    public GameObject rejectedItemTemplate;
    // the weather that needs to be turned on or off
    public List<GameObject> weatherStates;
    public int currentWeatherState = 0;

    //reference to the speech bubble displaying requested food
    public FavoriteFoodBubble speechBubble;

    // the way to transition to the next weather
    public List<IngredientData> requestStates;
    public IngredientData currentRequestState;

    //cutscenes to trigger
    [Header("Cutscene fields")]
    public EventTrigger optionalScene;
    private bool sceneTriggered = false;
    //will the scene be triggered upon next feeding?
    public bool sceneReady = false;

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
    public override void Feed(IngredientData ingredient, bool fedByPlayer, CharacterData feeder)
    {
        Debug.Log("DEVOURER HAS BEEN FED. GOD HAVE MERCY ON OUR SOULS.");
        Debug.Log("Fed by player: " + fedByPlayer);
        IngredientData check = requestStates[Mathf.Clamp(currentWeatherState, 0, requestStates.Count - 1)];


        if (ingredient == check)
        {
            Debug.Log("Detected correct ingredients for request state");
            CycleWeather();

            if (sfxPlayer != null)
            {
                sfxPlayer.clip = rewardSFX;
                sfxPlayer.Play();
            }
            if (!sceneTriggered && optionalScene != null && sceneReady)
            {
                Debug.Log("Starting devourer scene...");
                optionalScene.Trigger();
                sceneTriggered = true;
            }
        }
        else
        {
            IngredientData[] ingredientArray = { ingredient };
            SpawnReward(ingredientArray, fedByPlayer);
        }
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
            var spawn = Instantiate(rejectedItemTemplate, transform.position, Quaternion.identity);
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
        Debug.Log("Beginning weather cycle");
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
        var state = Mathf.Clamp(currentWeatherState, 0, requestStates.Count-1);
        currentRequestState = requestStates[state];
        speechBubble.favoriteFood1 = requestStates[state];
        speechBubble.favoriteFood2 = requestStates[state];
        speechBubble.favoriteFood3 = requestStates[state];
        speechBubble.reset = true;
    }

    public void SetSceneReady(bool isReady)
    {
        sceneReady = isReady;
    }
}
