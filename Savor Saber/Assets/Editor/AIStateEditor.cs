using SerializableCollections.GUIUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIState), true)]
public class AIStateEditor : Editor
{

    public override void OnInspectorGUI()
    {
        AIState state = (AIState)target;

        if (GUILayout.Button("Add New Transition", GUILayout.Width(400)))
        {
            if (state.Transitions == null) state.Transitions = new List<AITransition>();

            AITransition tran = new AITransition();
            state.Transitions.Add(tran);
        }
        if (GUILayout.Button("Delete Last Transition", GUILayout.Width(400)))
        {
            if (state.Transitions.Count > 0)
            {
                Destroy(state.Transitions[state.Transitions.Count - 1]);
                state.Transitions.RemoveAt(state.Transitions.Count - 1);
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (state.Transitions == null) return;

        foreach (AITransition transition in state.Transitions)
        {
            if (transition != null)
            {
                transition.NextState = (AIState) EditorGUILayout.ObjectField($"Transition To ", transition.NextState, typeof(AIState), true);

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("All Else Any ");
                transition.AllElseAny = (bool) EditorGUILayout.Toggle(transition.AllElseAny);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Priority ");
                transition.Priority = (int) EditorGUILayout.IntField(transition.Priority);
                GUILayout.EndHorizontal();

                int i = 0;
                foreach (AIDecision decide in transition.Decisions)
                {
                    if (decide != null)
                    {
                        transition.Decisions[i] = (AIDecision) EditorGUILayout.ObjectField($"  Decision > ", decide, typeof(AIDecision), true);
                    }
                    ++i;
                }


                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Add Decision");
                if (GUILayout.Button("+"))
                {
                    AIDecision dec = new AIDecision();
                    transition.Decisions.Add(dec);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Delete Decision");
                if (GUILayout.Button("-"))
                {
                    if (transition.Decisions.Count > 0) transition.Decisions.RemoveAt(transition.Decisions.Count - 1);
                }
                GUILayout.EndHorizontal();

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }
        }

    }

}
