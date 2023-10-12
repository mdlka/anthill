using System;
using NUnit.Framework;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap.Tests
{
    public class MapTests
    {
        private IHexMap _map;

        [SetUp]
        public void Initialize()
        {
            _map = new Map();
        }

        [Test]
        public void AddHex_DifferentPositions_ShouldAddHexAtTargetPosition()
        {
            var targetPositions = new AxialCoordinate[]
            {
                new(-1, 1), new(0, 1), new(1, 1),
                new(-1, 0), new(0, 0), new(1, 0),
                new(-1, -1), new(0, -1), new(1, -1),
            };

            foreach (var position in targetPositions)
            {
                _map.AddHex(position, new TestHex());
                Assert.True(_map.HasHexIn(position));
            }
        }
        
        [Test]
        public void AddHex_SamePositions_ShouldThrowException()
        {
            var targetPosition = new AxialCoordinate(1, 1);

            _map.AddHex(targetPosition, new TestHex());

            Assert.True(_map.HasHexIn(targetPosition));
            Assert.Throws<InvalidOperationException>(() => _map.AddHex(targetPosition, new TestHex()));
        }
        
        [Test]
        public void RemoveHex_InHexPosition_ShouldRemoveHexAtTargetPosition()
        {
            var targetPositions = new AxialCoordinate[]
            {
                new(-1, 1), new(0, 1), new(1, 1),
                new(-1, 0), new(0, 0), new(1, 0),
                new(-1, -1), new(0, -1), new(1, -1),
            };

            foreach (var position in targetPositions)
            {
                _map.AddHex(position, new TestHex());
                Assert.True(_map.HasHexIn(position));
            }

            foreach (var position in targetPositions)
            {
                _map.RemoveHex(position);
                Assert.False(_map.HasHexIn(position));
            }
        }
        
        [Test]
        public void RemoveHex_InEmptyPosition_ShouldThrowException()
        {
            var targetPosition = new AxialCoordinate(1, 1);

            Assert.False(_map.HasHexIn(targetPosition));
            Assert.Throws<InvalidOperationException>(() =>_map.RemoveHex(targetPosition));
        }
    }
}
