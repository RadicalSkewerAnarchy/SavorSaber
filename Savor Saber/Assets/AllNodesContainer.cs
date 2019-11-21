using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllNodesContainer : MonoBehaviour
{
    public void ReassessNeighbors()
    {
        TileNode node;
        foreach (Transform t in this.transform)
        {
            node = t.GetComponent<TileNode>();

            // check if nearby nodes exist
            // and are not in neighbors list
            //    add them if not in
        }
    }
}
