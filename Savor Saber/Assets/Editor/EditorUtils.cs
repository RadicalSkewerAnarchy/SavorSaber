using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A static class containingsome editor methods for easy GUI utility
/// </summary>
public static class EditorUtils
{
    /// <summary>Creates a horizontal bar separator </summary>
    public static void Separator()
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }
    /// <summary>Easy shortcut to the bold label text style</summary>
    public static GUIStyle Bold
    {
        get
        {
            return new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
        }
    }
}
