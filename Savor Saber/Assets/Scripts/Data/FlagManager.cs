using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Temporary flag manager until we need something more specific
/// </summary>
public class FlagManager : MonoBehaviour
{
    private static FlagManager instance;
    public const string undefined = "undefined";
    private Dictionary<string, string> flags = new Dictionary<string, string>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.transform);
        }
        else
            Destroy(this);
    }
    public static void SetFlag(string flag, string data)
    {
        if (instance.flags.ContainsKey(flag))
            instance.flags[flag] = data;
        else
            instance.flags.Add(flag, data);
    }
    public static string GetFlag(string flag)
    {
        if(instance.flags.ContainsKey(flag))
            return instance.flags[flag];
        Debug.LogWarning("Flag: " + flag + " does not exist!. returning \"" + undefined + "\"");
        return undefined;
    }
}
