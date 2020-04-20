using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateNavTo : AIStateMoveTo
{
    private TileNode Tile;

    public override void Perform()
    {
        if (Target != null)
        {
            Debug.Log($"Checking Path...");
            CheckPath();
            if (Tile != null)
            {
                Debug.Log($"Found Tile {Tile.name}");
                MoveTo(Tile.transform, myBrain.CharacterData.Speed, 1f);
            }
        }
        else
        {
            Debug.Log("Need a Target");
            ChooseTarget();
        }
    }

    public override void ChooseTarget()
    {
        // testing
        Target = GameObject.FindGameObjectWithTag("Player");
        if (!myBrain.IsAwareOf(Target))
        {
            Target = null;
        }
    }

    public override void OnEnter()
    {
        ChooseTarget();
    }

    public void CheckPath()
    {
        if (myBrain.path == null)
        {
            Debug.Log("...need a path");
            myBrain.path = AIPathfinder.Instance.AStar(myBrain.CharacterData.gameObject, GetNearestNode(Target.transform.position));
        }
        else
        {
            if (Tile != null)
            {
                Debug.Log("...need a tile");
                float distance = Vector2.Distance(Tile.transform.position, myBrain.CharacterData.transform.position);
                if (distance < 1)
                {
                    myBrain.path.RemoveAt(myBrain.path.Count - 1);
                }
            }

            if (myBrain.path.Count > 0)
            {
                Debug.Log("...need a whole new tile");
                int i = myBrain.path.Count - 1;
                Tile = myBrain.path[i];
            }
            else
            {
                Tile = null;
                myBrain.path = null;
            }
        }
    }

    public TileNode GetNearestNode(Vector2 pos)
    {
        int maxTries = 3;
        float sizeCheck = .5f;
        TileNode targetTile = null;
        for (var i = 0; i < maxTries; i++)
        {
            var availableNodes = Physics2D.OverlapCircleAll(pos, sizeCheck);
            var validNodes = new List<Collider2D>();
            foreach (var node in availableNodes)
            {
                if (node.GetComponent<TileNode>() != null)
                {
                    validNodes.Add(node);
                }
            }
            if (validNodes.Count > 0)
            {
                // RETURN VALID TILE
                targetTile = validNodes[(int)Random.Range(0, validNodes.Count)].GetComponent<TileNode>();
                return targetTile;
            }
            else
            {
                sizeCheck *= 2;
            }
        }
        return targetTile;
    }
}
