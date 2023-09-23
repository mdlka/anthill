using UnityEngine;

namespace YellowSquad.HexMath
{
    public static class HexExtensions
    {
        public static Vector3 HexCornerPosition(this AxialCoordinate axialCoordinate, int cornerIndex, float hexGridScale = 1f)
        {
            Vector3 centerPosition = axialCoordinate.ConvertToVector3(hexGridScale);
            
            float angle = 60f * cornerIndex;
            float radians = angle * Mathf.Deg2Rad;
            
            return new Vector3(
                centerPosition.x + hexGridScale * Mathf.Cos(radians), 
                0f, 
                centerPosition.z + hexGridScale * Mathf.Sin(radians));
        }
        
        public static Vector3 CornerPosition(this Hex hex, int cornerIndex, float hexGridScale = 1f)
        {
            return hex.Position.HexCornerPosition(cornerIndex, hexGridScale);
        }
    }
}