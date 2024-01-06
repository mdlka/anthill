using System;

namespace YellowSquad.HexMath
{
    /// <summary>
    /// Axial coordinate system
    /// Description <see href="https://www.redblobgames.com/grids/hexagons/">here</see>
    /// </summary>
    public readonly struct AxialCoordinate : IEquatable<AxialCoordinate>
    {
        private readonly int _hash;

        public AxialCoordinate(int q, int r)
        {
            Q = q;
            R = r;
            _hash = HashCode.Combine(Q, R);
        }

        public int Q { get; }
        public int R { get; }
        
        /// <summary>
        /// Estimated path distance without obstacles.
        /// </summary>
        public double DistanceEstimate() 
            => Math.Max(Math.Abs(Q + R), Math.Max(Math.Abs(Q), Math.Abs(R)));

        public bool Equals(AxialCoordinate other) => Q == other.Q && R == other.R;
        public override bool Equals(object obj) => obj is AxialCoordinate other && Equals(other);

        public override int GetHashCode() => _hash;
        public override string ToString() => $"({Q}, {R})";

        public static AxialCoordinate operator +(AxialCoordinate a, AxialCoordinate b) => new(a.Q + b.Q, a.R + b.R);
        public static AxialCoordinate operator -(AxialCoordinate a, AxialCoordinate b) => new(a.Q - b.Q, a.R - b.R);
        public static bool operator ==(AxialCoordinate a, AxialCoordinate b) => a.Q == b.Q && a.R == b.R;
        public static bool operator !=(AxialCoordinate a, AxialCoordinate b) => !(a == b);
    }
}