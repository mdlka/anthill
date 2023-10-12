using System;
using System.Collections.Generic;
using System.Linq;
using YellowSquad.HexMath;
using NUnit.Framework;

namespace YellowSquad.Anthill.Core.AStarPathfinding.Tests
{
    public class PathAndMovePoliciesTests
    {
        [TestCaseSource(nameof(WithoutObstaclesMovePolicies))]
        public void Calculate_WithoutObstacles_ReturnNearestPath(IMovePolicy movePolicy)
        {
            var path = new Path(movePolicy);

            IReadOnlyList<AxialCoordinate> result;

            path.Calculate(new AxialCoordinate(0, 0), new AxialCoordinate(3, 6), out result);
            Assert.AreEqual(10, result.Count);

            path.Calculate(new AxialCoordinate(-1, -2), new AxialCoordinate(1, -1), out result);
            Assert.AreEqual(4, result.Count);

            path.Calculate(new AxialCoordinate(-3, 2), new AxialCoordinate(0, 2), out result);
            Assert.AreEqual(4, result.Count);

            path.Calculate(new AxialCoordinate(-3, 3), new AxialCoordinate(3, -3), out result);
            Assert.AreEqual(7, result.Count);
        }

        [TestCaseSource(nameof(WithObstaclesMovePolicies))]
        public void Calculate_WithObstacles_ShouldFindNearestPath(IMovePolicy movePolicy)
        {
            var path = new Path(movePolicy);

            IReadOnlyList<AxialCoordinate> result;

            path.Calculate(new AxialCoordinate(0, 0), new AxialCoordinate(3, 1), out result);
            Assert.AreEqual(6, result.Count);

            foreach (var point in result)
                Assert.IsTrue(movePolicy.CanMove(point));

            path.Calculate(new AxialCoordinate(0, 3), new AxialCoordinate(2, -2), out result);
            Assert.AreEqual(6, result.Count);

            foreach (var point in result)
                Assert.IsTrue(movePolicy.CanMove(point));

            path.Calculate(new AxialCoordinate(5, 0), new AxialCoordinate(-2, 1), out result);
            Assert.AreEqual(9, result.Count);

            foreach (var point in result)
                Assert.IsTrue(movePolicy.CanMove(point));
        }

        [TestCaseSource(nameof(PathIsBlockedMovePolicies))]
        public void Calculate_PathIsBlocked_ShouldNotFindPath(IMovePolicy movePolicy)
        {
            var path = new Path(movePolicy, maxSteps: 100);

            IReadOnlyList<AxialCoordinate> result;

            path.Calculate(new AxialCoordinate(0, 0), new AxialCoordinate(3, 1), out result);
            Assert.AreEqual(0, result.Count);

            path.Calculate(new AxialCoordinate(0, 3), new AxialCoordinate(3, 0), out result);
            Assert.AreEqual(0, result.Count);

            path.Calculate(new AxialCoordinate(-2, 1), new AxialCoordinate(2, 1), out result);
            Assert.AreEqual(0, result.Count);
        }
        
        private static HashSet<AxialCoordinate> Map
        {
            get
            {
                var map = new HashSet<AxialCoordinate>();
                
                for (int q = -10; q < 10; q++)
                    for (int r = -10; r < 10; r++)
                        map.Add(new AxialCoordinate(q, r));

                return map;
            }
        }

        public static IEnumerable<TestCaseData> WithoutObstaclesMovePolicies
        {
            get
            {
                yield return new TestCaseData(new ObstacleMovePolicy(Array.Empty<AxialCoordinate>()));
                yield return new TestCaseData(new AvailableMovePolicy(Map));
            }
        }
        
        public static IEnumerable<TestCaseData> WithObstaclesMovePolicies
        {
            get
            {
                var obstacles = new[]
                {
                    new AxialCoordinate(-1, 1),
                    new AxialCoordinate(2, -1),
                    new AxialCoordinate(2, 0),
                    new AxialCoordinate(1, 1),
                };
                
                yield return new TestCaseData(new ObstacleMovePolicy(obstacles));
                yield return new TestCaseData(new AvailableMovePolicy(Map.Except(obstacles)));
            }
        }
        
        public static IEnumerable<TestCaseData> PathIsBlockedMovePolicies
        {
            get
            {
                var obstacles = new[]
                {
                    new AxialCoordinate(-1, 1),
                    new AxialCoordinate(2, -1),
                    new AxialCoordinate(2, 0),
                    new AxialCoordinate(1, 1),
                    new AxialCoordinate(1, 2),
                    new AxialCoordinate(2, 2),
                    new AxialCoordinate(3, 2),
                    new AxialCoordinate(4, 1),
                    new AxialCoordinate(4, 0),
                    new AxialCoordinate(4, -1),
                    new AxialCoordinate(3, -1),
                };
                
                yield return new TestCaseData(new ObstacleMovePolicy(obstacles));
                yield return new TestCaseData(new AvailableMovePolicy(Map.Except(obstacles)));
            }
        }
    }
}