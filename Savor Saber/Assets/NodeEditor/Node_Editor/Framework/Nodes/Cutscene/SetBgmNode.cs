using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace Gameflow
{
    [Node(false, "Audio/SetBgm", new System.Type[] { typeof(GameflowCanvas), typeof(DialogCanvas) })]
    public class SetBgmNode : CutsceneEventNode 
    {
        public const string ID = "Set Bgm Node";
        public override string GetID { get { return ID; } }

        public override string Title { get { return "Set Bgm"; } }
        public override Vector2 MinSize { get { return new Vector2(250, 60); } }

        public float fadeTime;
        public AudioClip bgm;

        protected override void OnCreate()
        {
            waitUntilFinished = false;
            fadeTime = 3;
        }

        public override void NodeGUI()
        {
            GUILayout.Space(5);
            GUILayout.BeginVertical("Box");
            waitUntilFinished = RTEditorGUI.Toggle(waitUntilFinished, "Wait until finished");
            fadeTime = RTEditorGUI.FloatField("Fade Time", fadeTime);
            bgm = RTEditorGUI.ObjectField(bgm, false);
            GUILayout.EndVertical();

            //Don't know why this code needs to be here exactly, but it makes everything nicer? maybe add to some static stuff?
            GUILayout.BeginHorizontal();
            RTEditorGUI.labelWidth = 50;
            GUILayout.EndHorizontal();
        }
    }
}
