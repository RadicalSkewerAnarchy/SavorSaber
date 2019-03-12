using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SerializableCollections.GUIUtils;

[CustomEditor(typeof(ControlProfile))]
public class ControlProfileEditor : Editor
{
    bool showSecondaries = false;
    public override void OnInspectorGUI()
    {
        var c = target as ControlProfile;
        c.displayName = EditorGUILayout.TextField(new GUIContent("Display Name"), c.displayName);
        c.inputSource = (ControlProfile.InputSource) EditorGUILayout.EnumPopup(new GUIContent("Input Source"), c.inputSource);
        SDictionaryGUI.ValueGUI<AxisName> valGUIAxis = (a) => (AxisName)EditorGUILayout.EnumPopup(a);
        c.axes.DoGUILayout(valGUIAxis, () => c.axes.EnumAddGUIVal(), "Axis Names", true);
        SDictionaryGUI.ValueGUI<KeyCode> valGUIKeyBind = (key) => (KeyCode)EditorGUILayout.EnumPopup(key);
        c.keyBinds.DoGUILayout(valGUIKeyBind, () => c.keyBinds.EnumAddGUI(), "Bindings", true);
        showSecondaries = EditorGUILayout.ToggleLeft(new GUIContent("Show Secondaries"), showSecondaries);
        if(showSecondaries)
        {
            c.secondaryAxes.DoGUILayout(valGUIAxis, () => c.secondaryAxes.EnumAddGUIVal(), "Axis Names 2", true);
            c.secondaryKeyBinds.DoGUILayout(valGUIKeyBind, () => c.secondaryKeyBinds.EnumAddGUI(), "Bindings 2", true);
        }
        if (GUI.changed)
            EditorUtility.SetDirty(c);
    }
}
