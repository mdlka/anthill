using System.Collections.Generic;
using NUnit.Framework;

namespace YellowSquad.HexMath.Tests
{
    public class AxialCoordinateTests
    {
        [Test]
        public void Equals_CoordinateWithItself_ShouldReturnTrue()
        {
            AxialCoordinate coordinate = new AxialCoordinate(1, 2);
            
            Assert.True(coordinate.Equals(coordinate));
        }
        
        [TestCaseSource(nameof(TwoIdenticalCoordinatesTestCases))]
        public void Equals_TwoIdenticalCoordinates_ShouldReturnTrue(AxialCoordinate first, AxialCoordinate second)
        {
            Assert.True(first.Equals(second));
            Assert.True(second.Equals(first));
        }
        
        [TestCaseSource(nameof(TwoDifferentCoordinatesTestCases))]
        public void Equals_TwoDifferentCoordinates_ShouldReturnFalse(AxialCoordinate first, AxialCoordinate second)
        {
            Assert.False(first.Equals(second));
            Assert.False(second.Equals(first));
        }

        [TestCaseSource(nameof(SumOfTwoCoordinatesTestCases))]
        public void PlusOperator_TwoCoordinates_ReturnCoordinateWithComponentsEqualToSum
            (AxialCoordinate first, AxialCoordinate second, AxialCoordinate result)
        {
            Assert.True(first + second == result);
        }
        
        [TestCaseSource(nameof(SumOfTwoCoordinatesTestCases))]
        public void PlusOperator_TwoCoordinates_SumOfCoordinatesShouldBeCommutative
            (AxialCoordinate first, AxialCoordinate second, AxialCoordinate result)
        {
            Assert.True(first + second == result);
            Assert.True(second + first == result);
        }
        
        [TestCaseSource(nameof(DifferenceOfTwoCoordinatesTestCases))]
        public void MinusOperator_TwoCoordinates_ReturnCoordinateWithComponentsEqualToDifference
            (AxialCoordinate first, AxialCoordinate second, AxialCoordinate result)
        {
            Assert.True(first - second == result);
        }
        
        public static IEnumerable<TestCaseData> TwoIdenticalCoordinatesTestCases
        {
            get
            {
                yield return new TestCaseData(new AxialCoordinate(1, 2), new AxialCoordinate(1, 2));
                yield return new TestCaseData(new AxialCoordinate(-1, 2), new AxialCoordinate(-1, 2));
                yield return new TestCaseData(new AxialCoordinate(1, -2), new AxialCoordinate(1, -2));
                yield return new TestCaseData(new AxialCoordinate(-1, -2), new AxialCoordinate(-1, -2));
                yield return new TestCaseData(new AxialCoordinate(0, 0), new AxialCoordinate(0, 0));
            }
        }
        
        public static IEnumerable<TestCaseData> TwoDifferentCoordinatesTestCases
        {
            get
            {
                yield return new TestCaseData(new AxialCoordinate(1, 2), new AxialCoordinate(2, 1));
                yield return new TestCaseData(new AxialCoordinate(-1, 2), new AxialCoordinate(2, -1));
                yield return new TestCaseData(new AxialCoordinate(1, -2), new AxialCoordinate(-2, 1));
                yield return new TestCaseData(new AxialCoordinate(-1, -2), new AxialCoordinate(-2, -1));
            }
        }
        
        public static IEnumerable<TestCaseData> SumOfTwoCoordinatesTestCases
        {
            get
            {
                yield return new TestCaseData(new AxialCoordinate(1, 2), new AxialCoordinate(2, 1), new AxialCoordinate(3, 3));
                yield return new TestCaseData(new AxialCoordinate(-1, 2), new AxialCoordinate(2, -1), new AxialCoordinate(1, 1));
                yield return new TestCaseData(new AxialCoordinate(1, -2), new AxialCoordinate(-2, 1), new AxialCoordinate(-1, -1));
                yield return new TestCaseData(new AxialCoordinate(-1, -2), new AxialCoordinate(-2, -1), new AxialCoordinate(-3, -3));
                yield return new TestCaseData(new AxialCoordinate(0, 0), new AxialCoordinate(0, 0), new AxialCoordinate(0, 0));
            }
        }
        
        public static IEnumerable<TestCaseData> DifferenceOfTwoCoordinatesTestCases
        {
            get
            {
                yield return new TestCaseData(new AxialCoordinate(1, 2), new AxialCoordinate(2, 1), new AxialCoordinate(-1, 1));
                yield return new TestCaseData(new AxialCoordinate(-1, 2), new AxialCoordinate(2, -1), new AxialCoordinate(-3, 3));
                yield return new TestCaseData(new AxialCoordinate(1, -2), new AxialCoordinate(-2, 1), new AxialCoordinate(3, -3));
                yield return new TestCaseData(new AxialCoordinate(-1, -2), new AxialCoordinate(-2, -1), new AxialCoordinate(1, -1));
                yield return new TestCaseData(new AxialCoordinate(0, 0), new AxialCoordinate(0, 0), new AxialCoordinate(0, 0));
            }
        }
    }
}
