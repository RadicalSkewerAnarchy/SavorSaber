using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPlayDialogScene :EventScript
{
    public BaseDialog dialog;

    public override IEnumerator PlayEvent(GameObject player)
    {
        dialog.Activate(true);
        yield return new WaitUntil(() => dialog.dialogFinished);
    }
}
