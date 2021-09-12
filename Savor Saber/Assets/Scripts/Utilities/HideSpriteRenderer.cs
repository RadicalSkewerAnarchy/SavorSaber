using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HideSpriteRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<TilemapRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
