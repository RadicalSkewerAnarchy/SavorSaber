using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreNonPathColliders : MonoBehaviour
{
    CompositeCollider2D compCol;
    // Start is called before the first frame update
    void Start()
    {
        compCol = GetComponent<CompositeCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<AIData>() != null)
        {
            if (collision.GetType() != typeof(BoxCollider2D))
            {
                Physics2D.IgnoreCollision(compCol, collision.collider);
            }
        }
        
    }
}
