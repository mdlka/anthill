using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace YellowSquad.HexMath.Tests
{
    public class CubeCoordinateTests
    {
        [Test]
        public void Constructor_SumOfCoordinatesIsNotEqualTo0_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => new CubeCoordinate(1, 3, 5));
            Assert.Throws<ArgumentException>(() => new CubeCoordinate(-1, 1, 1));
            Assert.Throws<ArgumentException>(() => new CubeCoordinate(-5, 2, 5));
        }
        
        [Test]
        public void Equals_CoordinateWithItself_ShouldReturnTrue()
        {
            CubeCoordinate coordinate = new CubeCoordinate(1, 2);
            
            Assert.True(coordinate.Equals(coordinate));
        }
        
        [TestCaseSource(nameof(TwoIdenticalCoordinatesTestCases))]
        public void Equals_TwoIdenticalCoordinates_ShouldReturnTrue(CubeCoordinate first, CubeCoordinate second)
        {
            Assert.True(first.Equals(second));
            Assert.True(second.Equals(first));
        }
        
        [TestCaseSource(nameof(TwoDifferentCoordinatesTestCases))]
        public void Equals_TwoDifferentCoordinates_ShouldReturnFalse(CubeCoordinate first, CubeCoordinate second)
        {
            Assert.False(first.Equals(second));
            Assert.False(second.Equals(first));
        }

        [TestCaseSource(nameof(SumOfTwoCoordinatesTestCases))]
        public void PlusOperator_TwoCoordinates_ReturnCoordinateWithComponentsEqualToSum
            (CubeCoordinate first, CubeCoordinate second, CubeCoordinate result)
        {
            Assert.True(first + second == result);
        }
        
        [TestCaseSource(nameof(SumOfTwoCoordinatesTestCases))]
        public void PlusOperator_TwoCoordinates_SumOfCoordinatesShouldBeCommutative
            (CubeCoordinate first, CubeCoordinate second, CubeCoordinate result)
        {
            Assert.True(first + second == result);
            Assert.True(second + first == result);
        }
        
        [TestCaseSource(nameof(DifferenceOfTwoCoordinatesTestCases))]
        public void MinusOperator_TwoCoordinates_ReturnCoordinateWithComponentsEqualToDifference
            (CubeCoordinate first, CubeCoordinate second, CubeCoordinate result)
        {
            Assert.True(first - second == result);
        }
        
        public static IEnumerable<TestCaseData> TwoIdenticalCoordinatesTestCases
        {
            get
            {
                yield return new TestCaseData(new CubeCoordinate(1, 2), new CubeCoordinate(1, 2));
                yield return new TestCaseData(new CubeCoordinate(-1, 2), new CubeCoordinate(-1, 2));
                yield return new TestCaseData(new CubeCoordinate(1, -2), new CubeCoordinate(1, -2));
                yield return new TestCaseData(new CubeCoordinate(-1, -2), new CubeCoordinate(-1, -2));
                yield return new TestCaseData(new CubeCoordinate(0, 0), new CubeCoordinate(0, 0));
            }
        }
        
        public static IEnumerable<TestCaseData> TwoDifferentCoordinatesTestCases
        {
            get
            {
                yield return new TestCaseData(new CubeCoordinate(1, 2), new CubeCoordinate(2, 1));
                yield return new TestCaseData(new CubeCoordinate(-1, 2), new CubeCoordinate(2, -1));
                yield return new TestCaseData(new CubeCoordinate(1, -2), new CubeCoordinate(-2, 1));
                yield return new TestCaseData(new CubeCoordinate(-1, -2), new CubeCoordinate(-2, -1));
            }
        }
        
        public static IEnumerable<TestCaseData> SumOfTwoCoordinatesTestCases
        {
            get
            {
                yield return new TestCaseData(new CubeCoordinate(1, 2, -3), new CubeCoordinate(2, 1, -3), new CubeCoordinate(3, 3, -6));
                yield return new TestCaseData(new CubeCoordinate(-1, 2, -1), new CubeCoordinate(2, -1, -1), new CubeCoordinate(1, 1, -2));
                yield return new TestCaseData(new CubeCoordinate(1, -5, 4), new CubeCoordinate(-3, 1, 2), new CubeCoordinate(-2, -4, 6));
                yield return new TestCaseData(new CubeCoordinate(-1, -2, 3), new CubeCoordinate(-2, -1, 3), new CubeCoordinate(-3, -3, 6));
                yield return new TestCaseData(new CubeCoordinate(0, 0, 0), new CubeCoordinate(0, 0, 0), new CubeCoordinate(0, 0, 0));
            }
        }
        
        public static IEnumerable<TestCaseData> DifferenceOfTwoCoordinatesTestCases
        {
            get
            {
                yield return new TestCaseData(new CubeCoordinate(1, 2, -3), new CubeCoordinate(2, 1, -3), new CubeCoordinate(-1, 1, 0));
                yield return new TestCaseData(new CubeCoordinate(-1, 5, -4), new CubeCoordinate(3, -1, -2), new CubeCoordinate(-4, 6, -2));
                yield return new TestCaseData(new CubeCoordinate(1, -2, 1), new CubeCoordinate(-2, 1, 1), new CubeCoordinate(3, -3, 0));
                yield return new TestCaseData(new CubeCoordinate(-1, -2, 3), new CubeCoordinate(-2, -1, 3), new CubeCoordinate(1, -1, 0));
                yield return new TestCaseData(new CubeCoordinate(0, 0, 0), new CubeCoordinate(0, 0, 0), new CubeCoordinate(0, 0, 0));
            }
        }
    }
}
