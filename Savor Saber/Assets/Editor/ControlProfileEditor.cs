using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SerializableCollections.GUIUtils;

[CustomEditor(typeof(ControlProfile))]
public class ControlProfileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var c = target as ControlProfile;
        c.displayName = EditorGUILayout.TextField(new GUIContent("Display Name"), c.displayName);
        c.inputSource = (ControlProfile.InputSource) EditorGUILayout.EnumPopup(new GUIContent("Input Source"), c.inputSource);
        SDictionaryGUI.ValueGUI<KeyCode> valGUI = (key) => (KeyCode)EditorGUILayout.EnumPopup(key);
        c.keyBinds.DoGUILayout(valGUI, () => c.keyBinds.EnumAddGUI(), "Bindings", true);
        if (GUI.changed)
            EditorUtility.SetDirty(c);
    }
}
