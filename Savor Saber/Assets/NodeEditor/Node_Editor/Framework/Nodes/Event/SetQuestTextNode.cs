using UnityEngine;
using UnityEditor;
using NodeEditorFramework;

namespace Gameflow
{

    [Node(false, "Event/Set Quest Text", new System.Type[] { typeof(GameflowCanvas), typeof(DialogCanvas) })]
    public class SetQuestTextNode : BaseNodeIO
    {
        public override string Title { get { return "Set Quest Text"; } }
        public override Vector2 MinSize { get { return new Vector2(150, 50); } }
        public override bool AutoLayout { get { return true; } }

        private const string Id = "Set Quest Text";
        public override string GetID { get { return Id; } }

        public string text;

        const string tooltip_setTo = "The text to set the quest UI to";

        public override void NodeGUI()
        {
            GUILayout.Space(5);
            GUILayout.BeginVertical("box");
            GUILayout.Label(new GUIContent("Set To", tooltip_setTo), NodeEditorGUI.nodeLabelBoldCentered);
            text = GUILayout.TextField(text);
            GUILayout.EndVertical();
        }

        protected override void OnCreate()
        {
            text = string.Empty;
        }
    }
}
