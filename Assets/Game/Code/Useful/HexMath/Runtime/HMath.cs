using System;

namespace YellowSquad.HexMath
{
    public static class HMath
    {
        public static FracAxialCoordinate Lerp(AxialCoordinate a, AxialCoordinate b, float t)
        {
            return new FracAxialCoordinate(
                Lerp(a.Q, b.Q, t),
                Lerp(a.R, b.R, t));
        }
        
        public static FracAxialCoordinate Lerp(FracAxialCoordinate a, FracAxialCoordinate b, float t)
        {
            return new FracAxialCoordinate(
                Lerp(a.Q, b.Q, t),
                Lerp(a.R, b.R, t));
        }

        public static float Distance(FracAxialCoordinate a, FracAxialCoordinate b)
        {
            return (Math.Abs(a.Q - b.Q) + Math.Abs(a.Q + a.R - b.Q - b.R) + Math.Abs(a.R - b.R)) / 2f;
        }
        
        private static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }
    }
}