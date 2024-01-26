namespace YellowSquad.HexMath
{
    public static class HBezier 
    {
        public static FracAxialCoordinate GetPoint(FracAxialCoordinate a, FracAxialCoordinate b, FracAxialCoordinate c, float t) 
        {
            float r = 1f - t;
            return r * r * a + 2f * r * t * b + t * t * c;
        }
        
        /// <remarks>
        /// Can be used to determine orientation
        /// </remarks>
        public static FracAxialCoordinate GetDerivative(FracAxialCoordinate a, FracAxialCoordinate b, FracAxialCoordinate c, float t) 
        {
            return 2f * ((1f - t) * (b - a) + t * (c - b));
        }
    }
}