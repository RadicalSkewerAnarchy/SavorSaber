using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace Gameflow
{
    [Node(false, "Event/Stop", new System.Type[] { typeof(GameflowCanvas), typeof(DialogCanvas) })]
    public class StopNode : BaseNodeIO 
    {
        public const string ID = "Stop Node";
        public override string GetID { get { return ID; } }

        public override string Title { get { return "Stop"; } }
        public override Vector2 MinSize { get { return new Vector2(250, 40); } }

        public float waitTime;

        public override void NodeGUI()
        {
            GUILayout.Space(5);
        }
    }
}
