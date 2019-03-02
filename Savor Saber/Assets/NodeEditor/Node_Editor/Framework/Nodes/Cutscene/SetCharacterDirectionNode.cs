using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace Gameflow
{
    [Node(false, "Event/Set Character Direction", new System.Type[] { typeof(GameflowCanvas), typeof(DialogCanvas) })]
    public class SetCharacterDirectionNode : CutsceneEventNode 
    {
        public const string ID = "Set Character Direction Node";
        public override string GetID { get { return ID; } }

        public override string Title { get { return "Set Character Direction"; } }
        public override Vector2 MinSize { get { return new Vector2(250, 60); } }

        public string character;
        public Direction facing;

        protected override void OnCreate()
        {
            waitUntilFinished = true;
        }

        public override void NodeGUI()
        {
            RTEditorGUI.labelWidth = 50;
            GUILayout.Space(5);
            GUILayout.BeginVertical("Box");
            waitUntilFinished = RTEditorGUI.Toggle(waitUntilFinished, new GUIContent("Wait While Playing"));
            character = RTEditorGUI.TextField(new GUIContent("Character", "The character to be moved. Should have an EntityController in-scene. " + DialogStartNode.extenalDependencyTooltip), character);
            facing = (Direction)RTEditorGUI.EnumPopup(new GUIContent("Direction"), facing);
            GUILayout.EndVertical();

            //Don't know why this code needs to be here exactly, but it makes everything nicer? maybe add to some static stuff?
            GUILayout.BeginHorizontal();
            RTEditorGUI.labelWidth = 50;
            GUILayout.EndHorizontal();
        }
    }
}
