using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Temporary flag manager until we need something more specific
/// </summary>
public class FlagManager : MonoBehaviour, IDataPersistence
{ 
    public static Dictionary<string, string> GlobalFlags { get; } = new Dictionary<string, string>();
    private static FlagManager instance;
    public const string undefined = "undefined";
    private Dictionary<string, string> flags = new Dictionary<string, string>();
    public List<string> keysToInit = new List<string>();
    public List<string> initValues = new List<string>();


    
    public void LoadData(GameData data)
    {
        //flags = new Dictionary<string, string>(data.flags);
        Debug.Log("Entering FlagManager LoadData()");

        // i hate myself
        // please forgive me
        for(int i = 0; i < data.flagKeys.Length; i++)
        {
            if(data.flagKeys[i] != null)
            {
                if (flags.ContainsKey(data.flagKeys[i]))
                {
                    flags[data.flagKeys[i]] = data.flagValues[i];
                }
                else
                {
                    flags.Add(data.flagKeys[i], data.flagValues[i]);
                }

            }
        }

    }


    public void SaveData(ref GameData data)
    {
        //data.flags = new Dictionary<string, string>(flags);
        //SDictionary<string, string> sDict = new SDictionary<string, string>();
        // TEMPORARY FLAG STORAGE SOLUTION, REPLACE ASAP
        // clear arrays
        for (int j = 0; j < data.flagKeys.Length; j++)
        {
            data.flagKeys[j] = null;
            data.flagValues[j] = null;
        }

        int i = 0;
        foreach(var kvp in flags)
        {
            if(i >= data.flagValues.Length)
            {
                Debug.LogError("Error: Too many flags to save in GameData");
                break;
            }
            data.flagKeys[i] = kvp.Key;
            data.flagValues[i] = kvp.Value;
            i++;
        }
    }

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
        if (instance == null)
            return undefined;

        if (GlobalFlags.ContainsKey(flag))
            return GlobalFlags[flag];
        else if(instance.flags.ContainsKey(flag))
            return instance.flags[flag];
        //Debug.LogWarning("Flag: " + flag + " does not exist!. returning \"" + undefined + "\"");
        return undefined;
    }

    /// <summary> Get the dictionary of this instance's flags</summary>
    public Dictionary<string, string> GetInstanceFlagDictionary()
    {
        if (instance == null)
            return null;

        return instance.flags;
    }
    
    public void SaveFlags()
    {
        for(int i = 0; i < flags.Count; i++)
        {
            
        }
        
    }
}
