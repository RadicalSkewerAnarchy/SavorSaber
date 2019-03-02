using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;

namespace Gameflow
{
    [Node(true, "Event/CutsceneBase", new System.Type[] { typeof(GameflowCanvas), typeof(DialogCanvas) })]
    public abstract class CutsceneEventNode : BaseNodeIO
    {
        public override bool AllowRecursion { get => true; }
        public bool waitUntilFinished;
    }
}
