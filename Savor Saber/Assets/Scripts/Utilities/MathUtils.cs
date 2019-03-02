using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathUtils
{
    [System.Serializable]
    public class FloatRange
    {
        public float min;
        public float max;

        public FloatRange(float min, float max)
        {
            this.min = min;
            this.max = max;
            if (!IsValid())
                throw new System.ArgumentException("Min: " + min + " and " + "Max: " + max + " is an invalid range");
        }
        public FloatRange(float minMax)
        {
            min = minMax;
            max = minMax;
        }

        public bool IsValid()
        {
            return min <= max;
        }
        public bool Contains(float item)
        {
            return (item >= min) && (item <= max);
        }
        public float Clamp(float value)
        {
            if (value <= max)
                return value >= min ? value : min;
            return max;
        }
        public void Shift(float shiftBy)
        {
            min += shiftBy;
            max += shiftBy;
        }
        public FloatRange Shifted(float shiftBy)
        {
            return new FloatRange(min + shiftBy, max + shiftBy);
        }
        public float Lerp(float t)
        {
            return Mathf.Lerp(min, max, t);
        }
    }

    [System.Serializable]
    public class IntRange
    {
        public int Count { get { return max - min + 1; } }
        public int min;
        public int max;

        public IntRange(int min, int max)
        {
            this.min = min;
            this.max = max;
            if (!isValid())
                throw new System.ArgumentException("Min: " + min + " and " + "Max: " + max + " is an invalid range");
        }
        public IntRange(int minMax)
        {
            min = minMax;
            max = minMax;
        }

        public bool isValid()
        {
            return min <= max;
        }
        public bool contains(int item)
        {
            return (item >= min) && (item <= max);
        }
        public int clamp(int value)
        {
            if (value <= max)
                return value >= min ? value : min;
            return max;
        }
        public void shift(int shiftBy)
        {
            min += shiftBy;
            max += shiftBy;
        }
    }
}


