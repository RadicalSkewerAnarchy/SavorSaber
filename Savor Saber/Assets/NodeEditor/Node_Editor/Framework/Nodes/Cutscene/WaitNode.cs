using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace Gameflow
{
    [Node(false, "Event/Wait", new System.Type[] { typeof(GameflowCanvas), typeof(DialogCanvas) })]
    public class WaitNode : CutsceneEventNode 
    {
        public const string ID = "Wait Node";
        public override string GetID { get { return ID; } }

        public override string Title { get { return "Wait"; } }
        public override Vector2 MinSize { get { return new Vector2(250, 40); } }

        public float waitTime;

        protected override void OnCreate()
        {
            waitUntilFinished = true;
        }

        public override void NodeGUI()
        {
            GUILayout.Space(5);
            GUILayout.BeginVertical("Box");
            waitTime = RTEditorGUI.FloatField("Seconds", waitTime);
            GUILayout.EndVertical();

            //Don't know why this code needs to be here exactly, but it makes everything nicer? maybe add to some static stuff?
            GUILayout.BeginHorizontal();
            RTEditorGUI.labelWidth = 50;
            GUILayout.EndHorizontal();
        }
    }
}
