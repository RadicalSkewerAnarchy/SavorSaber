using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializableCollections;

/// <summary>
/// A data class for the different times of the day (e.g morning)
/// Used to swap out lighting profiles for different weathers and evironments
/// </summary>
[System.Serializable]
public class TimeOfDayData
{
    /// <summary>The color of the ambient light</summary>
    public Color lightColor = new Color(1,1,1);
    /// <summary>A serializable version of a dictionary mapping TimeOfDay's to relevant data</summary>
    [System.Serializable] public class Dict : SDictionary<TimeOfDay, TimeOfDayData> { }
}
