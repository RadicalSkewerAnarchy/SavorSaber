using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IngredientData))]
public class IngredientDataEditor : Editor
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
        ingredient.flavors = (RecipeData.Flavors)EditorGUILayout.EnumFlagsField(new GUIContent("Flavors", "TODO: tooltip"), ingredient.flavors);
        if (GUI.changed)
            EditorUtility.SetDirty(ingredient);
    }
}
