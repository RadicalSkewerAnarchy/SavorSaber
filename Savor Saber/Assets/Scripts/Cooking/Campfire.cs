using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Currently unused, just an alternate way to check if the player is
/// near a campfire.
/// </summary>
public class Campfire : MonoBehaviour
{
    private Inventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Player near campfire");
            playerInventory = collision.gameObject.GetComponent<Inventory>();
            if(playerInventory != null)
            {
                playerInventory.nearCampfire = true;
            }
        }
        else if(collision.gameObject.tag == "Monster")
        {
            Debug.Log("Monster near campfire");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player left campfire");
            playerInventory = collision.gameObject.GetComponent<Inventory>();
            if (playerInventory != null)
            {
                playerInventory.nearCampfire = false;
            }
        }
        else if (collision.gameObject.tag == "Monster")
        {
            Debug.Log("Monster left campfire");
        }
    }
}
