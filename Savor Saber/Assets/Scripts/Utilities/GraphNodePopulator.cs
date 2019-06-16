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
	public TileNode [,] tilesArr = new TileNode[1000,1000];
    Tilemap[] activeTileMaps;
    BoundsInt bounds;
    bool walkable;
    int clusterLimit = 3;
    public GraphSingleton graph;

    public int birthPlace = 0; 

    private void Awake()
    {
        tiles = new List<List<TileNode>>();
        tiles.Add(new List<TileNode>());


        string gridCheck = this.gameObject.scene.name;
        birthPlace = (gridCheck == "Plains" ? 1 : ((gridCheck == "Marsh" ? 2 : (gridCheck == "Desert" ? 3 : 0))));

        //tilesArr = new TileNode[,][];
        Populate();
    }
    private void Populate()
    {
        /// List of all tilemaps
        activeTileMaps = GetComponentsInChildren<Tilemap>();
        List<Tilemap> inactiveTileMaps = new List<Tilemap>();
        List<Tilemap> groundTileMaps = new List<Tilemap>();

        graph = GraphSingleton.Instance;

        var localGrass = new Tilemap();
        foreach(var active in activeTileMaps)
        {
            if(active.GetComponent<TilemapCollider2D>() != null)
            {
                inactiveTileMaps.Add(active);
            }
            if(active.name == "Grass" || active.name == "Ground" || active.tag == "Walkable") groundTileMaps.Add(active);  //localGrass = active;

        }   
        var activeTiles = inactiveTileMaps[0];
        //var inactiveTiles = inactiveTileMaps[1];
        //Debug.Log("Inactive and active tiles found");
        /// iterates through tilemap based on bounds
        int i = 0, j = 0;
        bounds = activeTiles.cellBounds;
        //tilesArr[i] = new TileNode[1000];
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            j = 0;
            /// list of lists, this increments the X counter
            tiles.Add(new List<TileNode>());
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                walkable = true;
                bool ground = false;
                /// sets local position hard casted as int based on current x,y iteration
                Vector3Int local = new Vector3Int(x, y, (int)activeTiles.transform.position.z);
                //Vector3Int localInactive = new Vector3Int(x, y, (int)inactiveTiles.transform.position.z);
                /// takes real world x,y position as an int and gets specific tile from tilemap at that location
                Vector3 current = activeTiles.CellToWorld(local);
                //inactiveTiles.CellToWorld(localInactive);
                var parent = activeTiles.transform.GetChild(0);
                foreach(var collisionTiles in inactiveTileMaps){
                    if(collisionTiles.HasTile(local)/* || inactiveTiles.HasTile(local)*/)
                    {
                        walkable = false;
                    }
                }
                foreach(var groundTiles in groundTileMaps){
                    if(groundTiles.HasTile(local)){
                        ground = true;
                        //Debug.Log("Found ground");
                    }
                }
                if((y % clusterLimit == 0) && (x % clusterLimit == 0) && walkable && ground)// && localGrass.HasTile(local))
                {
                    GameObject tile = Instantiate(nodePrefab, current + new Vector3(.25f,.25f), new Quaternion(0, 0, 0, 1));
                    tile.transform.SetParent(parent.transform);
                    tile.name = tile.GetInstanceID().ToString();
                    TileNode node = tile.GetComponent<TileNode>();
                    node.x = i;
                    node.y = j;
                    node.birthPlace = this.birthPlace;
                    node.SetWalkable(walkable);
					node.active = true;
                    tiles[i].Add(node);
                    tilesArr[i, j] = node;
                }
                j++;
            }
            i++;
        }

        for (int x = clusterLimit; x < tilesArr.GetLength(0) - clusterLimit; x++)
        {
             //Debug.Log(x);
            //Debug.Log(tilesArr.GetLength(1));        
            if(tilesArr.GetLength(1) <= clusterLimit ) continue;
            for (int y = clusterLimit; y < tilesArr.GetLength(1) - clusterLimit; y++)
            {
                if(tilesArr[x,y] == null) continue;


				//if(!tiles[x][y].active) continue;
                //var node = tiles[x][y];
                var node = tilesArr[x,y];
                for (int m = -clusterLimit; m <= clusterLimit; m+= clusterLimit)
                {
                    for (int n = -clusterLimit; n <= clusterLimit; n+= clusterLimit)
                    {
						//node.neighbors.Add(tiles[x+m][y+n]);
						if(m == 0 && n == 0) continue;
                        try
                        {
                            
							//if(tiles[x+m][y+n].active)	node.neighbors.Add(tiles[x+m][y+n]);
                            if(tilesArr[x+m, y+n] != null) {
                                if(tilesArr[x+m,y+n].active) node.neighbors.Add(tilesArr[x+m, y+n]);
                            }

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
        foreach(var tileList in tiles)
        {
            tileList.RemoveAll(tile => tile.active == false);
        }
    }
}
