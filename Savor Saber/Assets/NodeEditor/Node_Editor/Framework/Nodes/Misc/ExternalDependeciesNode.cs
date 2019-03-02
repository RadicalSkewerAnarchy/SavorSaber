using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;


namespace Gameflow
{
    [Node(false, "Misc/External Dependencies", new System.Type[] { typeof(GameflowCanvas), typeof(DialogCanvas) })]
    public class ExternalDependeciesNode : BaseNode
    {
        public const string ID = "External Dependencies Node";
        public override string GetID { get { return ID; } }

        public override string Title { get { return "External Dependencies"; } }
        public override Vector2 MinSize { get => new Vector2(250, 100); }

        public const string extenalDependencyTooltip = "This string should by the External Dependency ID of an object in scene. If there is no external dependencies node or Dialog start node, please add one.";

        public List<string> dependencies;

        protected override void OnCreate()
        {
            dependencies = new List<string>();
        }

        public override void NodeGUI()
        {
            GUILayout.Space(5);
            GUILayout.BeginVertical("Box");           
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Dependencies"), NodeEditorGUI.nodeLabelBold);
            if (GUILayout.Button("Sort", GUILayout.Width(45)))
                dependencies.Sort();
            if (GUILayout.Button("+", GUILayout.Width(45)))
                dependencies.Add(string.Empty);
            int? toDelete = null;
            GUILayout.EndHorizontal();    
            for(int i = 0; i < dependencies.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                dependencies[i] = GUILayout.TextField(dependencies[i]);
                if (GUILayout.Button("-", GUILayout.Width(45)))
                    toDelete = i;
                GUILayout.EndHorizontal();

            }
            GUILayout.EndVertical();
            if (toDelete != null)
                dependencies.RemoveAt((int)toDelete);

            //Don't know why this code needs to be here exactly, but it makes everything nicer? maybe add to some static stuff?
            GUILayout.BeginHorizontal();
            RTEditorGUI.labelWidth = 50;
            GUILayout.EndHorizontal();
        }
    }
}
