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
            public delegate T ValueGUI<T>(T value);
            public delegate void AddGUI();
            public delegate T GetNew<T>();

            public static void StringAddGUI<TValue>(this SDictionary<string, TValue> dict, ref string toAdd) where TValue : new()
            {
                StringAddGUI(dict, ref toAdd, () => new TValue());
            }

            public static void StringAddGUID<TValue>(this SDictionary<string, TValue> dict, ref string toAdd)
            {
                StringAddGUI(dict, ref toAdd, () => default);
            }

            private static void StringAddGUI<TValue>(SDictionary<string, TValue> dict, ref string toAdd, GetNew<TValue> getNew)
            {
                EditorGUILayout.LabelField("New:", GUILayout.Width(45));
                toAdd = EditorGUILayout.TextField(toAdd);
                if (GUILayout.Button(new GUIContent("Add"), GUILayout.Width(45)))
                {
                    if (!string.IsNullOrWhiteSpace(toAdd) && !dict.ContainsKey(toAdd))
                    {
                        dict.Add(toAdd, getNew());
                    }
                    toAdd = string.Empty;
                    GUIUtility.keyboardControl = 0;
                }
            }

            public static void EnumAddGUI<TKey, TValue>(this SDictionary<TKey, TValue> dict) where TKey: System.Enum where TValue: new()
            {
                EnumAddGUI(dict, () => new TValue());
            }

            public static void EnumAddGUIVal<TKey, TValue>(this SDictionary<TKey, TValue> dict) where TKey : System.Enum
            {
                EnumAddGUI(dict, () => default);
            }
           
            private static void EnumAddGUI<TKey, TValue>(this SDictionary<TKey, TValue> dict, GetNew<TValue> getNew) where TKey : System.Enum
            {
                if (EditorGUILayout.DropdownButton(new GUIContent("+"), FocusType.Keyboard))
                {
                    GenericMenu menu = new GenericMenu();
                    foreach (var t in EnumUtils.GetValues<TKey>())
                        if (!dict.ContainsKey(t))
                            menu.AddItem(new GUIContent(t.ToString()), false, (obj) => dict.Add((TKey)obj, getNew()), t);
                    menu.ShowAsContext();
                }
            }

            private static void GenericMenuAddGUI<TKey, TValue>(this SDictionary<TKey, TValue> dict, GenericMenu menu)
            {
                if (EditorGUILayout.DropdownButton(new GUIContent("+"), FocusType.Keyboard))
                {
                    menu.ShowAsContext();
                }
            }

            public static void DoGUILayout<TKey, TValue>(this SDictionary<TKey, TValue> dict, ValueGUI<TValue> valueGUI, AddGUI addGUI, string title, bool oneLine = false, bool showRemove = true)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(title + ": " + dict.Count, EditorUtils.Bold, GUILayout.MaxWidth(120));
                GUILayout.Space(-20);
                //GUILayout.FlexibleSpace();
                addGUI();
                GUILayout.EndHorizontal();
                EditorUtils.Separator();
                if(dict.Count > 0)
                    DoGUILayout(dict, valueGUI, oneLine, showRemove);

            }

            private static void DoGUILayout<TKey, TValue>(SDictionary<TKey, TValue> dict, ValueGUI<TValue> valueGUI, bool oneLine, bool showRemove)
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
                    EditorGUILayout.PrefixLabel(key.ToString());
                    GUILayout.Space(1);
                    if (oneLine)
                        dict[key] = valueGUI(dict[key]);
                    if(showRemove)
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("-", GUILayout.Width(45)))
                        {
                            toDelete = key;
                            delete = true;
                        }
                    }
                    GUILayout.EndHorizontal();
                    if (!oneLine)
                        dict[key] = valueGUI(dict[key]);
                }
                if (delete)
                    dict.Remove(toDelete);
                EditorGUI.indentLevel--;
                EditorUtils.Separator();
            }
        }
    }
}
