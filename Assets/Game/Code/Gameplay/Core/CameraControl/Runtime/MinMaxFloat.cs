using System;
using UnityEngine;

namespace YellowSquad.Anthill.Core.CameraControl
{
    [Serializable]
    public struct MinMaxFloat
    {
        public MinMaxFloat(float min, float max)
        {
            Min = min;
            Max = max;
        }        
        
        [field: SerializeField] public float Min { get; private set; }
        [field: SerializeField] public float Max { get; private set; }
    }
}