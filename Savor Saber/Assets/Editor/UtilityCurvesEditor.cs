using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UtilityCurves))]
public class UtilityCurvesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var data = target as UtilityCurves;
    }
    //private void doDictGUILayout(string title, UtilityCurves.AIStates dict, UtilityCurves data)
    //{
    //    #region Title and Controls
    //    GUILayout.BeginHorizontal();
    //    GUILayout.Label(title + ": " + dict.Count, EditorUtils.Bold);
    //    //string toAdd = EditorGUILayout.TextField()
    //    if (EditorGUILayout.DropdownButton(new GUIContent("+"), FocusType.Keyboard))
    //    {
    //        GenericMenu menu = new GenericMenu();
    //        foreach (var t in EnumUtils.GetValues<TimeOfDay>())
    //            if (!dict.ContainsKey(t))
    //                menu.AddItem(new GUIContent(t.ToString()), false, (obj) => dict.Add((TimeOfDay)obj, new TimeOfDayData()), t);
    //        menu.ShowAsContext();
    //        EditorUtility.SetDirty(data);
    //    }
    //    GUILayout.EndHorizontal();
    //    #endregion

    //    EditorGUI.indentLevel++;
    //    TimeOfDay toDelete = (TimeOfDay)(-1); // Item to delete; -1 if none chosen
    //    TimeOfDay[] keys = new TimeOfDay[dict.Count];
    //    dict.Keys.CopyTo(keys, 0);
    //    System.Array.Sort(keys);
    //    foreach (var key in keys)
    //    {
    //        GUILayout.BeginHorizontal();
    //        EditorGUILayout.LabelField(key.ToString(), EditorUtils.Bold, GUILayout.Width(100));
    //        GUILayout.Space(125);
    //        if (GUILayout.Button("-", GUILayout.Width(50)))
    //            toDelete = key;
    //        GUILayout.EndHorizontal();
    //        var item = dict[key];
    //        GUILayout.BeginVertical();
    //        //item.lightColor = EditorGUILayout.ColorField(new GUIContent("Light Color"), item.lightColor);
    //        EditorUtils.Separator();
    //        GUILayout.EndVertical();
    //    }
    //    //if (toDelete != (TimeOfDay)(-1))
    //        //dict.Remove(toDelete);
    //    EditorGUI.indentLevel--;
    //}
}
