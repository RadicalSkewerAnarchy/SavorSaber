using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDToggle : MonoBehaviour
{

    public GameObject targetToHide;
    private bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            active = !active;
            targetToHide.SetActive(active);
        }
    }
}
