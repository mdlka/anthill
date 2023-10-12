using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace YellowSquad.HexMath.Tests
{
    public class AxialCoordinateExtensionsTests
    {
        [TestCaseSource(nameof(Vector2ToAxialCoordinateTestCases))]
        public void Vector2ToAxialCoordinate_Vector2WithHexGridScale1_ReturnAxialCoordinate(Vector2 vector2, AxialCoordinate result)
        {
            Assert.True(vector2.ToAxialCoordinate() == result);
        }
        
        [TestCaseSource(nameof(AxialCoordinateToVector2TestCases))]
        public void AxialCoordinateToVector2_AxialCoordinateWithHexGridScale1_ReturnVector2(AxialCoordinate coordinate, Vector2 result)
        {
            Assert.True(coordinate.ToVector2() == result);
        }

        [TestCaseSource(nameof(HexCornerPositionTestCases))]
        public void HexCornerPosition_AxialCoordinateWithHexGridScale1_ReturnCornerPosition(AxialCoordinate coordinate, int cornerIndex, Vector3 result)
        {
            Assert.True(coordinate.HexCornerPosition(cornerIndex) == result);
        }

        public static IEnumerable<TestCaseData> Vector2ToAxialCoordinateTestCases
        {
            get
            {
                yield return new TestCaseData(new Vector2(3, 6), new AxialCoordinate(2, 2));
                yield return new TestCaseData(new Vector2(-3, -6), new AxialCoordinate(-2, -2));
                yield return new TestCaseData(new Vector2(0, 0), new AxialCoordinate(0, 0));
            }
        }
        
        public static IEnumerable<TestCaseData> AxialCoordinateToVector2TestCases
        {
            get
            {
                yield return new TestCaseData(new AxialCoordinate(2, 2), new Vector2(3, 3 * Mathf.Sqrt(3)));
                yield return new TestCaseData(new AxialCoordinate(-2, -2), new Vector2(-3, -3 * Mathf.Sqrt(3)));
                yield return new TestCaseData(new AxialCoordinate(0, 0), new Vector2(0, 0));
            }
        }
        
        public static IEnumerable<TestCaseData> HexCornerPositionTestCases
        {
            get
            {
                yield return new TestCaseData(new AxialCoordinate(0, 0), 0, new Vector3(1, 0, 0));
                yield return new TestCaseData(new AxialCoordinate(0, 0), 1, new Vector3(0.5f, 0, Mathf.Sqrt(3)/2));
                yield return new TestCaseData(new AxialCoordinate(0, 0), 2, new Vector3(-0.5f, 0, Mathf.Sqrt(3)/2));
                yield return new TestCaseData(new AxialCoordinate(0, 0), 3, new Vector3(-1, 0, 0));
                yield return new TestCaseData(new AxialCoordinate(0, 0), 4, new Vector3(-0.5f, 0, -Mathf.Sqrt(3)/2));
                yield return new TestCaseData(new AxialCoordinate(0, 0), 5, new Vector3(0.5f, 0, -Mathf.Sqrt(3)/2));
                yield return new TestCaseData(new AxialCoordinate(2, 2), 3, new Vector3(2f, 0, 3 * Mathf.Sqrt(3)));
            }
        }
    }
}