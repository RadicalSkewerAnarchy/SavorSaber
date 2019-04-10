using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class SetFlagTrigger : MonoBehaviour
{
    public string flag;
    public string value;
    public UnityEvent playEvent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FlagManager.SetFlag(flag, value);
        if (playEvent != null)
            playEvent.Invoke();
    }
}
