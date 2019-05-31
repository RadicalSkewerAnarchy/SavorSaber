using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using SerializableCollections;
using SerializableCollections.GUIUtils;

[CustomEditor(typeof(EventGraph))]
public class EventGraphEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        var scene = target as EventGraph;
        //scene. = EditorGUILayout.ObjectField(new GUIContent("Dialog Prefab"), scene.Graph, typeof(GameObject), false) as GameObject;
        scene.Graph = EditorGUILayout.ObjectField(new GUIContent("Graph"), scene.Graph, typeof(Gameflow.DialogCanvas), false) as Gameflow.DialogCanvas;
        if(scene.Graph != null)
        {
            var startNode = scene.Graph.getStartNode() as Gameflow.DialogStartNode;
            if (startNode.actors.Count > 0)
            {
                foreach (string ID in startNode.actors)
                    if (!scene.Actors.ContainsKey(ID))
                        scene.Actors.Add(ID, null);
                if (scene.Actors.Count > startNode.actors.Count)
                {
                    var toDelete = new List<string>();
                    foreach (string ID in scene.Actors.Keys)
                        if (!startNode.actors.Contains(ID))
                            toDelete.Add(ID);
                    toDelete.ForEach((ID) => scene.Actors.Remove(ID));
                }
                scene.Actors.DoGUILayout((obj) => EditorGUILayout.ObjectField(obj, typeof(DialogData), true) as DialogData, () => { }, "Actors", true, false);
            }
            else
                EditorGUILayout.LabelField("No Actors", EditorUtils.Bold);
            if(startNode.dependencies.Count > 0)
            {
                foreach (string ID in startNode.dependencies)
                    if (!scene.Dependencies.ContainsKey(ID))
                        scene.Dependencies.Add(ID, null);
                if (scene.Dependencies.Count > startNode.dependencies.Count)
                {
                    var toDelete = new List<string>();
                    foreach (string ID in scene.Dependencies.Keys)
                        if (!startNode.dependencies.Contains(ID))
                            toDelete.Add(ID);
                    toDelete.ForEach((ID) => scene.Dependencies.Remove(ID));
                }
                scene.Dependencies.DoGUILayout((obj) => EditorGUILayout.ObjectField(obj, typeof(GameObject), true) as GameObject, () => { }, "Dependencies", true, false);
            }
            else
                EditorGUILayout.LabelField("No Dependencies", EditorUtils.Bold);
            if (startNode.unityEvents.Count > 0)
            {
                foreach (string ID in startNode.unityEvents)
                    if (!scene.Events.ContainsKey(ID))
                        scene.Events.Add(ID, new UnityEvent());
                if(scene.Events.Count > startNode.unityEvents.Count)
                {
                    var toDelete = new List<string>();
                    foreach (string ID in scene.Events.Keys)
                        if (!startNode.unityEvents.Contains(ID))
                            toDelete.Add(ID);
                    toDelete.ForEach((ID) => scene.Events.Remove(ID));
                }
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_events"),true);
                //scene.Events.DoGUILayout((obj) => EditorGUILayout.PropertyField(serializedObject.FindProperty("_events")), () => { }, "Dependencies", true, false);
            }
            else
                EditorGUILayout.LabelField("No Unity Events", EditorUtils.Bold);
        }
        else
        {
            EditorGUILayout.HelpBox("Set the dialog graph to set actors and other dependecies", MessageType.Warning);
        }
        serializedObject.ApplyModifiedProperties();
        EditorUtils.SetSceneDirtyIfGUIChanged(target);
    }
}
