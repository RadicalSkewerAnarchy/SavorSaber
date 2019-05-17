using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMacros : MonoBehaviour
{
    public const string undefined = "undefined";
    public static TextMacros instance;
    public delegate string MacroFn(string argument);

    private Dictionary<string, MacroFn> macroMap = new Dictionary<string, MacroFn>()
    {
        {"flag", (flag) => FlagManager.GetFlag(flag) }
    };

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(transform);
        }
        
    }

    public string ParseMacro(string command, string argument)
    {
        if(macroMap.ContainsKey(command))
            return macroMap[command](argument);
        return undefined;
    }
}
