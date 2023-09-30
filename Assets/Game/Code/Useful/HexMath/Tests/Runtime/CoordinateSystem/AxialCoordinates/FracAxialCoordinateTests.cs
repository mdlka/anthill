using System.Collections.Generic;
using NUnit.Framework;

namespace YellowSquad.HexMath.Tests
{
    public class FracAxialCoordinateTests
    {
        [TestCaseSource(nameof(RoundFracAxialCoordinateTestCases))]
        public void AxialRound_FracAxialCoordinate_ReturnRoundedAxialCoordinate(FracAxialCoordinate frac, AxialCoordinate result)
        {
            Assert.True(frac.AxialRound() == result);
        }
        
        [Test]
        public void Equals_CoordinateWithItself_ShouldReturnTrue()
        {
            FracAxialCoordinate coordinate = new FracAxialCoordinate(1, 2);
            
            Assert.True(coordinate.Equals(coordinate));
        }
        
        [TestCaseSource(nameof(TwoIdenticalCoordinatesTestCases))]
        public void Equals_TwoIdenticalCoordinates_ShouldReturnTrue(FracAxialCoordinate first, FracAxialCoordinate second)
        {
            Assert.True(first.Equals(second));
            Assert.True(second.Equals(first));
        }
        
        [TestCaseSource(nameof(TwoDifferentCoordinatesTestCases))]
        public void Equals_TwoDifferentCoordinates_ShouldReturnFalse(FracAxialCoordinate first, FracAxialCoordinate second)
        {
            Assert.False(first.Equals(second));
            Assert.False(second.Equals(first));
        }

        [TestCaseSource(nameof(SumOfTwoCoordinatesTestCases))]
        public void PlusOperator_TwoCoordinates_ReturnCoordinateWithComponentsEqualToSum
            (FracAxialCoordinate first, FracAxialCoordinate second, FracAxialCoordinate result)
        {
            Assert.True(first + second == result);
        }
        
        [TestCaseSource(nameof(SumOfTwoCoordinatesTestCases))]
        public void PlusOperator_TwoCoordinates_SumOfCoordinatesShouldBeCommutative
            (FracAxialCoordinate first, FracAxialCoordinate second, FracAxialCoordinate result)
        {
            Assert.True(first + second == result);
            Assert.True(second + first == result);
        }
        
        [TestCaseSource(nameof(DifferenceOfTwoCoordinatesTestCases))]
        public void MinusOperator_TwoCoordinates_ReturnCoordinateWithComponentsEqualToDifference
            (FracAxialCoordinate first, FracAxialCoordinate second, FracAxialCoordinate result)
        {
            Assert.True(first - second == result);
        }
        
        public static IEnumerable<TestCaseData> RoundFracAxialCoordinateTestCases
        {
            get
            {
                yield return new TestCaseData(new FracAxialCoordinate(1.001f, 2.53f), new AxialCoordinate(1, 3));
                yield return new TestCaseData(new FracAxialCoordinate(-1.001f, -2.53f), new AxialCoordinate(-1, -3));
                yield return new TestCaseData(new FracAxialCoordinate(1.001f, -2.53f), new AxialCoordinate(1, -3));
                yield return new TestCaseData(new FracAxialCoordinate(-1.001f, 2.53f), new AxialCoordinate(-1, 3));
                yield return new TestCaseData(new FracAxialCoordinate(-0.053f, 0.051f), new AxialCoordinate(0, 0));
                yield return new TestCaseData(new FracAxialCoordinate(2, 3), new AxialCoordinate(2, 3));
            }
        }

        public static IEnumerable<TestCaseData> TwoIdenticalCoordinatesTestCases
        {
            get
            {
                yield return new TestCaseData(new FracAxialCoordinate(1, 2), new FracAxialCoordinate(1, 2));
                yield return new TestCaseData(new FracAxialCoordinate(-1, 2), new FracAxialCoordinate(-1, 2));
                yield return new TestCaseData(new FracAxialCoordinate(1, -2), new FracAxialCoordinate(1, -2));
                yield return new TestCaseData(new FracAxialCoordinate(-1, -2), new FracAxialCoordinate(-1, -2));
                yield return new TestCaseData(new FracAxialCoordinate(0, 0), new FracAxialCoordinate(0, 0));
                yield return new TestCaseData(new FracAxialCoordinate(1.35f, -4.51f), new FracAxialCoordinate(1.35f, -4.51f));
                yield return new TestCaseData(new FracAxialCoordinate(-1.35f, 4.51f), new FracAxialCoordinate(-1.35f, 4.51f));
            }
        }
        
