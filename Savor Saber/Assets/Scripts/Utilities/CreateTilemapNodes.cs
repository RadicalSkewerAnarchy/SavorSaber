using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class CreateTilemapNodes : MonoBehaviour
{
    // Used for design in tilemap node population
    // https://www.youtube.com/watch?v=htZFdfSLiYo
    //

    public Grid grid;
    public Tilemap floor;
    public List<Tilemap> obstacleLayers;
    public GameObject nodePrefab;
}

