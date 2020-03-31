using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(CustomList))]

public class CustomListEditor : Editor
{
    CustomList t;
    SerializedObject GetTarget;
    SerializedProperty ListOfStates;
    int ListSize;

    void OnEnable()
    {
        t = (CustomList)target;
        GetTarget = new SerializedObject(t);
        ListOfStates = GetTarget.FindProperty("MyList"); // Find the List in our script and create a refrence of it
    }

    public override void OnInspectorGUI()
    {
        //Update our list
        GetTarget.Update();

        //======================
        EditorGUILayout.Space();
        //======================


        // BUTTON TO ADD
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Add State ======================");

        if (GUILayout.Button("++", GUILayout.MaxWidth(100), GUILayout.MaxHeight(30)))
        {
            t.MyList.Add(new CustomList.MyClass());
        }
        GUILayout.EndHorizontal();


        //======================
        EditorGUILayout.Space();
        //======================

        //Display States
        for (int i = 0; i < ListOfStates.arraySize; i++)
        {
            // collect element properties
            SerializedProperty MyListRef = ListOfStates.GetArrayElementAtIndex(i);

            SerializedProperty State = MyListRef.FindPropertyRelative("AnGO");
            SerializedProperty TransitionList = MyListRef.FindPropertyRelative("AnTranArray");


            // add property fields based on that object
            EditorGUILayout.ObjectField(State);


            // Array fields with remove at index
            //======================
            EditorGUILayout.Space();
            //======================

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Transitions");
            if (GUILayout.Button("++", GUILayout.MaxWidth(75), GUILayout.MaxHeight(25)))
            {
                TransitionList.InsertArrayElementAtIndex(TransitionList.arraySize);
                TransitionList.GetArrayElementAtIndex(TransitionList.arraySize - 1).objectReferenceValue = new AITransition();
            }
            GUILayout.EndHorizontal();

            
            // within each Transition
            for (int a = 0; a < TransitionList.arraySize; a++)
            {
                SerializedObject Transition = TransitionList.GetArrayElementAtIndex(a).serializedObject;
                EditorGUILayout.ObjectField(TransitionList.GetArrayElementAtIndex(a));

                SerializedProperty DecisionList = Transition.FindProperty("Decisions");

                // adding decisions
                if (DecisionList != null)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Decisions");
                    if (GUILayout.Button("+", GUILayout.MaxWidth(25), GUILayout.MaxHeight(25)))
                    {
                        DecisionList.InsertArrayElementAtIndex(DecisionList.arraySize);
                    }
                    GUILayout.EndHorizontal();

                    // display decisions
                    for (int b = 0; b < DecisionList.arraySize; b++)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.ObjectField(DecisionList.GetArrayElementAtIndex(b));
                        if (GUILayout.Button("-", GUILayout.MaxWidth(20), GUILayout.MaxHeight(15)))
                        {
                            DecisionList.DeleteArrayElementAtIndex(b);
                        }
                        GUILayout.EndHorizontal();
                    }
                }

                // remove transition
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("remove transition >" + (a));
                if (GUILayout.Button("--", GUILayout.MaxWidth(50), GUILayout.MaxHeight(20)))
                {
                    TransitionList.DeleteArrayElementAtIndex(a);
                    TransitionList.DeleteArrayElementAtIndex(a);
                }
                GUILayout.EndHorizontal();


                //======================
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                //======================

            }

            //======================
            EditorGUILayout.Space();
            //======================

            //Remove state from the List
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Remove State >>>>>>>>>>>>>>>>>>>>>>");

            if (GUILayout.Button("--", GUILayout.MaxWidth(25), GUILayout.MaxHeight(15)))
            {
                ListOfStates.DeleteArrayElementAtIndex(i);
            }
            GUILayout.EndHorizontal();
            //======================
            EditorGUILayout.Space();
            //======================
        }

        //Apply the changes to our list
        GetTarget.ApplyModifiedProperties();
    }
}

