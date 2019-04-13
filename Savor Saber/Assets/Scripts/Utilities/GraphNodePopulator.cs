﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//[ExecuteInEditMode]
public class GraphNodePopulator : MonoBehaviour
{
    public GameObject nodePrefab;
    public Tile[] discoveredTiles;
    public Tilemap navMeshData;
    public List<List<TileNode>> tiles;
    Tilemap[] activeTileMaps;
    BoundsInt bounds;
    bool walkable;
    private void Update()
    {
        tiles = new List<List<TileNode>>();
        tiles.Add(new List<TileNode>());
        Populate();
    }
    private void Populate()
    {
        /// List of all tilemaps
        activeTileMaps = GetComponentsInChildren<Tilemap>();
        bool first = true;
        List<Tilemap> inactiveTileMaps = new List<Tilemap>();
        foreach(var activeTiles in activeTileMaps)
        {
            if(activeTiles.GetComponent<TilemapCollider2D>() != null)
            {
                inactiveTileMaps.Add(activeTiles);
            }
        }
        foreach (var activeTiles in inactiveTileMaps)
        {
            Debug.Log("COLLISION TILEMAPS");
            /// iterates through tilemap based on bounds
            int i = 0, j = 0;
            bounds = activeTiles.cellBounds;
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                Debug.Log("Xmin " + bounds.xMin);
                j = 0;
                /// list of lists, this increments the X counter
                tiles.Add(new List<TileNode>());
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Debug.Log("Ymin " + bounds.yMin);

                    walkable = true;
                    /// sets local position hard casted as int based on current x,y iteration
                    Vector3Int local = new Vector3Int(x, y, (int)activeTiles.transform.position.z);
                    /// takes real world x,y position as an int and gets specific tile from tilemap at that location
                    Vector3 current = activeTiles.CellToWorld(local);
                    var parent = activeTiles.transform.GetChild(0);
                    foreach(var inactive in inactiveTileMaps)
                    {
                        if(activeTiles.HasTile(local) || inactive.HasTile(local))
                        {
                            Debug.Log("Unwalkable");
                            parent = navMeshData.transform.GetChild(1);
                            walkable = false;
                        }
                    }
                    GameObject tile = Instantiate(nodePrefab, current + new Vector3(.25f, .25f, 0), new Quaternion(0, 0, 0, 1));
                    tile.transform.SetParent(parent.transform);
                    tile.name = tile.GetInstanceID().ToString();
                    tile.GetComponent<TileNode>().x = i;
                    tile.GetComponent<TileNode>().y = j;
                    tile.GetComponent<TileNode>().SetWalkable(walkable);
                    tiles[i + 1].Add(tile.GetComponent<TileNode>());
                    j++;
                }
                i++;
            }

            /// iterate over every node in the tile matrix
            for (int x = 2; x < tiles.Count - 1; x++)
            {
                for (int y = 1; y < tiles[x].Count - 1; y++)
                {
                    // for every node, get it's neighbors list
                    var node = tiles[x][y];
                    if (!node.walkable)
                    {
                        continue;
                    }
                    var neighbors = node.GetComponent<TileNode>().neighbors;
                    // index error bug if looping is done???
                    //neighbors.Add(tiles[x - 1][y - 1]);
                    neighbors.Add(tiles[x - 1][y]);
                    //neighbors.Add(tiles[x - 1][y + 1]);
                    neighbors.Add(tiles[x][y - 1]);
                    neighbors.Add(tiles[x][y + 1]);
                    //neighbors.Add(tiles[x + 1][y - 1]);
                    neighbors.Add(tiles[x + 1][y]);
                    //neighbors.Add(tiles[x + 1][y + 1]);
                    var neighborsCopy = new List<TileNode>(neighbors);
                    foreach (var neighbor in neighborsCopy)
                    {
                        if (!neighbor.walkable)
                        {
                            neighbors.Remove(neighbor);
                        }
                    }
                }
            }
        }
    }
}
