using System;
using UnityEngine;

namespace YellowSquad.Utils
{
    [Serializable]
    public struct MinMaxFloat
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;
        
        public MinMaxFloat(float min, float max)
        {
            _min = min;
            _max = max;
        }

        public float Min => _min;
        public float Max => _max;
    }
}