using System;

namespace YellowSquad.HexMath
{
    /// <summary>
    /// Cube coordinate system
    /// Description <see href="https://www.redblobgames.com/grids/hexagons/">here</see>
    /// </summary>
    public readonly struct FracCubeCoordinate : IEquatable<FracCubeCoordinate>
    {
        private readonly int _hash;

        public FracCubeCoordinate(float q, float r, float s)
        {
            Q = q;
            R = r;
            S = s;
            _hash = HashCode.Combine(Q, R, S);
        }

        public float Q { get; }
        public float R { get; }
        public float S { get; }
        
        public CubeCoordinate CubeRound()
        {
            double q = Math.Round(Q);
            double r = Math.Round(R);
            double s = Math.Round(S);

            double qDiff = Math.Abs(q - Q);
            double rDiff = Math.Abs(r - R);
            double sDiff = Math.Abs(s - S);

            if (qDiff > rDiff && qDiff > sDiff)
                q = -r - s;
            else if (rDiff > sDiff)
                r = -q - s;
            else
                s = -q - r;

            return new CubeCoordinate((int)q, (int)r, (int)s);
        }

        public bool Equals(FracCubeCoordinate other)
            => Math.Abs(Q - other.Q) < float.Epsilon && Math.Abs(R - other.R) < float.Epsilon && Math.Abs(S - other.S) < float.Epsilon;

        public override bool Equals(object obj) 
            => obj is FracCubeCoordinate other && Equals(other);

        public override int GetHashCode() => _hash;
        public override string ToString() => $"({Q}, {R}, {S})";
        
        public static FracCubeCoordinate operator +(FracCubeCoordinate a, FracCubeCoordinate b) => new FracCubeCoordinate(a.Q + b.Q, a.R + b.R, a.S + b.S);
        public static FracCubeCoordinate operator -(FracCubeCoordinate a, FracCubeCoordinate b) => new FracCubeCoordinate(a.Q - b.Q, a.R - b.R, a.S - b.S);
        public static bool operator ==(FracCubeCoordinate a, FracCubeCoordinate b) => a.Equals(b);
        public static bool operator !=(FracCubeCoordinate a, FracCubeCoordinate b) => !(a == b);
    }
}