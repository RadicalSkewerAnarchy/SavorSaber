using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    Dictionary<TileNode, float> gScore;
    Dictionary<TileNode, float> fScore;
    public GameObject[] allNodes; 

    public void Start()
    {
        InitGraph();
    }
    private void InitGraph()
    {
        gScore = new Dictionary<TileNode, float>();
        fScore = new Dictionary<TileNode, float>();
        var nodeList = GameObject.Find("Walkable");
        for(int i = 0; i < nodeList.transform.childCount; i++)
        {
            gScore.Add(nodeList.transform.GetChild(i).GetComponent<TileNode>(), Mathf.Infinity);
            fScore.Add(nodeList.transform.GetChild(i).GetComponent<TileNode>(), Mathf.Infinity);
        }
    }

    /// <summary>
    /// take in two tilenodes, the start and the target tile
    /// GSCORE and FSCORE should contain graphs initialized based on populated grid
    /// </summary>
    /// <param name="start"></param>
    /// <param name="target"></param>
    public TileNode[] AStar(TileNode start, TileNode target)
    {
        // set of evaluated nodes
        List<TileNode> closed = new List<TileNode>();
        // set of discovered but unevaluated nodes
        List<TileNode> open = new List<TileNode>();
        // the most efficient previous node mapped
        Dictionary<TileNode, TileNode> cameFrom = new Dictionary<TileNode, TileNode>();
        // the tilenode with its cost from the start node
        gScore[start] = 0;
        // total cost of getting from start to goal through this node, partly known, partly heuristic
        fScore[start] = Vector3.Distance(start.transform.position, target.transform.position);

        while (open.Count > 0)
        {
            TileNode current = null;
            float min = 0;
            foreach(var node in open)
            {
                if(fScore[node] < min)
                {
                    min = fScore[node];
                    current = node;
                }
            }
            if(current.GetInstanceID() == target.GetInstanceID())
            {
                //GOAL IS REACHED, RETURN PATH
                break;
            }
            open.Remove(current);
            closed.Add(current);

            foreach(var neighbor in current.neighbors)
            {
                if (closed.Contains(neighbor))
                {
                    continue;
                }
                float tempScore = gScore[current] + Vector3.Distance(current.transform.position, neighbor.transform.position);
                if (!open.Contains(neighbor))
                {
                    open.Add(neighbor);
                }
                else if(tempScore >= gScore[neighbor])
                {
                    continue;
                }
                cameFrom.Add(neighbor, current);
                gScore[neighbor] = tempScore;
                fScore[neighbor] = gScore[neighbor]; // + heuristic cost estimate

            }
        }

        return null;
    }
}
//http://gigi.nullneuron.net/gigilabs/a-pathfinding-example-in-c/
