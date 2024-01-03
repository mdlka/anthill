using System;

namespace YellowSquad.HexMath
{
    /// <summary>
    /// Axial coordinate system
    /// Description <see href="https://www.redblobgames.com/grids/hexagons/">here</see>
    /// </summary>
    public readonly struct FracAxialCoordinate : IEquatable<FracAxialCoordinate>
    {
        private const float Epsilon = 1e-6f;
        private readonly int _hash;
        
        public FracAxialCoordinate(float q, float r)
        {
            Q = q;
            R = r;
            _hash = HashCode.Combine(Q, R);
        }

        public float Q { get; }
        public float R { get; }

        public AxialCoordinate AxialRound() 
            => new FracCubeCoordinate(Q, R).CubeRound().ToAxial();

        public bool Equals(FracAxialCoordinate other) 
            => Math.Abs(Q - other.Q) < Epsilon && Math.Abs(R - other.R) < Epsilon;

        public override bool Equals(object obj) 
            => obj is FracAxialCoordinate other && Equals(other);

        public override int GetHashCode() => _hash;
        public override string ToString() => $"({Q}, {R})";
        
        public static FracAxialCoordinate operator +(FracAxialCoordinate a, FracAxialCoordinate b) => new FracAxialCoordinate(a.Q + b.Q, a.R + b.R);
        public static FracAxialCoordinate operator -(FracAxialCoordinate a, FracAxialCoordinate b) => new FracAxialCoordinate(a.Q - b.Q, a.R - b.R);
        public static bool operator ==(FracAxialCoordinate a, FracAxialCoordinate b) => a.Equals(b);
        public static bool operator !=(FracAxialCoordinate a, FracAxialCoordinate b) => !(a == b);
    }
}