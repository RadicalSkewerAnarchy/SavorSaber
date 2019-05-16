using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    Dictionary<TileNode, float> gScore;
    Dictionary<TileNode, float> fScore;
    public GameObject allNodes; 

    public void Start()
    {
        InitGraph();
        //allNodes = GameObject.Find("Grid 16px");
        //allNodes = allNodes.transform.Find("Collision").Find("Walkable").gameObject;
    }
    private void InitGraph()
    {
        gScore = new Dictionary<TileNode, float>();
        fScore = new Dictionary<TileNode, float>();
        //var nodeList = GameObject.Find("Walkable");
        if(allNodes != null)
        {
            for (int i = 0; i < allNodes.transform.childCount; i++)
            {
                gScore.Add(allNodes.transform.GetChild(i).GetComponent<TileNode>(), Mathf.Infinity);
                fScore.Add(allNodes.transform.GetChild(i).GetComponent<TileNode>(), Mathf.Infinity);
            }
        }
        //Debug.Log("Graph initialized");
    }

    public List<TileNode> GetShortestPath(Dictionary<TileNode, TileNode> cameFrom, TileNode current)
    {
        List<TileNode> path = new List<TileNode>();
        path.Add(current);
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
            //Debug.Log("Item in path: " + current.gameObject.GetInstanceID());
        }
        return path;
    }

    /// <summary>
    /// take in two tilenodes, the start and the target tile
    /// GSCORE and FSCORE should contain graphs initialized based on populated grid
    /// </summary>
    /// <param name="start"></param>
    /// <param name="target"></param>
    public List<TileNode> AStar(TileNode target)
    {
        TileNode start = null;
        float minDist = Mathf.Infinity;
        for (int i = 0; i < allNodes.transform.childCount; i++)
        {
            float nodeDist = Vector3.Distance(transform.position, allNodes.transform.GetChild(i).position);
            if (nodeDist < minDist)
            {
                minDist = nodeDist;
                start = allNodes.transform.GetChild(i).GetComponent<TileNode>();
            }
        }
        if (start == null || target == null)
        {
            Debug.Log("Current agent or target is not on tilemap");
            return null;
        }
        // set of evaluated nodes
        List<TileNode> closed = new List<TileNode>();
        // set of discovered but unevaluated nodes
        List<TileNode> open = new List<TileNode>();
        open.Add(start);
        // the most efficient previous node mapped
        Dictionary<TileNode, TileNode> cameFrom = new Dictionary<TileNode, TileNode>();
        // the tilenode with its cost from the start node
        gScore[start] = 0;
        // total cost of getting from start to goal through this node, partly known, partly heuristic
        fScore[start] = Vector3.Distance(start.transform.position, target.transform.position);
        while (open.Count > 0)
        {
            TileNode current = null;
            float min = Mathf.Infinity;
            foreach(var node in open)
            {
                if(fScore[node] < min)
                {
                    min = fScore[node];
                    current = node;
                }
            }
            if(current.gameObject.GetInstanceID() == target.gameObject.GetInstanceID())
            {
                //GOAL IS REACHED, RETURN PATH
                return GetShortestPath(cameFrom, current);
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
                if (cameFrom.ContainsKey(neighbor))
                {
                    cameFrom[neighbor] = current;
                } else {
                    cameFrom.Add(neighbor, current);
                }
                gScore[neighbor] = tempScore;
                fScore[neighbor] = gScore[neighbor]; //+ Vector3.Distance(neighbor.transform.position, target.transform.position); // + heuristic cost estimate

            }
        }

        return null;
    }
}
//https://en.wikipedia.org/wiki/A*_search_algorithm
