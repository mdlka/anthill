using UnityEngine;

namespace YellowSquad.HexMath
{
    public static class AxialCoordinateExtensions
    {
        private const float Sqrt3 = 1.73205080757f;

        public static AxialCoordinate ConvertToAxialCoordinate(this Vector2 vector2, float hexGridScale)
        {
            float q = 2f / 3f * vector2.x / hexGridScale;
            float r = (-1f / 3f * vector2.x + Sqrt3 / 3f * vector2.y) / hexGridScale;

            return new FracAxialCoordinate(q, r).AxialRound();
        }
        
        public static AxialCoordinate ConvertToAxialCoordinate(this Vector3 vector3, float hexGridScale)
        {
            return ConvertToAxialCoordinate(new Vector2(vector3.x, vector3.z), hexGridScale);
        }

        public static Vector2 ConvertToVector2(this AxialCoordinate axialCoordinate, float hexGridScale)
        {
            float x = axialCoordinate.Q * 1.5f * hexGridScale;
            float y = hexGridScale * Sqrt3 * (axialCoordinate.R + axialCoordinate.Q / 2f);
            
            return new Vector2(x, y);
        }

        public static Vector3 ConvertToVector3(this AxialCoordinate axialCoordinate, float hexGridScale)
        {
            Vector2 vector2 = axialCoordinate.ConvertToVector2(hexGridScale);
            return new Vector3(vector2.x, 0, vector2.y);
        }
    }
}