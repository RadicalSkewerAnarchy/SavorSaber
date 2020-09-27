using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicMulti : MonoBehaviour
{
    public enum LogicType
    {
        AND,
        OR,
        XOR
    }

    public LogicType type = LogicType.AND;
    [HideInInspector]
    public bool[] inputs = new bool[2];
    public PoweredObject[] targetObjects;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInputValue(int input, bool value)
    {
        inputs[input] = value;

        //Logic AND
        if(type == LogicType.AND)
        {
            if(inputs[0] && inputs[1])
            {
                foreach (PoweredObject target in targetObjects)
                {
                    target.TurnOn();
                }
            }
            else
            {
                foreach (PoweredObject target in targetObjects)
                {
                    target.ShutOff();
                }
            }
        }
        else if(type == LogicType.OR)
        {
            if (inputs[0] || inputs[1])
            {
                foreach (PoweredObject target in targetObjects)
                {
                    target.TurnOn();
                }
            }
            else
            {
                foreach (PoweredObject target in targetObjects)
                {
                    target.ShutOff();
                }
            }
        }
        else if (type == LogicType.XOR)
        {
            if (inputs[0] ^ inputs[1])
            {
                foreach (PoweredObject target in targetObjects)
                {
                    target.TurnOn();
                }
            }
            else
            {
                foreach (PoweredObject target in targetObjects)
                {
                    target.ShutOff();
                }
            }
        }
        else
        {
            Debug.Log("Logic gate error: " + type + " is not a supported operation type.");
        }
    }
}
