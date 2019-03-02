using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace Gameflow
{
    [Node(false, "Event/Pan Camera", new System.Type[] { typeof(GameflowCanvas), typeof(DialogCanvas) })]
    public class PanCameraNode : CutsceneEventNode 
    {
        public enum MoveType
        {
            Smoothed,
            Linear,
        }

        public const string ID = "Pan Camera Node";
        public override string GetID { get { return ID; } }

        public override string Title { get { return "Pan Camera"; } }
        public override Vector2 MinSize { get { return new Vector2(250, 60); } }

        public MoveType movementType;
        public string target;
        public float maxPullSpeed;
        public float snapTime;

        protected override void OnCreate()
        {
            waitUntilFinished = true;
            maxPullSpeed = 100;
            snapTime = 0.75f;
        }

        public override void NodeGUI()
        {
            RTEditorGUI.labelWidth = 50;
            GUILayout.Space(5);
            GUILayout.BeginVertical("Box");
            waitUntilFinished = RTEditorGUI.Toggle(waitUntilFinished, new GUIContent("Wait While Playing"));
            target = RTEditorGUI.TextField(new GUIContent("Target object", "The GameObject to move to. " + DialogStartNode.extenalDependencyTooltip), target);
            movementType = (MoveType)RTEditorGUI.EnumPopup(new GUIContent("Movement Type"), movementType);
            if(movementType == MoveType.Linear)
                maxPullSpeed = RTEditorGUI.FloatField("Speed", maxPullSpeed);
            else
            {
                maxPullSpeed = RTEditorGUI.FloatField("Max Pull Speed", maxPullSpeed);
                snapTime = RTEditorGUI.FloatField("Snap Time", snapTime);
            }
            GUILayout.EndVertical();

            //Don't know why this code needs to be here exactly, but it makes everything nicer? maybe add to some static stuff?
            GUILayout.BeginHorizontal();
            RTEditorGUI.labelWidth = 50;
            GUILayout.EndHorizontal();
        }
    }
}
