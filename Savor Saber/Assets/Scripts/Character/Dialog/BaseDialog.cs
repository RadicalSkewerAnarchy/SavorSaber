using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Exists so that dialog triggers can activate different styles of dialog
/// </summary>
public class BaseDialog : MonoBehaviour
{
    [System.NonSerialized]
    public bool active = false;

    /// <summary>
    /// Signals dialog triggers that this dialog has finished, in case something
    /// needs to happen after it plays.
    /// </summary>
    [System.NonSerialized]
    public bool dialogFinished = false;

    public virtual void Activate()
    {

    }

    public virtual void NextDialog()
    {

    }
}
