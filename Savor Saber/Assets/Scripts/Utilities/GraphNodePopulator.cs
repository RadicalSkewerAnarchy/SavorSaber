using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//[ExecuteInEditMode]
public class GraphNodePopulator : MonoBehaviour
{
    public GameObject nodePrefab;
    Tilemap[] activeTileMaps;
    BoundsInt bounds;
    bool setCollision = false;
    private void Start()
    {
        /// List of all tilemaps
        activeTileMaps = GetComponentsInChildren<Tilemap>();
        foreach(var activeTiles in activeTileMaps)
        {
            /// retrieve renderer for layer data
            if (activeTiles.GetComponent<CompositeCollider2D>())
            {
                setCollision = true;
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
                    if (activeTiles.HasTile(local))
                    {
                        GameObject tile = Instantiate(nodePrefab, local + new Vector3(.5f, .5f, 0), new Quaternion(0, 0, 0, 1));
                        tile.transform.SetParent(GetComponentInParent<Grid>().transform);
                        tile.GetComponent<TileNode>().isCollision = setCollision;
                    }
                }
            }
        }       
    }
}