        public static IEnumerable<TestCaseData> TwoDifferentCoordinatesTestCases
        {
            get
            {
                yield return new TestCaseData(new FracAxialCoordinate(1, 2), new FracAxialCoordinate(2, 1));
                yield return new TestCaseData(new FracAxialCoordinate(-1, 2), new FracAxialCoordinate(2, -1));
                yield return new TestCaseData(new FracAxialCoordinate(1, -2), new FracAxialCoordinate(-2, 1));
                yield return new TestCaseData(new FracAxialCoordinate(-1, -2), new FracAxialCoordinate(-2, -1));
                yield return new TestCaseData(new FracAxialCoordinate(-1.45f, -2.1f), new FracAxialCoordinate(-2.51f, -1.51f));
                yield return new TestCaseData(new FracAxialCoordinate(1.11f, 2.123f), new FracAxialCoordinate(1.12f, 2.124f));
                yield return new TestCaseData(new FracAxialCoordinate(1.10001f, 2.12301f), new FracAxialCoordinate(1.10f, 2.123f));
            }
        }
        
        public static IEnumerable<TestCaseData> SumOfTwoCoordinatesTestCases
        {
            get
            {
                yield return new TestCaseData(new FracAxialCoordinate(1, 2), new FracAxialCoordinate(2, 1), new FracAxialCoordinate(3, 3));
                yield return new TestCaseData(new FracAxialCoordinate(-1, 2), new FracAxialCoordinate(2, -1), new FracAxialCoordinate(1, 1));
                yield return new TestCaseData(new FracAxialCoordinate(1, -2), new FracAxialCoordinate(-2, 1), new FracAxialCoordinate(-1, -1));
                yield return new TestCaseData(new FracAxialCoordinate(-1, -2), new FracAxialCoordinate(-2, -1), new FracAxialCoordinate(-3, -3));
                yield return new TestCaseData(new FracAxialCoordinate(0, 0), new FracAxialCoordinate(0, 0), new FracAxialCoordinate(0, 0));
                yield return new TestCaseData(new FracAxialCoordinate(2.35f, 1.45f), new FracAxialCoordinate(0.65f, 0.55f), new FracAxialCoordinate(3, 2));
                yield return new TestCaseData(new FracAxialCoordinate(-2.35f, -1.45f), new FracAxialCoordinate(-0.65f, -0.55f), new FracAxialCoordinate(-3, -2));
                yield return new TestCaseData(new FracAxialCoordinate(2.35f, 1.45f), new FracAxialCoordinate(0.75f, 0.65f), new FracAxialCoordinate(3.1f, 2.1f));
            }
        }
        
        public static IEnumerable<TestCaseData> DifferenceOfTwoCoordinatesTestCases
        {
            get
            {
                yield return new TestCaseData(new FracAxialCoordinate(1, 2), new FracAxialCoordinate(2, 1), new FracAxialCoordinate(-1, 1));
                yield return new TestCaseData(new FracAxialCoordinate(-1, 2), new FracAxialCoordinate(2, -1), new FracAxialCoordinate(-3, 3));
                yield return new TestCaseData(new FracAxialCoordinate(1, -2), new FracAxialCoordinate(-2, 1), new FracAxialCoordinate(3, -3));
                yield return new TestCaseData(new FracAxialCoordinate(-1, -2), new FracAxialCoordinate(-2, -1), new FracAxialCoordinate(1, -1));
                yield return new TestCaseData(new FracAxialCoordinate(0, 0), new FracAxialCoordinate(0, 0), new FracAxialCoordinate(0, 0));
                yield return new TestCaseData(new FracAxialCoordinate(2.35f, 1.45f), new FracAxialCoordinate(0.35f, 0.45f), new FracAxialCoordinate(2, 1));
                yield return new TestCaseData(new FracAxialCoordinate(-2.35f, -1.45f), new FracAxialCoordinate(-0.35f, -0.45f), new FracAxialCoordinate(-2, -1));
                yield return new TestCaseData(new FracAxialCoordinate(2.35f, 1.45f), new FracAxialCoordinate(0.45f, 0.55f), new FracAxialCoordinate(1.9f, 0.9f));
            }
        }
    }
}