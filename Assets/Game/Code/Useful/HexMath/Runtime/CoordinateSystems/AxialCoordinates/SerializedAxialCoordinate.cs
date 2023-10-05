using System;
using UnityEngine;

namespace YellowSquad.HexMath
{
    [Serializable]
    public struct SerializedAxialCoordinate
    {
        public SerializedAxialCoordinate(int q, int r)
        {
            Q = q;
            R = r;
        }
        
        [field: SerializeField] public int Q { get; private set; }
        [field: SerializeField] public int R { get; private set; }
        
        public static implicit operator AxialCoordinate(SerializedAxialCoordinate serializedAxialCoordinate) 
            => new(serializedAxialCoordinate.Q, serializedAxialCoordinate.R);
        
        public static implicit operator SerializedAxialCoordinate(AxialCoordinate axialCoordinate) 
            => new(axialCoordinate.Q, axialCoordinate.R);
    }
}