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

    private void Awake()
    {
        SetWalkable(true);
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

}
