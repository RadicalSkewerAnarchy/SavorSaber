﻿using UnityEngine;
using NodeEditorFramework;

namespace Gameflow
{
    [Node(false, "Gameflow/End", new System.Type[] { typeof(GameflowCanvas), typeof(DialogCanvas) })]
    public class GameflowEndNode : BaseNode
    {
        public const string ID = "Gameflow End Node";
        public override string GetID { get { return ID; } }

        public override string Title { get { return "Gameflow End"; } }
        public override Vector2 MinSize { get { return new Vector2(150, 60); } }
        public override bool AutoLayout { get { return true; } }

        //Connection from previous node (INPUT)
        [ConnectionKnob("From Previous", NodeEditorFramework.Direction.In, "Gameflow", ConnectionCount.Multi, NodeSide.Left, 30)]
        public ConnectionKnob fromPreviousIN;

        public override void NodeGUI()
        {
            GUILayout.Space(3);
        }
    }
}
