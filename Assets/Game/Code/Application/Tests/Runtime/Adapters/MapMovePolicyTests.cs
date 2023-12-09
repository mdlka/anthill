using System.Collections.Generic;
using NUnit.Framework;
using YellowSquad.Anthill.Application.Adapters;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Application.Tests
{
    public class MapMovePolicyTests
    {
        [Test]
        public void CanMove_NoObstacle_ReturnTrue()
        {
            // given
            IHexMap map = new StubHexMap(new Dictionary<AxialCoordinate, bool>()
            {
                { new(0, 0), false }
            });

            var movePolicy = new MapMovePolicy(map);

            // when
            bool actual = movePolicy.CanMove(new AxialCoordinate(0, 0));
            
            // then
            Assert.IsTrue(actual);
        }
        
        [Test]
        public void CanMove_HasObstacle_ReturnFalse()
        {
            // given
            IHexMap map = new StubHexMap(new Dictionary<AxialCoordinate, bool>()
            {
                { new(0, 0), true }
            });

            var movePolicy = new MapMovePolicy(map);

            // when
            bool actual = movePolicy.CanMove(new AxialCoordinate(0, 0));
            
            // then
            Assert.IsFalse(actual);
        }

        [Test]
        public void CanMove_NoPosition_ReturnFalse()
        {
            // given
            IHexMap map = new StubHexMap(new Dictionary<AxialCoordinate, bool>()
            {
                { new(0, 0), true }
            });

            var movePolicy = new MapMovePolicy(map);

            // when
            bool actual = movePolicy.CanMove(new AxialCoordinate(0, 1));
            
            // then
            Assert.IsFalse(actual);
        }
    }
}
