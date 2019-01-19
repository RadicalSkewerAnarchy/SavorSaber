using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SerializableCollections.GUIUtils;

[CustomEditor(typeof(UtilityCurves))]
public class UtilityCurvesEditor : Editor
{
    string addStr;
    string innerAddStr;
    string macroAddStr;
    string macroInnerAddStr;
    public override void OnInspectorGUI()
    {
        var data = target as UtilityCurves;
        // AI states dictionary
        SDictionaryGUI.AddGUI addGUI = () => { data.aiStates.StringAddGUI(ref addStr); };
        SDictionaryGUI.ValueGUI<UtilityCurves.CurveDict> valueGUI = (dict) =>
        {
            SDictionaryGUI.AddGUI innerAddGUI = () => { dict.StringAddGUI(ref innerAddStr); };
            SDictionaryGUI.ValueGUI <AnimationCurve> innerValueGUI= (curve) =>
            {
                curve = EditorGUILayout.CurveField(curve);
            };
            dict.DoGUILayout(innerValueGUI, innerAddGUI, "Values", true);
        };
        data.aiStates.DoGUILayout(valueGUI, addGUI, "AI States");
        // Macro values dictionary
        SDictionaryGUI.AddGUI macroAddGUI = () => { data.macroValues.StringAddGUI(ref macroAddStr); };
        SDictionaryGUI.ValueGUI<UtilityCurves.CurveDict> macroValueGUI = (dict) =>
        {
            SDictionaryGUI.AddGUI innerAddGUI = () => { dict.StringAddGUI(ref macroInnerAddStr); };
            SDictionaryGUI.ValueGUI<AnimationCurve> innerValueGUI = (curve) =>
            {
                curve = EditorGUILayout.CurveField(curve);
            };
            dict.DoGUILayout(innerValueGUI, innerAddGUI, "Sub Values", true);
        };
        data.macroValues.DoGUILayout(macroValueGUI, macroAddGUI, "Macro Values");
    }
}
