using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkewerAttack : BaseMeleeAttack
{

    public Inventory inventory;

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SkewerableObject" && !inventory.ActiveSkewerFull() && !inventory.ActiveSkewerCooked())
        {
            Debug.Log("Hit skewerable object");
            SkewerableObject targetObject = collision.gameObject.GetComponent<SkewerableObject>();

            inventory.AddToSkewer(targetObject.data);
            Destroy(collision.gameObject);
        }
    }
}   
