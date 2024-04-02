using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overcharge : MonoBehaviour
{
    [HideInInspector]
    public bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Activate()
    {
        active = true;
    }

    public virtual void Deactivate()
    {
        active = false;
    }
}
