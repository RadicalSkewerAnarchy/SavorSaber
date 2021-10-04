using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noclip : MonoBehaviour
{
    private Collider2D collision;
    private bool noclip = false;
    // Start is called before the first frame update
    void Start()
    {
        collision = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Noclip"))
        {
            if (noclip)
            {
                collision.enabled = true;
                noclip = false;
            }
            else
            {
                collision.enabled = false;
                noclip = true;
            }
        }
    }

    
}
