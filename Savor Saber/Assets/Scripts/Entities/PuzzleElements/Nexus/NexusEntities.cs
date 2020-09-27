using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A component that should be attatched to any object that is turned on or off by a Nexus
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class NexusEntities : NexusEntity
{
    public override void Activate(bool b)
    {
        activated = b;
        foreach (Transform t in this.transform)
        {
            spriteRenderer = t.GetComponent<SpriteRenderer>();
            collider = t.GetComponent<Collider2D>();
            var looper = t.GetComponentInChildren<LoopedSpawner>();

            if (spriteRenderer != null)
                spriteRenderer.color = b ? Color.white : new Color(1, 1, 1, 0.25f);
            else Debug.Log(this.nexusID + " is trying to modify" + t.name + "'s NULL SpriteRenderer ");

            if (collider != null)
                collider.enabled = b;
            else Debug.Log(this.nexusID + " is trying to modify" + t.name + "'s NULL Collider");

            if (looper != null)
                looper.enabled = b;
        }
    }
}
