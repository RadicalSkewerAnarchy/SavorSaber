using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverchargeHealingEgg : Overcharge
{

    public GameObject eggTemplate;
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
        Instantiate(eggTemplate, transform.position, Quaternion.identity);
        Deactivate();
    }
}
