namespace YellowSquad.HexMath
{
    public static class AxialCoordinateMath
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
        
        private static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }
    }
}