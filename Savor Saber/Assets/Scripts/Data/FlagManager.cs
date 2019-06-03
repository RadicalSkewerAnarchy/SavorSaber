using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Temporary flag manager until we need something more specific
/// </summary>
public class FlagManager : MonoBehaviour
{ 
    public static Dictionary<string, string> GlobalFlags { get; } = new Dictionary<string, string>();
    private static FlagManager instance;
    public const string undefined = "undefined";
    private Dictionary<string, string> flags = new Dictionary<string, string>();
    public List<string> keysToInit = new List<string>();
    public List<string> initValues = new List<string>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeFlags();
        }
        else
            Destroy(gameObject);
    }
    private void InitializeFlags()
    {
        for (int i = 0; i < Mathf.Min(keysToInit.Count, initValues.Count); ++i)
            SetFlag(keysToInit[i], initValues[i]);
    }
    /// <summary> Set a global flag that does not reset on scene load </summary>
    public static void SetGlobal(string flag, string data)
    {
        if (GlobalFlags.ContainsKey(flag))
            GlobalFlags[flag] = data;
        else
            GlobalFlags.Add(flag, data);
    }
    /// <summary> Set a flag that resets on scene load </summary>
    public static void SetFlag(string flag, string data)
    {
        if (instance.flags.ContainsKey(flag))
            instance.flags[flag] = data;
        else
            instance.flags.Add(flag, data);
    }
    /// <summary> Get the value of a flag (global or instance)</summary>
    public static string GetFlag(string flag)
    {
        if (GlobalFlags.ContainsKey(flag))
            return GlobalFlags[flag];
        else if(instance.flags.ContainsKey(flag))
            return instance.flags[flag];
        Debug.LogWarning("Flag: " + flag + " does not exist!. returning \"" + undefined + "\"");
        return undefined;
    }
}
