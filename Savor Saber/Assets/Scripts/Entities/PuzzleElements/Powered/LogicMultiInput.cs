using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicMultiInput : PoweredObject
{
    private LogicMulti baseMulti;
    public int inputIndex;
    // Start is called before the first frame update
    void Start()
    {
        baseMulti = GetComponentInParent<LogicMulti>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TurnOn()
    {
        base.TurnOn();
        baseMulti.SetInputValue(inputIndex, true);
    }

    public override void ShutOff()
    {
        base.ShutOff();
        baseMulti.SetInputValue(inputIndex, false);
    }
}
