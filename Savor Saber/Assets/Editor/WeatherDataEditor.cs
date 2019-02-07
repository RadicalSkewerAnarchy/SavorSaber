using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SerializableCollections.GUIUtils;

[CustomEditor(typeof(WeatherData))]
public class WeatherDataEditor : Editor
{
    private const string displayNameTooltip = "The name of the weather type in game. In-engine, use the name of this SO";
    public override void OnInspectorGUI()
    {
        var data = target as WeatherData;
        // Display data
        data.displayName = EditorGUILayout.TextField(new GUIContent("Display Name", displayNameTooltip), data.displayName);
        EditorUtils.Separator();
        data.effectCreator = EditorGUILayout.ObjectField(new GUIContent("Effect Creator"), data.effectCreator, typeof(GameObject),false) as GameObject;
        data.ambientSound = EditorGUILayout.ObjectField(new GUIContent("Ambient Sound"), data.ambientSound, typeof(AudioClip), false) as AudioClip;
        SDictionaryGUI.ValueGUI<TimeOfDayData> valGUI = (tod) =>
        {
            tod.lightColor = EditorGUILayout.ColorField(tod.lightColor);
            return tod;
        };
        data.lightingOverrides.DoGUILayout(valGUI, data.lightingOverrides.EnumAddGUI, "Lighting Overrides", true);
        if (GUI.changed)
            EditorUtility.SetDirty(data);

    }
}
