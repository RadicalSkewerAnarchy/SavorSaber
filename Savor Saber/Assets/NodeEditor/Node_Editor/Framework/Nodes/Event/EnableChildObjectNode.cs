using UnityEngine;
using UnityEditor;
using NodeEditorFramework;

namespace Gameflow
{

    [Node(false, "Event/Enable Child Object", new System.Type[] { typeof(GameflowCanvas), typeof(DialogCanvas) })]
    public class EnableChildObjectNode : BaseNodeIO
    {
        public override string Title { get { return "Enable Child Object"; } }
        public override Vector2 MinSize { get { return new Vector2(150, 50); } }
        public override bool AutoLayout { get { return true; } }

        private const string Id = "Enable Child Object Node";
        public override string GetID { get { return Id; } }

        public bool disable = false;
        public string parent;
        public string childName;

        const string tooltip_objName = "TODO: tooltip";
        const string tooltip_childName = "TODO: tooltip";

        public override void NodeGUI()
        {
            GUILayout.Space(5);
            GUILayout.BeginVertical("box");
            GUILayout.Label(new GUIContent("Parent Name", tooltip_objName), NodeEditorGUI.nodeLabelBoldCentered);
            parent = GUILayout.TextField(parent);
            GUILayout.Label(new GUIContent("Child Name", tooltip_childName), NodeEditorGUI.nodeLabelBoldCentered);
            childName = GUILayout.TextField(childName);
            disable = GUILayout.Toggle(disable, new GUIContent("Disable?"));
            GUILayout.EndVertical();
        }

        protected override void OnCreate()
        {
            parent = "";
            childName = "";
        }
    }
}
