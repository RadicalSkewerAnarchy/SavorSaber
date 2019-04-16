using System.Collections;
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
    int clusterLimit = 2;
    private void Start()
    {
        tiles = new List<List<TileNode>>();
        tiles.Add(new List<TileNode>());
        Populate();
    }
    private void Populate()
    {
        /// List of all tilemaps
        activeTileMaps = GetComponentsInChildren<Tilemap>();
        List<Tilemap> inactiveTileMaps = new List<Tilemap>();
        foreach(var active in activeTileMaps)
        {
            if(active.GetComponent<TilemapCollider2D>() != null)
            {
                inactiveTileMaps.Add(active);
            }
        }
        var activeTiles = inactiveTileMaps[0];
        var inactiveTiles = inactiveTileMaps[1];
        /// iterates through tilemap based on bounds
        int i = 0, j = 0;
        bounds = activeTiles.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax-50; x++)
        {
            j = 0;
            /// list of lists, this increments the X counter
            tiles.Add(new List<TileNode>());
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                walkable = true;
                /// sets local position hard casted as int based on current x,y iteration
                Vector3Int local = new Vector3Int(x, y, (int)activeTiles.transform.position.z);
                Vector3Int localInactive = new Vector3Int(x, y, (int)inactiveTiles.transform.position.z);
                /// takes real world x,y position as an int and gets specific tile from tilemap at that location
                Vector3 current = activeTiles.CellToWorld(local);
                inactiveTiles.CellToWorld(localInactive);
                var parent = activeTiles.transform.GetChild(0);
                if(activeTiles.HasTile(local) || inactiveTiles.HasTile(local))
                {
                    walkable = false;
                }
                if((x % clusterLimit == 0 && y % clusterLimit == 0) && walkable)
                {
                    GameObject tile = Instantiate(nodePrefab, current, new Quaternion(0, 0, 0, 1));
                    tile.transform.SetParent(parent.transform);
                    tile.name = tile.GetInstanceID().ToString();
                    tile.GetComponent<TileNode>().x = i;
                    tile.GetComponent<TileNode>().y = j;
                    tile.GetComponent<TileNode>().SetWalkable(walkable);
                    tiles[i].Add(tile.GetComponent<TileNode>());
                }
                j++;
            }
            i++;
        }
        if(tiles.Count > 4)
        {
            for (int x = 1; x < tiles.Count - 2; x++)
            {
                if (tiles[x].Count > 4)
                {
                    for (int y = 1; y < tiles[x].Count - 2; y++)
                    {
                        var node = tiles[x][y].GetComponent<TileNode>();
                        for (int m = -1; m <= 1; m++)
                        {
                            for (int n = -1; n <= 1; n++)
                            {
                                try
                                {
                                    node.neighbors.Add(tiles[x + m][y + n]);
                                }
                                catch (System.ArgumentOutOfRangeException ex)
                                {

                                }
                            }
                        }
                    }
                }
            }
        }               
    }
}
