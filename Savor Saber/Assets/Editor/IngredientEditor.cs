﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IngredientData))]
public class IngredientEditor : Editor
{
    private const string displayNameTooltip = "The name of the ingredient in game";
    public override void OnInspectorGUI()
    {
        var ingredient = target as IngredientData;
        // Display data
        ingredient.displayName = EditorGUILayout.TextField(new GUIContent("Display Name", displayNameTooltip), ingredient.displayName);
        ingredient.image = EditorGUILayout.ObjectField(new GUIContent("Image", "TODO: tooltip"), ingredient.image, typeof(Texture2D), false) as Texture2D;
        EditorUtils.Separator();
        // Gameplay data
        ingredient.types = (IngredientData.Types)EditorGUILayout.EnumFlagsField(new GUIContent("Food Groups", "TODO: tooltip"), ingredient.types);
        ingredient.healValue = EditorGUILayout.IntField(new GUIContent("Heal Value", "TODO: tooltip"), ingredient.healValue);
        if (GUI.changed)
            EditorUtility.SetDirty(ingredient);
    }
}
