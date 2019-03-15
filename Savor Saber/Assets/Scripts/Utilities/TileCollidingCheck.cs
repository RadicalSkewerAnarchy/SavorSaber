using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollidingCheck : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetType() != typeof(BoxCollider2D) && collision.gameObject.tag != "Player")
        {
            Physics2D.IgnoreCollision(GetComponent<CompositeCollider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
    }
}
