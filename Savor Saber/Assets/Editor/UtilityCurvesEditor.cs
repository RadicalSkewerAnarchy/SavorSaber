using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SerializableCollections.GUIUtils;

[CustomEditor(typeof(UtilityCurves))]
public class UtilityCurvesEditor : Editor
{
    string addStr;
    string addStr2;
    public override void OnInspectorGUI()
    {
        var data = target as UtilityCurves;
        SDictionaryGUI.AddGUI addGUI = () => { data.aiStates.StringAddGUI(ref addStr); };
        SDictionaryGUI.ValueGUI<UtilityCurves.CurveDict> valueGUI = (dict) =>
        {
            SDictionaryGUI.AddGUI innerAddGUI = () => { dict.StringAddGUI(ref addStr2); };
            SDictionaryGUI.ValueGUI <AnimationCurve> innerValueGUI= (curve) =>
            {
                curve = EditorGUILayout.CurveField(curve);
            };
            dict.DoGUILayout(innerValueGUI, innerAddGUI, "Values", true);
        };
        data.aiStates.DoGUILayout(valueGUI, addGUI, "AI States");
    }
}
