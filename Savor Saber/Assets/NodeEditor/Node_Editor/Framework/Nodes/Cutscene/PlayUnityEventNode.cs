using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace Gameflow
{
    [Node(false, "Event/Play Unity Event", new System.Type[] { typeof(GameflowCanvas), typeof(DialogCanvas) })]
    public class PlayUnityEventNode : CutsceneEventNode 
    {
        public const string ID = "Play Unity Event Node";
        public override string GetID { get { return ID; } }

        public override string Title { get { return "Play Unity Event"; } }
        public override Vector2 MinSize { get { return new Vector2(250, 40); } }

        public string eventName;

        protected override void OnCreate()
        {
            waitUntilFinished = true;
        }

        public override void NodeGUI()
        {
            GUILayout.Space(5);
            GUILayout.BeginVertical("Box");
            eventName = RTEditorGUI.TextField(new GUIContent("Event"), eventName);
            GUILayout.EndVertical();

            //Don't know why this code needs to be here exactly, but it makes everything nicer? maybe add to some static stuff?
            GUILayout.BeginHorizontal();
            GUILayout.EndHorizontal();
        }
    }
}
