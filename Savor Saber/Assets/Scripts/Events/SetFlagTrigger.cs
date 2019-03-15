using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SetFlagTrigger : MonoBehaviour
{
    public string flag;
    public string value;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FlagManager.SetFlag(flag, value);
    }
}
