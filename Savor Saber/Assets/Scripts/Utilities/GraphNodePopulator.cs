using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class GraphNodePopulator : MonoBehaviour
{
    public GameObject nodePrefab;
    public Tile[] discoveredTiles;
    public Tilemap navMeshData;
    Tilemap[] activeTileMaps;
    BoundsInt bounds;
    bool setCollision = false;
    private void ActivateTileNodes(){
        
        /// List of all tilemaps
        activeTileMaps = GetComponentsInChildren<Tilemap>();
        foreach(var activeTiles in activeTileMaps)
        {
            //setCollision = false;
            /// retrieve renderer for layer data
            if (activeTiles.GetComponent<TilemapCollider2D>() == null)
            {
                break;
                //parent = navMeshData.transform.GetChild(1);
            }
            /// tile bounds set based on current tilemap
            bounds = activeTiles.cellBounds;
            /// iterates through tilemap based on bounds
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    /// sets local position hard casted as int based on current x,y iteration
                    Vector3Int local = new Vector3Int(x, y, (int)activeTiles.transform.position.y);
                    /// takes real world x,y position as an int and gets specific tile from tilemap at that location
                    Vector3 current = activeTiles.CellToWorld(local);
                    var parent = navMeshData.transform.GetChild(0);
                    if (activeTiles.HasTile(local))
                    {
                        parent = navMeshData.transform.GetChild(1);
                    }
                    GameObject tile = Instantiate(nodePrefab, current + new Vector3(.5f, .5f, 0), new Quaternion(0, 0, 0, 1));
                    tile.transform.SetParent(parent.transform);
                    tile.GetComponent<TileNode>().isCollision = setCollision;
                }
            }
        }       
    }
}
