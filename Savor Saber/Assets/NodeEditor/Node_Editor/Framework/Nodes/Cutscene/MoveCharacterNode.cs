using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace Gameflow
{
    [Node(false, "Event/Move Character", new System.Type[] { typeof(GameflowCanvas), typeof(DialogCanvas) })]
    public class MoveCharacterNode : CutsceneEventNode 
    {
        public const string ID = "Move Character Node";
        public override string GetID { get { return ID; } }

        public override string Title { get { return "Move Character"; } }
        public override Vector2 MinSize { get { return new Vector2(250, 60); } }

        public string character;
        public string destination;
        public MathUtils.FloatRange xLeniency;
        public MathUtils.FloatRange yLeniency;
        public float speed;
        public Direction endFacing;

        protected override void OnCreate()
        {
            waitUntilFinished = true;
            xLeniency = new MathUtils.FloatRange(-0.1f, 0.1f);
            yLeniency = new MathUtils.FloatRange(-0.1f, 0.1f);
            speed = 2;
        }

        public override void NodeGUI()
        {
            GUILayout.Space(5);
            GUILayout.BeginVertical("Box");
            waitUntilFinished = RTEditorGUI.Toggle(waitUntilFinished, new GUIContent("Wait While Playing"));
            character = RTEditorGUI.TextField(new GUIContent("Character", "The character to be moved. Should have an EntityController in-scene. " + DialogStartNode.extenalDependencyTooltip), character);
            destination = RTEditorGUI.TextField(new GUIContent("Destination Object", "The object to move to." + DialogStartNode.extenalDependencyTooltip), destination);
            speed = RTEditorGUI.FloatField("Speed", speed);           
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("X Leniency"));
            xLeniency.min = RTEditorGUI.FloatField(xLeniency.min, GUILayout.Width(58));
            GUILayout.Space(2);
            xLeniency.max = RTEditorGUI.FloatField(xLeniency.max, GUILayout.Width(58));
            GUILayout.EndHorizontal();          
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Y Leniency"));
            yLeniency.min = RTEditorGUI.FloatField(yLeniency.min, GUILayout.Width(58));
            GUILayout.Space(2);
            yLeniency.max = RTEditorGUI.FloatField(yLeniency.max, GUILayout.Width(58));
            GUILayout.EndHorizontal();
            endFacing = (Direction)RTEditorGUI.EnumPopup("End Facing", endFacing);
            GUILayout.EndVertical();

            //Don't know why this code needs to be here exactly, but it makes everything nicer? maybe add to some static stuff?
            GUILayout.BeginHorizontal();
            RTEditorGUI.labelWidth = 50;
            GUILayout.EndHorizontal();
        }
    }
}
