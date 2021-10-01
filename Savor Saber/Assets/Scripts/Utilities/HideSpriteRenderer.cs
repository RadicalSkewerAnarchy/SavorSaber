using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HideSpriteRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var sr = GetComponent<SpriteRenderer>();
        var tr = GetComponent<TilemapRenderer>();
        if(sr != null)
            sr.enabled = false;
        if(tr != null)
            tr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
