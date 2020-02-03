using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFlagTrue(string flag)
    {
        FlagManager.SetFlag(flag, "True");
    }

    public void SetFlagFalse(string flag)
    {
        FlagManager.SetFlag(flag, "False");
    }
}
