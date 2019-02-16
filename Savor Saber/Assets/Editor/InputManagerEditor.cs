using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SerializableCollections.GUIUtils;

[CustomEditor(typeof(InputManager))]
public class InputManagerEditor : Editor
{
    string addStr;
    public override void OnInspectorGUI()
    {
        var i = target as InputManager;
        i.keyboardControls = EditorGUILayout.ObjectField(new GUIContent("Keyboard Control Profile"), i.keyboardControls, typeof(ControlProfile), false) as ControlProfile;
        i.gamepadControls = EditorGUILayout.ObjectField(new GUIContent("GamePad Control Profile"), i.gamepadControls, typeof(ControlProfile), false) as ControlProfile;
        SDictionaryGUI.ValueGUI<ControlProfile> valGUI = (c) =>
        {
            return EditorGUILayout.ObjectField(c, typeof(ControlProfile), false) as ControlProfile;
        };
        i.controlProfiles.DoGUILayout(valGUI, () => i.controlProfiles.StringAddGUI(ref addStr), "Profiles", true);
        EditorUtils.SetSceneDirtyIfGUIChanged(target);
    }
}
