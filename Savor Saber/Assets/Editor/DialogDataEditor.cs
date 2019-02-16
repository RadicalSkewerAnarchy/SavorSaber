using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SerializableCollections.GUIUtils;

[CustomEditor(typeof(DialogData))]
public class DialogDataEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DialogData dialogData = target as DialogData;
        dialogData.displayName = EditorGUILayout.TextField(new GUIContent("Display name"), dialogData.displayName);

        SDictionaryGUI.ValueGUI<Sprite> valGUI = (spr) =>
        {
            return EditorGUILayout.ObjectField(spr, typeof(Sprite), false) as Sprite;
        };

        dialogData.portraitDictionary.DoGUILayout(valGUI, dialogData.portraitDictionary.EnumAddGUIVal, "Portraits", true);

        EditorUtils.SetSceneDirtyIfGUIChanged(target);
    }
}
