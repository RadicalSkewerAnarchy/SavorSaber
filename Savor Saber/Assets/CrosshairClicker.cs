using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairClicker : MonoBehaviour
{
    Vector2 area;
    // Start is called before the first frame update
    void Start()
    {
        Collider2D temp = this.GetComponent<Collider2D>();
        area = new Vector2(temp.bounds.extents.x, temp.bounds.extents.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// returns the game objects within the bounds of 
    /// the 2D collider
    /// </summary>
    /// <returns>all fruits</returns>
    public List<GameObject> ClickArea()
    {
        List <GameObject> all = new List<GameObject>();
        Debug.Log(this.name + " start ClickArea():");

        // find them
        Collider2D[] collected = Physics2D.OverlapBoxAll(this.transform.position, area, 0);

        // add their gameobjects
        foreach (Collider2D member in collected)
        {
            GameObject go = member.gameObject;
            Debug.Log(" has been clicked on..." + go.name + " has been clicked on...");
            all.Add(go);
        }

        Debug.Log(this.name + " returning Clicked On area.");
        return all;
    }
}
