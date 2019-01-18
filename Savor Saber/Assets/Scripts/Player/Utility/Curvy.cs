using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curvy : MonoBehaviour
{

    public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));


    public int hp = 50;
    public int hpmax = 50;
	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    private float EvaluateAttribute(AnimationCurve cur, int now, int max)
    {
        // proportion of now to max
        //      0 <= ret <= 1
        float ret = (float)now / (float)max;

        // evaluate with respect to curve
        //      example: curve = (y = x^2), ret = 0.5, eva = 0.25
        float eva = cur.Evaluate(ret);

        // return
        return eva;
    }

    private float SumCurves(AnimationCurve[] curves, int[] attributes)
    {
        // attributes is TWICE as long as curves
        // curves[i] ==> attributes[2i] and attributes[2i+1]
        //      where in attributes: 2i is the now value and 2i+1 is the max value

        // return variables
        float sum = 0;
        int len = curves.Length;

        // loop variables
        AnimationCurve c;
        int a, b;

        // extrapolate, iterate, sum
        for (int i = 0; i < len; i++)
        {
            c = curves[i];
            a = attributes[2 * i];
            b = attributes[(2 * i) + 1];
            sum += EvaluateAttribute(c, a, b);
        }

        // return the average
        return (sum / len);
    }
}
