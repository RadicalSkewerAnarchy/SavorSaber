using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ShadowTint : MonoBehaviour
{
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
        SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
        if(sr != null && collision.gameObject != transform.parent.gameObject)
        {
            sr.color = new Color(0.75f, 0.75f, 0.75f);

            //does the object have a shadow? If so, make it invisible
            ShadowTint shadow = collision.gameObject.GetComponentInChildren<ShadowTint>();
            if(shadow != null)
            {
                Debug.Log("Detecing shadow on enter");
                SpriteRenderer shadowSR = shadow.gameObject.GetComponent<SpriteRenderer>();
                shadowSR.enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
        if (sr != null && collision.gameObject != transform.parent.gameObject)
        {
            sr.color = Color.white;

            //does the object have a shadow? If so, make it invisible
            ShadowTint shadow = collision.gameObject.GetComponentInChildren<ShadowTint>();
            if (shadow != null)
            {
                Debug.Log("Detecting shadow on exit");
                SpriteRenderer shadowSR = shadow.gameObject.GetComponent<SpriteRenderer>();
                shadowSR.enabled = true;
            }
        }
    }
}
