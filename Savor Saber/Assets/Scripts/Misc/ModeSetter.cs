using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSetter : MonoBehaviour
{
    public Text buttonText;
    public const string modeFlag = "goal";
    private void Awake()
    {
        if(!FlagManager.GlobalFlags.ContainsKey(modeFlag))
            FlagManager.GlobalFlags.Add(modeFlag, "marsh");
        SetButtonText();
    }
    public void ChangeMode()
    {
        if(FlagManager.GlobalFlags[modeFlag] == "marsh")
            FlagManager.GlobalFlags[modeFlag] = "desert";
        else
            FlagManager.GlobalFlags[modeFlag] = "marsh";
        SetButtonText();
    }
    private void SetButtonText()
    {
        buttonText.text = FlagManager.GlobalFlags[modeFlag].Replace('m', 'M').Replace('d', 'D') + "Mode";
    }
}
