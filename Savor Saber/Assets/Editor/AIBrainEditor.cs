using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AIBrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AIBrain brain = (AIBrain)target;

        EditorGUILayout.ObjectField(brain.CurrentState, typeof(AIState), true);

        base.OnInspectorGUI();
    }
}
