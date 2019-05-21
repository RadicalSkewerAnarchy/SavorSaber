using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSingleton
{
    private static GraphSingleton instance = null;
    public List<GameObject> tiles = null;
    GraphSingleton()
    {
        tiles = new List<GameObject>();
        //Debug.Log("Singleton is created");
    }
    
    public static GraphSingleton Instance
    {
        get {
            if (instance == null)
            {
                instance = new GraphSingleton();

            }
            return instance;
        }
    }
}
