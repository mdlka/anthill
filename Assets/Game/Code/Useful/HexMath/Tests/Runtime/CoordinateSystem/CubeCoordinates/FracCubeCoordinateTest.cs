using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace YellowSquad.HexMath.Tests
{
    public class FracCubeCoordinateTest
    {
        [Test]
        public void Constructor_SumOfCoordinatesIsNotEqualTo0_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => new FracCubeCoordinate(1.01f, 3.205f, 5));
            Assert.Throws<ArgumentException>(() => new FracCubeCoordinate(-1, 0.001f, 1));
            Assert.Throws<ArgumentException>(() => new FracCubeCoordinate(-5, 0, 5.001f));
        }
        
        [TestCaseSource(nameof(RoundFracCubeCoordinateTestCases))]
        public void CubeRound_FracCubeCoordinate_ReturnRoundedCubeCoordinate(FracCubeCoordinate frac, CubeCoordinate result)
        {
            Assert.True(frac.CubeRound() == result);
        }
        
        [Test]
        public void Equals_CoordinateWithItself_ShouldReturnTrue()
        {
            FracCubeCoordinate coordinate = new FracCubeCoordinate(1.01f, 2.01f);
            
            Assert.True(coordinate.Equals(coordinate));
        }
        
        [TestCaseSource(nameof(TwoIdenticalCoordinatesTestCases))]
        public void Equals_TwoIdenticalCoordinates_ShouldReturnTrue(FracCubeCoordinate first, FracCubeCoordinate second)
        {
            Assert.True(first.Equals(second));
            Assert.True(second.Equals(first));
        }
        
        [TestCaseSource(nameof(TwoDifferentCoordinatesTestCases))]
        public void Equals_TwoDifferentCoordinates_ShouldReturnFalse(FracCubeCoordinate first, FracCubeCoordinate second)
        {
            Assert.False(first.Equals(second));
            Assert.False(second.Equals(first));
        }

        [TestCaseSource(nameof(SumOfTwoCoordinatesTestCases))]
        public void PlusOperator_TwoCoordinates_ReturnCoordinateWithComponentsEqualToSum
            (FracCubeCoordinate first, FracCubeCoordinate second, FracCubeCoordinate result)
        {
            Assert.True(first + second == result);
        }
        
        [TestCaseSource(nameof(SumOfTwoCoordinatesTestCases))]
        public void PlusOperator_TwoCoordinates_SumOfCoordinatesShouldBeCommutative
            (FracCubeCoordinate first, FracCubeCoordinate second, FracCubeCoordinate result)
        {
            Assert.True(first + second == result);
            Assert.True(second + first == result);
        }
        
        [TestCaseSource(nameof(DifferenceOfTwoCoordinatesTestCases))]
        public void MinusOperator_TwoCoordinates_ReturnCoordinateWithComponentsEqualToDifference
            (FracCubeCoordinate first, FracCubeCoordinate second, FracCubeCoordinate result)
        {
            Assert.True(first - second == result);
        }
        
        public static IEnumerable<TestCaseData> RoundFracCubeCoordinateTestCases
        {
            get
            {
                yield return new TestCaseData(new FracCubeCoordinate(1.001f, 2.53f), new CubeCoordinate(1, 3));
                yield return new TestCaseData(new FracCubeCoordinate(-1.001f, -2.53f), new CubeCoordinate(-1, -3));
                yield return new TestCaseData(new FracCubeCoordinate(1.001f, -2.53f), new CubeCoordinate(1, -3));
                yield return new TestCaseData(new FracCubeCoordinate(-1.001f, 2.53f), new CubeCoordinate(-1, 3));
                yield return new TestCaseData(new FracCubeCoordinate(-0.053f, 0.051f), new CubeCoordinate(0, 0));
                yield return new TestCaseData(new FracCubeCoordinate(2, 3), new CubeCoordinate(2, 3));
            }
        }
        
        public static IEnumerable<TestCaseData> TwoIdenticalCoordinatesTestCases
        {
            get
            {
                yield return new TestCaseData(new FracCubeCoordinate(1.001f, 2), new FracCubeCoordinate(1.001f, 2));
                yield return new TestCaseData(new FracCubeCoordinate(-1, 2), new FracCubeCoordinate(-1, 2));
                yield return new TestCaseData(new FracCubeCoordinate(1, -2.0005f), new FracCubeCoordinate(1, -2.0005f));
                yield return new TestCaseData(new FracCubeCoordinate(-1.153f, -2.100001f), new FracCubeCoordinate(-1.153f, -2.100001f));
                yield return new TestCaseData(new FracCubeCoordinate(0, 0), new FracCubeCoordinate(0, 0));
            }
        }
        
        public static IEnumerable<TestCaseData> TwoDifferentCoordinatesTestCases
        {
            get
            {
                yield return new TestCaseData(new FracCubeCoordinate(1, 2), new FracCubeCoordinate(2, 1));
                yield return new TestCaseData(new FracCubeCoordinate(-1.0001f, 2), new FracCubeCoordinate(-1.001f, 2));
                yield return new TestCaseData(new FracCubeCoordinate(1.05135f, -2.01515f), new FracCubeCoordinate(1.051351f, -2.0151f));
                yield return new TestCaseData(new FracCubeCoordinate(-1.123f, -2), new FracCubeCoordinate(-2, -1));
            }
        }
        
        public static IEnumerable<TestCaseData> SumOfTwoCoordinatesTestCases
        {
            get
            {
                yield return new TestCaseData(new FracCubeCoordinate(1f, 2, -3), new FracCubeCoordinate(2, 1, -3), new FracCubeCoordinate(3, 3, -6));
                yield return new TestCaseData(new FracCubeCoordinate(-1, 2, -1), new FracCubeCoordinate(2, -1, -1), new FracCubeCoordinate(1, 1, -2));
                yield return new TestCaseData(new FracCubeCoordinate(1, -5, 4), new FracCubeCoordinate(-3, 1, 2), new FracCubeCoordinate(-2, -4, 6));
                yield return new TestCaseData(new FracCubeCoordinate(-1, -2, 3), new FracCubeCoordinate(-2, -1, 3), new FracCubeCoordinate(-3, -3, 6));
                yield return new TestCaseData(new FracCubeCoordinate(0, 0, 0), new FracCubeCoordinate(0, 0, 0), new FracCubeCoordinate(0, 0, 0));
                yield return new TestCaseData(new FracCubeCoordinate(2.35f, 1.45f), new FracCubeCoordinate(0.65f, 0.55f), new FracCubeCoordinate(3, 2));
                yield return new TestCaseData(new FracCubeCoordinate(-2.35f, -1.45f), new FracCubeCoordinate(-0.65f, -0.55f), new FracCubeCoordinate(-3, -2));
                yield return new TestCaseData(new FracCubeCoordinate(2.35f, 1.45f), new FracCubeCoordinate(0.75f, 0.65f), new FracCubeCoordinate(3.1f, 2.1f));
            }
        }
        
        public static IEnumerable<TestCaseData> DifferenceOfTwoCoordinatesTestCases
        {
            get
            {
                yield return new TestCaseData(new FracCubeCoordinate(1, 2, -3), new FracCubeCoordinate(2, 1, -3), new FracCubeCoordinate(-1, 1, 0));
                yield return new TestCaseData(new FracCubeCoordinate(-1, 5, -4), new FracCubeCoordinate(3, -1, -2), new FracCubeCoordinate(-4, 6, -2));
                yield return new TestCaseData(new FracCubeCoordinate(1, -2, 1), new FracCubeCoordinate(-2, 1, 1), new FracCubeCoordinate(3, -3, 0));
                yield return new TestCaseData(new FracCubeCoordinate(-1, -2, 3), new FracCubeCoordinate(-2, -1, 3), new FracCubeCoordinate(1, -1, 0));
                yield return new TestCaseData(new FracCubeCoordinate(0, 0, 0), new FracCubeCoordinate(0, 0, 0), new FracCubeCoordinate(0, 0, 0));
                yield return new TestCaseData(new FracCubeCoordinate(2.35f, 1.45f), new FracCubeCoordinate(0.35f, 0.45f), new FracCubeCoordinate(2, 1));
                yield return new TestCaseData(new FracCubeCoordinate(-2.35f, -1.45f), new FracCubeCoordinate(-0.35f, -0.45f), new FracCubeCoordinate(-2, -1));
                yield return new TestCaseData(new FracCubeCoordinate(2.35f, 1.45f), new FracCubeCoordinate(0.45f, 0.55f), new FracCubeCoordinate(1.9f, 0.9f));
            }
        }
    }
}