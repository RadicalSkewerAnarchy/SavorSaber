using SerializableCollections.GUIUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIBrain))]
public class AIBrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AIBrain brain = target as AIBrain;

        // show current state
        //EditorGUILayout.ObjectField("Current State", brain.CurrentState, typeof(AIState), false);

        //EditorGUILayout.PropertyField(brain.States, "States", true, GUILayout.Height(100));

        GUILayout.BeginHorizontal();
        // add transition
        if (GUILayout.Button("+"))
        {
            brain.States.Capacity++;
        }
        // remove transition
        if (GUILayout.Button("-"))
        {

        }
        GUILayout.EndHorizontal();
    }
}
