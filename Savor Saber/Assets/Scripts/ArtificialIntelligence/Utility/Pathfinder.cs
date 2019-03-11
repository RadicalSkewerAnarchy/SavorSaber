using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    Dictionary<TileNode, float> cost = null;
    TileNode[] discovered;
    TileNode[] evaluated;

    public void Start()
    {
        cost = new Dictionary<TileNode, float>();
    }
    /// <summary>
    /// take in two tilenodes, the current and the target tile
    /// </summary>
    /// <param name="current"></param>
    /// <param name="target"></param>
    public void AStar(TileNode current, TileNode target)
    {
        




    }
}
//http://gigi.nullneuron.net/gigilabs/a-pathfinding-example-in-c/
