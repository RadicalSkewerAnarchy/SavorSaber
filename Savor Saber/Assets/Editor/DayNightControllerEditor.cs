using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DayNightController))]
public class DayNightControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var targ = target as DayNightController;
        targ.Paused = EditorGUILayout.Toggle(new GUIContent("Paused"), targ.Paused);
        targ.LengthOfDay = EditorGUILayout.FloatField(new GUIContent("Game day (Seconds)"), targ.LengthOfDay);
        targ.GameHour = EditorGUILayout.FloatField(new GUIContent("Game hour (Seconds)"), targ.GameHour);
        targ.transitionTime = EditorGUILayout.FloatField(new GUIContent("Transition Hours"), targ.transitionTime);
        targ.clearWeather = EditorGUILayout.ObjectField(new GUIContent("Clear Weather"), targ.clearWeather, typeof(WeatherData), false) as WeatherData;
        EditorUtils.Separator();
        var setTimeOfDay = (TimeOfDay)EditorGUILayout.EnumPopup(new GUIContent("Time of Day"), targ.CurrTimeOfDay);
        if (setTimeOfDay != targ.CurrTimeOfDay)
            targ.SetTimeOfDay(setTimeOfDay);
        targ.currWeather = EditorGUILayout.ObjectField(new GUIContent("Current Weather"), targ.currWeather, typeof(WeatherData), false) as WeatherData;
        EditorUtils.Separator();
        foreach(var tod in EnumUtils.GetValues<TimeOfDay>())
            targ.timeTable[tod] = EditorGUILayout.FloatField(new GUIContent("Hours in " + tod.ToString()), targ.timeTable[tod]);
        if (!targ.timeTable.CheckSum())
            EditorGUILayout.HelpBox("Hours in day does not add up to 24", MessageType.Warning);
    }
}
