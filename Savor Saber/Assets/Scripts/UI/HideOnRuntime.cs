using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For editor logic entities like triggers
/// Disables spriterenderers so that they can be seen in editor
/// but not in-game
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class HideOnRuntime : MonoBehaviour
{
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
