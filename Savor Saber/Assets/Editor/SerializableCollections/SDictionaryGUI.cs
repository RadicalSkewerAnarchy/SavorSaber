using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SerializableCollections
{
    namespace GUIUtils
    {
        public static class SDictionaryGUI
        {
            public delegate void ValueGUI<T>(T value);
            public delegate void AddGUI();

            public static void StringAddGUI<TValue>(this SDictionary<string, TValue> dict, ref string toAdd) where TValue : new()
            {
                EditorGUILayout.LabelField("New:", GUILayout.Width(45));
                toAdd = EditorGUILayout.TextField(toAdd);
                if (GUILayout.Button(new GUIContent("Add"), GUILayout.Width(45)))
                {
                    if (!string.IsNullOrWhiteSpace(toAdd) && !dict.ContainsKey(toAdd))
                    {
                        dict.Add(toAdd, new TValue());
                    }
                    toAdd = string.Empty;
                    GUIUtility.keyboardControl = 0;
                }
            }

            public static bool DoGUILayout<TKey, TValue>(this SDictionary<TKey, TValue> dict, ValueGUI<TValue> valueGUI, GenericMenu menu, string title, bool oneLine = false)
            {
                bool ret = false;
                GUILayout.BeginHorizontal();
                GUILayout.Label(title + ": " + dict.Count, EditorUtils.Bold);
                if (EditorGUILayout.DropdownButton(new GUIContent("+"), FocusType.Keyboard))
                {
                    menu.ShowAsContext();
                    ret = true;
                }
                GUILayout.EndHorizontal();
                DoGUILayout(dict, valueGUI, oneLine);
                return ret;
            }

            public static void DoGUILayout<TKey, TValue>(this SDictionary<TKey, TValue> dict, ValueGUI<TValue> valueGUI, AddGUI addGUI, string title, bool oneLine = false)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(title + ": " + dict.Count, EditorUtils.Bold, GUILayout.MaxWidth(60));
                GUILayout.FlexibleSpace();
                addGUI();
                GUILayout.EndHorizontal();
                EditorUtils.Separator();
                DoGUILayout(dict, valueGUI, oneLine);

            }

            private static void DoGUILayout<TKey, TValue>(SDictionary<TKey, TValue> dict, ValueGUI<TValue> valueGUI, bool oneLine)
            {
                EditorGUI.indentLevel++;
                TKey toDelete = default;
                bool delete = false;
                TKey[] keys = new TKey[dict.Count];
                dict.Keys.CopyTo(keys, 0);
                System.Array.Sort(keys);
                foreach (var key in keys)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(key.ToString(), GUILayout.MaxWidth(150));
                    GUILayout.Space(1);
                    if (oneLine)
                        valueGUI(dict[key]);
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("-", GUILayout.Width(45)))
                    {
                        toDelete = key;
                        delete = true;
                    }
                    GUILayout.EndHorizontal();
                    if (!oneLine)
                        valueGUI(dict[key]);
                }
                if (delete)
                    dict.Remove(toDelete);
                EditorGUI.indentLevel--;
                EditorUtils.Separator();
            }
        }
    }
}
