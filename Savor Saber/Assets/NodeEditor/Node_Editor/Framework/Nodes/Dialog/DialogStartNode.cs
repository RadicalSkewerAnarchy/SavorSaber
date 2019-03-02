using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace Gameflow
{
    [Node(false, "Dialog/Start", new System.Type[] { typeof(DialogCanvas) })]
    public class DialogStartNode : GameflowStartNode
    {
        new public const string ID = "Dialog Start Node";
        public override string GetID { get { return ID; } }

        public override string Title { get { return "Dialog Start"; } }
        public override Vector2 MinSize { get => new Vector2(250, 100); }

        public const string extenalDependencyTooltip = "This string should be an Actor or external dependency ID.";

        public List<string> dependencies;
        public List<string> actors;
        public List<string> unityEvents;

        protected override void OnCreate()
        {
            dependencies = new List<string>();
            actors = new List<string>();
            unityEvents = new List<string>();
        }

        public override void NodeGUI()
        {
            GUILayout.Space(5);
            GUILayout.BeginVertical("Box");

            #region Actors
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Actors"), NodeEditorGUI.nodeLabelBold);
            if (GUILayout.Button("Sort", GUILayout.Width(45)))
                actors.Sort();
            if (GUILayout.Button("+", GUILayout.Width(45)))
                actors.Add(string.Empty);
            int? toDeleteActor = null;
            GUILayout.EndHorizontal();
            GUILayout.Space(2);
            for (int i = 0; i < actors.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                actors[i] = GUILayout.TextField(actors[i]);
                if (GUILayout.Button("-", GUILayout.Width(45)))
                    toDeleteActor = i;
                GUILayout.EndHorizontal();
            }
            if (toDeleteActor != null)
                actors.RemoveAt((int)toDeleteActor);
            #endregion

            RTEditorGUI.Seperator();

            #region Dependencies
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Dependencies"), NodeEditorGUI.nodeLabelBold);
            if (GUILayout.Button("Sort", GUILayout.Width(45)))
                dependencies.Sort();
            if (GUILayout.Button("+", GUILayout.Width(45)))
                dependencies.Add(string.Empty);
            int? toDeleteDependency = null;
            GUILayout.EndHorizontal();
            GUILayout.Space(2);
            for (int i = 0; i < dependencies.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                dependencies[i] = GUILayout.TextField(dependencies[i]);
                if (GUILayout.Button("-", GUILayout.Width(45)))
                    toDeleteDependency = i;
                GUILayout.EndHorizontal();
            }
            if (toDeleteDependency != null)
                dependencies.RemoveAt((int)toDeleteDependency);
            #endregion

            RTEditorGUI.Seperator();

            #region Unity Events
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Unity Events"), NodeEditorGUI.nodeLabelBold);
            if (GUILayout.Button("Sort", GUILayout.Width(45)))
                unityEvents.Sort();
            if (GUILayout.Button("+", GUILayout.Width(45)))
                unityEvents.Add(string.Empty);
            int? toDeleteEvent = null;
            GUILayout.EndHorizontal();
            GUILayout.Space(2);
            for (int i = 0; i < unityEvents.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                unityEvents[i] = GUILayout.TextField(unityEvents[i]);
                if (GUILayout.Button("-", GUILayout.Width(45)))
                    toDeleteEvent = i;
                GUILayout.EndHorizontal();
            }
            if (toDeleteEvent != null)
                unityEvents.RemoveAt((int)toDeleteEvent);
            #endregion

            GUILayout.EndVertical();

            //Don't know why this code needs to be here exactly, but it makes everything nicer? maybe add to some static stuff?
            GUILayout.BeginHorizontal();
            GUILayout.EndHorizontal();
        }
    }
}
