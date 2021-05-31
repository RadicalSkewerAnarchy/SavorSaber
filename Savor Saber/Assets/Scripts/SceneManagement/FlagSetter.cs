using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSetter : MonoBehaviour
{
    public bool resetFlagsOnLoad = false;

    [Header("Flags to automatically set")]
    public string[] flagsToBeTrue;
    public string[] flagsToBeFalse;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        SetAutoFlags();
    }

    private void SetAutoFlags()
    {
        foreach (string flag in flagsToBeTrue)
        {
            if (resetFlagsOnLoad)
            {
                SetFlagTrue(flag);
            }
            else if (!((FlagManager.GetFlag(flag) == "True") || (FlagManager.GetFlag(flag) == "False")))
            {
                SetFlagTrue(flag);
            }

        }

        foreach (string flag in flagsToBeFalse)
        {
            if (resetFlagsOnLoad)
            {
                SetFlagFalse(flag);
            }
            else if (!((FlagManager.GetFlag(flag) == "True") || (FlagManager.GetFlag(flag) == "False")))
            {
                SetFlagFalse(flag);
            }
        }
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
