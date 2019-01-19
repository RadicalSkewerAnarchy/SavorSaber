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
    public AIData data;

    // Use this for initialization
    void Start()
    {
        data = GetComponent<AIData>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move later
        if(Input.GetKeyDown(KeyCode.U))
            Debug.Log("Picked state: " + decideState());
    }

    public string decideState()
    {
        //Dictionary<string, float> utilityValues = new Dictionary<string, float>();
        string maxState = "";
        float max = 0;
        foreach(var kvp in aiStates)
        {
            Debug.Log("State: " + kvp.Key);
            //utilityValues.Add(kvp.Key, SumCurves(kvp.Value));
            float utility = SumCurves(kvp.Value);
            Debug.Log(kvp.Key + " utility: " + utility);
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
        float a;

        // extrapolate, iterate, sum
        foreach(var curvePair in curves)
        {
            c = curvePair.Value;
            a = data.getNormalizedValue(curvePair.Key.ToLower());
            float val = EvaluateAttribute(c, a);
            Debug.Log(curvePair.Key + " value: " + a + " weight: " + val);
            if(val >= 0)
            {
                sum += val;//EvaluateAttribute(c, a);
                ++count;
            }            
        }

        // return the average
        return count == 0 ? 0 : (sum / count);
    }

    [System.Serializable]
    public class AIStates : SDictionary<string, CurveDict> { }
    [System.Serializable]
    public class CurveDict : SDictionary<string, AnimationCurve> { }
}
