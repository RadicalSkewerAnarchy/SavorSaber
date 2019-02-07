using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeatherData", menuName = "WeatherData")]
public class WeatherData : ScriptableObject
{
    /// <summary>The in-game name of this weather</summary>
    public string displayName;
    /// <summary>The game object responsible for creating the weather effects (should be a prefab)</summary>
    public GameObject effectCreator;
    /// <summary>The sound to play in the background while this weather is happening</summary>
    public AudioClip ambientSound;
    /// <summary>The lighting data to override for any TimeOfDay key defined. 
    /// If a key is not in the dictionary, the Clear Weather data will be used</summary>
    public TimeOfDayData.Dict lightingOverrides = new TimeOfDayData.Dict();
}
