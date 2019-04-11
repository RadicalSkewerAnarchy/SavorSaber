using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IntensifyWaterfallParticle))]
public class WaterfallControl : Editor
{
    /*private void OnEnable()
    {
        intensity = serializedObject.FindProperty("intensity");
    }*/
    public override void OnInspectorGUI()
    {
        IntensifyWaterfallParticle particleInt = (IntensifyWaterfallParticle)target;        
        particleInt.intensity = EditorGUILayout.Slider("Intensity", particleInt.intensity, 0, 1f);
        particleInt.particleMinSize = EditorGUILayout.FloatField("Water Particle Minimum Size", particleInt.particleMinSize);
        particleInt.startSpeed = EditorGUILayout.FloatField("Water Particle Start Speed", particleInt.startSpeed);
        particleInt.waterColor = EditorGUILayout.ColorField("Water base color", particleInt.waterColor);
        particleInt.steamOpacity = EditorGUILayout.FloatField("Steam Opacity", particleInt.steamOpacity);
        particleInt.steamColor = EditorGUILayout.ColorField("Steam Color", particleInt.steamColor);
    }

}
