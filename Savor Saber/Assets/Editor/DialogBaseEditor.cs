using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SerializableCollections.GUIUtils;

[CustomEditor(typeof(BaseDialog), true)]
public class DialogBaseEditor : Editor
{
    private string addStr;
    public override void OnInspectorGUI()
    {
        var dialog = target as BaseDialog;
        dialog.repeatable = EditorGUILayout.Toggle(new GUIContent("Repeatable"), dialog.repeatable);
        dialog.dialogBoxPrefab = EditorGUILayout.ObjectField(new GUIContent("Dialog Box Prefab"), dialog.dialogBoxPrefab, typeof(GameObject), false) as GameObject;
        dialog.UICanvas = EditorGUILayout.ObjectField(new GUIContent("UI Canvas"), dialog.UICanvas, typeof(Canvas), true) as Canvas;
        SDictionaryGUI.ValueGUI<GameObject> valueGUI = (actor) =>
        {
            return EditorGUILayout.ObjectField(actor, typeof(GameObject), true) as GameObject;
        };
        dialog.actors.DoGUILayout(valueGUI, () => { dialog.actors.StringAddGUID(ref addStr); }, "Actors", true);
    }
}
