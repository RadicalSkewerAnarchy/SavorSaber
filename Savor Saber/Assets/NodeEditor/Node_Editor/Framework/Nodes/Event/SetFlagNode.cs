using UnityEngine;
using UnityEditor;
using NodeEditorFramework;

namespace Gameflow
{

    [Node(false, "Event/Set Flag", new System.Type[] { typeof(GameflowCanvas), typeof(DialogCanvas) })]
    public class SetFlagNode : BaseNodeIO
    {
        public override string Title { get { return "Set Flag"; } }
        public override Vector2 MinSize { get { return new Vector2(150, 50); } }
        public override bool AutoLayout { get { return true; } }

        private const string Id = "Set Variable Node";
        public override string GetID { get { return Id; } }

        public string flagName;
        public string value;

        const string tooltip_varName = "The name of the flag to set. Will not parse macros. Naming convention: PascalCase";
        const string tooltip_setTo = "The name of the variable to set. Will (NOT CURRENTLY) parse macro variables w/ {varName}";

        public override void NodeGUI()
        {
            GUILayout.Space(5);
            GUILayout.BeginVertical("box");
            GUILayout.Label(new GUIContent("Flag Name", tooltip_varName), NodeEditorGUI.nodeLabelBoldCentered);
            flagName = GUILayout.TextField(flagName);
            GUILayout.Label(new GUIContent("Set To", tooltip_setTo), NodeEditorGUI.nodeLabelBoldCentered);
            value = GUILayout.TextField(value);
            GUILayout.EndVertical();
        }

        protected override void OnCreate()
        {
            flagName = "flag";
            value = "value";
        }
    }
}
