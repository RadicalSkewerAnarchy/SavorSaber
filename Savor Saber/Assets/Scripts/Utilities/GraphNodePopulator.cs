using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//[ExecuteInEditMode]
public class GraphNodePopulator : MonoBehaviour
{
    public GameObject nodePrefab;
    public Tile[] discoveredTiles;
    public List<List<TileNode>> tiles;
	public TileNode [][] tilesArr;
    Tilemap[] activeTileMaps;
    BoundsInt bounds;
    bool walkable;
    int clusterLimit = 3;
    private void Awake()
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
        //var inactiveTiles = inactiveTileMaps[1];
        //Debug.Log("Inactive and active tiles found");
        /// iterates through tilemap based on bounds
        int i = 0, j = 0;
        bounds = activeTiles.cellBounds;
		tilesArr = new TileNode[bounds.xMax][];
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            j = 0;
            /// list of lists, this increments the X counter
            tiles.Add(new List<TileNode>());
			tilesArr[j] = new TileNode[bounds.yMax];
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                walkable = true;
                /// sets local position hard casted as int based on current x,y iteration
                Vector3Int local = new Vector3Int(x, y, (int)activeTiles.transform.position.z);
                //Vector3Int localInactive = new Vector3Int(x, y, (int)inactiveTiles.transform.position.z);
                /// takes real world x,y position as an int and gets specific tile from tilemap at that location
                Vector3 current = activeTiles.CellToWorld(local);
                //inactiveTiles.CellToWorld(localInactive);
                var parent = activeTiles.transform.GetChild(0);
                if(activeTiles.HasTile(local)/* || inactiveTiles.HasTile(local)*/)
                {
                    walkable = false;
                }
                if((y % clusterLimit == 0) && (x % clusterLimit == 0) && walkable)
                {
                    GameObject tile = Instantiate(nodePrefab, current + new Vector3(.25f,.25f), new Quaternion(0, 0, 0, 1));
                    tile.transform.SetParent(parent.transform);
                    tile.name = tile.GetInstanceID().ToString();
                    tile.GetComponent<TileNode>().x = i;
                    tile.GetComponent<TileNode>().y = j;
                    tile.GetComponent<TileNode>().SetWalkable(walkable);
					tile.GetComponent<TileNode>().active = true;
                    tiles[i].Add(tile.GetComponent<TileNode>());
					//tilesArr[i][j] = tile.GetComponent<TileNode>();
					j+=clusterLimit;
                }else{
					var tempTile = new TileNode();
					tiles[i].Add(tempTile);
					j+=clusterLimit;
					//tilesArr[i][j] = tempTile;
				}
                //j++;
            }
            i++;
        }

        for (int x = 0; x < tiles.Count - clusterLimit; x++)
        {

                for (int y = 0; y < tiles[x].Count - clusterLimit; y++)
                {
					if(!tiles[x][y].active) continue;
                    var node = tiles[x][y];
                    for (int m = -clusterLimit; m <= clusterLimit; m+= clusterLimit)
                    {
                        for (int n = -clusterLimit; n <= clusterLimit; n+= clusterLimit)
                        {
							//node.neighbors.Add(tiles[x+m][y+n]);
							if(m == 0 && n == 0) continue;
                            try
                            {
								if(tiles[x+m][y+n].active)	node.neighbors.Add(tiles[x+m][y+n]);

                            //node.neighbors.Add(tiles[x + m][y + n]);
							//if((tiles[x+m][y+n].x != 0  ) && ( tiles[x+m][y+n].y != 0)){
							//	node.neighbors.Add(tiles[x+m][y+n]);
							//}
                            }
                            catch (System.ArgumentOutOfRangeException ex)
                            {
								//Debug.Log("OutOfRange");
								//Debug.Log("tiles.length + tiles[x].length " + tiles.Count +  " | " + tiles[x].Count + " | Responsible tiles | " + tiles[x][y].x + ", " + tiles[x][y].y);
                            }


                        }
                    }
                }
        	}
    }
}
