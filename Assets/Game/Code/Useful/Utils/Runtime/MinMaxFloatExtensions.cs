using UnityEngine;

namespace YellowSquad.Utils
{
    public static class MinMaxFloatExtensions
    {
        public static float Clamp(this MinMaxFloat minMax, float value)
        {
            return Mathf.Clamp(value, minMax.Min, minMax.Max);
        }
    }
}