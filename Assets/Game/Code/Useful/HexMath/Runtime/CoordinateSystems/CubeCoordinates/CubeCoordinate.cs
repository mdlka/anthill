using System;

namespace YellowSquad.HexMath
{
    /// <summary>
    /// Cube coordinate system
    /// Description <see href="https://www.redblobgames.com/grids/hexagons/">here</see>
    /// </summary>
    public readonly struct CubeCoordinate : IEquatable<CubeCoordinate>
    {
        private readonly int _hash;

        public CubeCoordinate(int q, int r) : this(q, r, -q - r) { }

        public CubeCoordinate(int q, int r, int s)
        {
            if (q + r + s != 0)
                throw new ArgumentException("q + r + s must equals to 0");
            
            Q = q;
            R = r;
            S = s;
            _hash = HashCode.Combine(Q, R, S);
        }

        public int Q { get; }
        public int R { get; }
        public int S { get; }
        
        public AxialCoordinate ToAxial() => new AxialCoordinate(Q, R);

        public bool Equals(CubeCoordinate other)
            => Q == other.Q && R == other.R && S == other.S;

        public override bool Equals(object obj) 
            => obj is CubeCoordinate other && Equals(other);

        public override int GetHashCode() => _hash;
        public override string ToString() => $"({Q}, {R}, {S})";
        
        public static CubeCoordinate operator +(CubeCoordinate a, CubeCoordinate b) => new CubeCoordinate(a.Q + b.Q, a.R + b.R, a.S + b.S);
        public static CubeCoordinate operator -(CubeCoordinate a, CubeCoordinate b) => new CubeCoordinate(a.Q - b.Q, a.R - b.R, a.S - b.S);
        public static bool operator ==(CubeCoordinate a, CubeCoordinate b) => a.Equals(b);
        public static bool operator !=(CubeCoordinate a, CubeCoordinate b) => !(a == b);
    }
}