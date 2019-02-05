using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SerializableCollections.GUIUtils;

[CustomEditor(typeof(WeatherController))]
public class WeatherControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var w = target as WeatherController;
        if (EditorApplication.isPlaying)
            w.Weather = (WeatherType)EditorGUILayout.EnumPopup(new GUIContent("Weather"), w.Weather);
        else
            w._weather = (WeatherType)EditorGUILayout.EnumPopup(new GUIContent("Weather"), w.Weather);
        SDictionaryGUI.ValueGUI<WeatherData> valGUI = (data) =>
        {
            return EditorGUILayout.ObjectField(data, typeof(WeatherData), false) as WeatherData;
        };
        w.weatherData.DoGUILayout(valGUI, w.weatherData.EnumAddGUI, "Weather Data", true);
        EditorUtils.SetSceneDirtyIfGUIChanged();
    }
}
