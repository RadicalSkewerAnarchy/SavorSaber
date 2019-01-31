using System.Collections;
using System.Collections.Generic;
using SerializableCollections;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AIData))]
public class UtilityCurves : MonoBehaviour
{
    public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    public AIStates aiStates = new AIStates();
    public MacroDict macroValues = new MacroDict();
    private Dictionary<string, float> macroCache = new Dictionary<string, float>();
    public AIData data;

    // Use this for initialization
    void Start()
    {
        data = GetComponent<AIData>();
    }

    // Update is called once per frame
    void Update()
    {
        // nothing
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns> a string representing a key to the enum state in AIData </returns>
    public AIData.Protocols DecideState()
    {
        macroCache.Clear();
        AIData.Protocols maxState = AIData.Protocols.Lazy;
        float max = 0;
        foreach(var kvp in aiStates)
        {
            Debug.Log("=== " + kvp.Key);
            float utility = SumCurves(kvp.Value);
            Debug.Log(">>> " + kvp.Key + " Utility: " + utility);
            if (utility > max)
            {
                max = utility;
                maxState = kvp.Key;
            }          
        }
        return maxState;
    }

    private float EvaluateAttribute(AnimationCurve cur, float value)
    {
        // evaluate with respect to curve
        //      example: curve = (y = x^2), ret = 0.5, eva = 0.25
        float eva = cur.Evaluate(value);

        // return
        return eva;
    }

    /// <summary> Returns the macro's value from the cache if it has been calculated this frame.
    /// Else, calculates the macro's value and caches it</summary>
    private float GetMacroValue(string macroName)
    {
        Debug.Log("Calculating Macro value: " + macroName);
        if (!macroCache.ContainsKey(macroName))
            macroCache.Add(macroName, SumCurves(macroValues[macroName]));
        return macroCache[macroName];
    }

    private float SumCurves(CurveDict curves)
    {
        // attributes is TWICE as long as curves
        // curves[i] ==> attributes[2i] and attributes[2i+1]
        //      where in attributes: 2i is the now value and 2i+1 is the max value

        // return variables
        float sum = 0;
        int count = 0;

        // loop variables
        AnimationCurve c;
        string key;
        float a;

        // extrapolate, iterate, sum
        foreach(var curvePair in curves)
        {
            c = curvePair.Value;
            key = curvePair.Key;
            // Use the Macro value if one exists, else get the value from the AI data
            a = macroValues.ContainsKey(key) ? GetMacroValue(key) : data.getNormalizedValue(key);
            float val = EvaluateAttribute(c, a);
            Debug.Log(">>>>>>" + curvePair.Key + " value: " + a + " weight: " + val);
            // If the value is less than 0, do not factor it in to the utility
            if (val >= 0)
            {
                sum += val;
                ++count;
            }            
        }

        // return the average
        return count == 0 ? 0 : (sum / count);
    }

    [System.Serializable]
    public class AIStates : SDictionary<AIData.Protocols, CurveDict> { }
    [System.Serializable]
    public class MacroDict : SDictionary<string, CurveDict> { }
    [System.Serializable]
    public class CurveDict : SDictionary<string, AnimationCurve> { }
}
