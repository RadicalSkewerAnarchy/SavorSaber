﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class TileNode : MonoBehaviour
{
    public float x = 0, y = 0;
    public bool walkable = true;
    public List<TileNode> neighbors;
    /*private void Awake()
    {
        neighbors = new List<TileNode>();
    }*/
    private void Awake()
    {
        SetWalkable(true);
    }
    public void SetWalkable(bool on)
    {
        walkable = on;
        if (!walkable)
        {
            Destroy(gameObject.GetComponent<BoxCollider2D>());
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetType() == typeof(CompositeCollider2D))
        {
            Destroy(gameObject.GetComponent<BoxCollider2D>());
        }
    }
}

