using UnityEngine;

namespace YellowSquad.Utils
{
    public static class MinMaxFloatExtensions
    {
        public static float Clamp(this MinMaxFloat minMax, float value)
        {
            return Mathf.Clamp(value, minMax.Min, minMax.Max);
        }

        public static float Lerp(this MinMaxFloat minMax, float t)
        {
            return Mathf.Lerp(minMax.Min, minMax.Max, t);
        }

        public static float InverseLerp(this MinMaxFloat minMax, float value)
        {
            return Mathf.InverseLerp(minMax.Min, minMax.Max, value);
        }
    }
}