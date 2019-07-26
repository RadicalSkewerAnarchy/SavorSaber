using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A component that should be attatched to any object that is turned on or off by a Nexus
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class NexusEntity : MonoBehaviour
{
    public string nexusID;
    [HideInInspector]
    public bool activated;

    private SpriteRenderer spriteRenderer;
    new private Collider2D collider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        string flag = FlagManager.GetFlag(nexusID);
        Activate(flag == Nexus.State.Activated.ToString());
    }

    private void Update()
    {
        if (activated)
            return;
        string flag = FlagManager.GetFlag(nexusID);
        if (flag == Nexus.State.Activated.ToString())
            Activate(true);
    }

    public void Activate(bool b)
    {
        activated = b;
        spriteRenderer.color = b ? Color.white : new Color(1,1,1,0.25f);
        if (collider != null)
            collider.enabled = b;
    }

}
