using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SerializableCollections.GUIUtils;

[CustomEditor(typeof(WeatherController))]
public class WeatherControllerEditor : Editor
{
    private WeatherType setTo = WeatherType.Sun;
    public override void OnInspectorGUI()
    {
        var w = target as WeatherController;
        if(EditorApplication.isPlaying)
        {
            EditorGUILayout.LabelField(new GUIContent("Weather: " + w.Weather.ToString()));
            EditorGUILayout.LabelField(new GUIContent("Buffer: " + w.Buffer.ToString()));
            setTo = (WeatherType)EditorGUILayout.EnumPopup(new GUIContent("Set to"), setTo);
            if (GUILayout.Button("Set Weather"))
                w.Weather = setTo;
        }
        SDictionaryGUI.ValueGUI<WeatherData> valGUI = (data) =>
        {
            return EditorGUILayout.ObjectField(data, typeof(WeatherData), false) as WeatherData;
        };
        w.weatherData.DoGUILayout(valGUI, w.weatherData.EnumAddGUI, "Weather Data", true);
        EditorUtils.SetSceneDirtyIfGUIChanged();
    }
}
