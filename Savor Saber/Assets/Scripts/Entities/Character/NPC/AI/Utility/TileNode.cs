using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class TileNode : MonoBehaviour
{
    public float x = 0, y = 0;
    public bool walkable = true;
	public bool active = false;
    public List<TileNode> neighbors;
    private Collider2D[] overlappingTile = null;

    [HideInInspector]
    public bool valid = true;

    private void Awake()
    {
        SetWalkable(true);
        CheckOverlap();
        CleanNeighbors();
    }

    public void SetWalkable(bool on)
    {
        walkable = on;
        if (!walkable)
        {
            Destroy(gameObject.GetComponent<BoxCollider2D>());
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TileNode tn = collision.GetComponent<TileNode>();
        //Debug.Log(this.name + " collided with " + tn.name);
        if (tn != null)
        {
            if (!this.neighbors.Contains(tn))
            {
                tn.neighbors.Add(this);
                this.neighbors.Add(tn);
            }
        }
    }

    private void CheckOverlap()
    {
        overlappingTile = Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 1), 0f);

        if (overlappingTile.Length == 0)
            return;
        else
        {
            foreach (Collider2D collider in overlappingTile)
            {
                if (collider.gameObject.tag == "Scenery" || collider.gameObject.tag == "LargePlant")
                {

                    foreach(TileNode neighbor in neighbors)
                    {
                        neighbor.neighbors.Remove(this);
                    }
                    //Destroy(this.gameObject);
                    valid = false;
                    //GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
            return;
        }
    }

    private void CleanNeighbors()
    {
        foreach(TileNode tile in neighbors)
        {
            if (tile.Equals(null))
            {
                Debug.Log("Pruning null tile...");
                neighbors.Remove(tile);
            }
        }
    }
}
