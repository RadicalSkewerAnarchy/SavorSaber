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
            CheckPath();
            if (Tile != null)
            {
                MoveTo(Tile.transform, myBrain.CharacterData.Speed, 1f);
            }
            else
            {
                ChooseTarget();
            }
        }
        else
        {
            ChooseTarget();
            Tile = null;
        }
    }

    public void CheckPath()
    {
        if (myBrain.path == null)
        {
            myBrain.path = AIPathfinder.Instance.AStar(myBrain.CharacterData.gameObject, GetNearestNode(Target.transform.position));
        }
        else
        {
            if (Tile != null)
            {
                float distance = Vector2.Distance(Tile.transform.position, myBrain.CharacterData.transform.position);
                if (distance < 1)
                {
                    myBrain.path.RemoveAt(myBrain.path.Count - 1);
                }
            }

            if (myBrain.path.Count > 0)
            {
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
