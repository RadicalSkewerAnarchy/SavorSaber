using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class TileNode : MonoBehaviour
{
    public float x = 0, y = 0;
    public bool walkable = true;
    public List<TileNode> neighbors;
    private void Awake()
    {
        neighbors = new List<TileNode>();
    }
}
