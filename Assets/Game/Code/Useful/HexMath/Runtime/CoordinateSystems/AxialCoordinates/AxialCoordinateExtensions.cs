﻿using UnityEngine;

namespace YellowSquad.HexMath
{
    public static class AxialCoordinateExtensions
    {
        private const float Sqrt3 = 1.73205080757f;

        public static AxialCoordinate ToAxialCoordinate(this Vector2 value, float hexGridScale = 1f)
        {
            float q = 2f / 3f * value.x / hexGridScale;
            float r = (-1f / 3f * value.x + Sqrt3 / 3f * value.y) / hexGridScale;

            return new FracAxialCoordinate(q, r).AxialRound();
        }
        
        public static AxialCoordinate ToAxialCoordinate(this Vector3 value, float hexGridScale = 1f)
        {
            return new Vector2(value.x, value.z).ToAxialCoordinate(hexGridScale);
        }

        public static Vector2 ToVector2(this AxialCoordinate axialCoordinate, float hexGridScale = 1f)
        {
            float x = axialCoordinate.Q * 1.5f * hexGridScale;
            float y = hexGridScale * Sqrt3 * (axialCoordinate.R + axialCoordinate.Q / 2f);
            
            return new Vector2(x, y);
        }

        public static Vector3 ToVector3(this AxialCoordinate axialCoordinate, float hexGridScale = 1f)
        {
            Vector2 vector2 = axialCoordinate.ToVector2(hexGridScale);
            return new Vector3(vector2.x, 0, vector2.y);
        }
        
        public static Vector3 HexCornerPosition(this AxialCoordinate axialCoordinate, int cornerIndex, float hexGridScale = 1f)
        {
            Vector3 centerPosition = axialCoordinate.ToVector3(hexGridScale);
            
            float angle = 60f * cornerIndex;
            float radians = angle * Mathf.Deg2Rad;
            
            return new Vector3(
                centerPosition.x + hexGridScale * Mathf.Cos(radians), 
                0f, 
                centerPosition.z + hexGridScale * Mathf.Sin(radians));
        }
    }
}