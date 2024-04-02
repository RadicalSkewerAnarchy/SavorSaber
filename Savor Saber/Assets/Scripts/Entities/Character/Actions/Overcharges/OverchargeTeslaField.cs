using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverchargeTeslaField : Overcharge
{
    public GameObject TeslaField;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Activate()
    {
        base.Activate();
        TeslaField.SetActive(true);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        TeslaField.SetActive(false);
    }
}
