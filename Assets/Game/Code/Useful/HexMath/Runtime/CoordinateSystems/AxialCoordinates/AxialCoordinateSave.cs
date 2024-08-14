using System;
using Newtonsoft.Json;

namespace YellowSquad.HexMath
{
    [Serializable]
    public struct AxialCoordinateSave
    {
        [JsonProperty] public int Q;
        [JsonProperty] public int R;
        
        public AxialCoordinateSave(AxialCoordinate coordinate)
        {
            Q = coordinate.Q;
            R = coordinate.R;
        }
        
        public static implicit operator AxialCoordinateSave(AxialCoordinate axialCoordinate) 
            => new(axialCoordinate);

        public static implicit operator AxialCoordinate(AxialCoordinateSave serializedAxialCoordinate) 
            => new(serializedAxialCoordinate.Q, serializedAxialCoordinate.R);
    }
}